using System.Collections.Generic;
using System.Linq;


namespace SciSharp.Language.Grammars.BottomUp
{
    public class LrParserBuilder<T>
        where T : Node, new()
    {
        private readonly Dictionary<Def<T>, HashSet<Token<T>>> firsts;
        private readonly Grammar<T> grammar;
        private readonly Dictionary<LrKernel<T>, LrState<T>> states;

        public LrParserBuilder(Grammar<T> grammar)
        {
            this.grammar = grammar;
            states = new Dictionary<LrKernel<T>, LrState<T>>();
            firsts = new Dictionary<Def<T>, HashSet<Token<T>>>();
        }

        public IActionGotoTable<T> Build()
        {
            LrState<T>[] lrStates;
            return Build(out lrStates);
        }

        public IActionGotoTable<T> Build(out LrState<T>[] lrStates)
        {
            // Símbolo inicial de la gramática aumentada (S')
            var start = new Def<T> {Name = grammar.StartSymbol.ToString() + "'"};

            // Calcular todos los firsts
            CalculateFirsts();

            // Generar todos los estados
            ManageStates(start);

            // Comprimir los estados si es posible
            lrStates = Compress(states.Values).ToArray();

            var table = new ActionGotoTable<T>();

            foreach (var state in lrStates)
            {
                // Chequear todos los Goto
                foreach (var gt in state.Gotos)
                {
                    // Es un no-terminal, llenar la tabla Goto
                    if (gt.Symbol.IsNonTerminal)
                    {
                        table.SetGoto(gt.Origin.Index, gt.Symbol, gt.Destination.Index);
                    }
                        // Es un terminal, llenar la tabla Action (shift)
                    else
                    {
                        var action = new ParsingAction<T>(gt.Destination.Index, ActionCode.Shift);
                        table.SetAction(gt.Origin.Index, (Token<T>) gt.Symbol, action);
                    }
                }
                // Chequear todos los Shift
                foreach (var lrItem in state)
                {
                    // Ver si el item tiene el punto al final
                    if (lrItem.Pointer == lrItem.Body.Count)
                    {
                        ParsingAction<T> action;

                        if (lrItem.Symbol == start)
                        {
                            // Si es S'->S, entonces es un Accept
                            action = new ParsingAction<T>(-1, ActionCode.Accept, lrItem.Symbol, lrItem.Body);
                        }
                        else
                        {
                            // Es un reduce común
                            action = new ParsingAction<T>(-1, ActionCode.Reduce, lrItem.Symbol, lrItem.Body);
                        }

                        // Para cada símbolo del Follow, lleno la tabla
                        foreach (var token in lrItem.Follow)
                            table.SetAction(state.Index, token, action);
                    }
                }
            }

            // Devolver la tabla
            return table;
        }

        /// <summary>
        /// Precalcula los First de todos los símbolos de la gramática.
        /// </summary>
        private void CalculateFirsts()
        {
            // Todas las reglas empiezan con un First vacío
            foreach (var rule in grammar.Rules)
                firsts[rule] = new HashSet<Token<T>>();

            // Todos los tokens empiezan con un First consigo mismo
            foreach (var token in grammar.Tokens)
                firsts[token] = new HashSet<Token<T>> {token};

            bool changed = true;

            // Mientras haya cambios
            while (changed)
            {
                changed = false;

                foreach (var production in grammar.Productions)
                {
                    Def<T> A = production.Definition;
                    HashSet<Token<T>> first = firsts[A];

                    // Proceso todas las reglas { A -> B1, ..., Bn }
                    foreach (var rule in production.List)
                    {
                        bool next = true;
                        int current = 0;
                        Def<T> Bi = rule[current];

                        // Para saber si necesito procesar el siguiente
                        while (next)
                        {
                            next = false;

                            // Agrego a First(Bi) en el First(A), excepto por epsilon
                            foreach (var token in firsts[Bi])
                            {
                                if (token != grammar.Epsilon && !first.Contains(token))
                                {
                                    first.Add(token);
                                    changed = true;
                                }
                            }

                            // Si el First(Bi) contiene a epsilon
                            if (firsts[Bi].Contains(grammar.Epsilon))
                            {
                                // Si Bi es el último elemento, agrego epsilon al First(A)
                                if (current == rule.Count - 1)
                                {
                                    first.Add(grammar.Epsilon);
                                    changed = true;
                                }
                                    // Paso a procesar el siguiente Bi
                                else
                                {
                                    changed = true;
                                    next = true;
                                    current++;
                                    Bi = rule[current];
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Intenta comprimir el conjunto de estados LR(1) en el 
        /// conjunto de estados LALR(1) correspondiente.
        /// </summary>
        /// <typeparam name="T">El tipo de la gramática.</typeparam>
        /// <param name="statesList">Conjunto de estados para comprimir.</param>
        /// <returns>Un nuevo conjunto de estados comprimidos si es posible,
        /// es decir, si la gramática es LALR, o el conjunto original en caso contrario.</returns>
        private IEnumerable<LrState<T>> Compress(IEnumerable<LrState<T>> statesList)
        {
            return statesList;

            //var compressedStates = new Dictionary<LrState<T>, LrState<T>>();
            //var statesIndex = new Dictionary<int, int>();

            //var statesCopy = states.ToArray();

            //foreach (var state in statesCopy)
            //{
            //    if (!compressedStates.ContainsKey(state))
            //    {
            //        compressedStates.Add(state, state);
            //        statesIndex[state.Index] = state.Index;
            //    }
            //    else
            //    {
            //        var state2 = compressedStates[state];

            //        foreach (var lrItem in state2)
            //        {

            //        }
            //    }
            //}
        }

        private void ManageStates(Def<T> start)
        {
            //cola de nuevos estados
            var newStates = new Queue<LrState<T>>();

            // Construir el item S' -> .S
            var item0 = new LrItem<T>(start, new ProductionRule<T> {Defs = {grammar.StartSymbol}},
                                      new HashSet<Token<T>> {grammar.Eof});

            // Creo el estado inicial
            var state0 = new LrState<T>(0, Clousure(new HashSet<LrItem<T>> {item0}).ToArray());

            //agrego sus gotos
            AddGotos(state0);

            // Lo pongo en la cola
            newStates.Enqueue(state0);

            // Lo agrego al diccionario
            states.Add(new LrKernel<T>(state0.ToArray()), state0);

            while (newStates.Count > 0)
            {
                LrState<T> state = newStates.Dequeue();
                //para cada goto del estado
                foreach (var gt in state.Gotos)
                {
                    LrState<T> newSt = ApplyGoto(gt);

                    if (newSt != null)
                    {
                        //si hay un nuevo estado
                        AddGotos(newSt);
                        newStates.Enqueue(newSt);
                    }
                }
            }
        }

        private void AddGotos(LrState<T> state)
        {
            var defs = new HashSet<Def<T>>();

            foreach (var lrItem in state)
            {
                Def<T> d = lrItem.NextSymbol;
                if (d != null && !defs.Contains(d))
                {
                    defs.Add(d);
                    state.Gotos.Add(new Goto<T>(state, d, null));
                }
            }
        }

        /// <summary>
        /// Clasula la clausura dado un conjunto de producciones.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        private HashSet<LrItem<T>> Clousure(IEnumerable<LrItem<T>> items)
        {
            // Convertir HashSet a Dictionary para poder indexar
            Dictionary<LrItem<T>, LrItem<T>> itemsDict = items.ToDictionary(x => x);

            // Si hay cambios 
            bool changed = true;

            // Mientras hay cambios
            while (changed)
            {
                changed = false;

                // Para las producciones donde el siguiente símbolo es un no terminal
                LrItem<T>[] nonTerminals = itemsDict.Keys.Where(x => x.NextSymbol != null && x.NextSymbol.IsNonTerminal).ToArray();

                foreach (var item in nonTerminals)
                {
                    List<ProductionRule<T>> list = item.NextSymbol.List.List;
                    foreach (var rule in list)
                    {
                        // Creo un lrItem con el Follow igual al First de la producción actual.
                        var lrItem = new LrItem<T>(item.NextSymbol, rule, First(item));
                        // Si el lrItem está en las producciones anteriores
                        if (itemsDict.ContainsKey(lrItem))
                        {
                            HashSet<Token<T>> temp = itemsDict[lrItem].Follow;
                            // Si el Follow no es el mismo
                            if (!temp.IsSupersetOf(lrItem.Follow))
                            {
                                // Actualizo el Follow del que está en el diccionario
                                temp.UnionWith(lrItem.Follow);
                                changed = true;
                            }
                        }
                        else
                        {
                            // Si no está, se agrega
                            itemsDict.Add(lrItem, lrItem);
                            changed = true;
                        }
                    }
                }
            }

            return new HashSet<LrItem<T>>(itemsDict.Values);
        }

        /// <summary>
        /// Encuentra el primer terminal detrás del símbolo siguiente.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="production"></param>
        private HashSet<Token<T>> First(LrItem<T> production)
        {
            var first = new HashSet<Token<T>>();

            bool next = true;
            int current = production.Pointer + 1;

            while (next)
            {
                next = false;

                // Si es { A -> .B | c } agrego c al First
                if (current == production.Count)
                {
                    foreach (var token in production.Follow)
                        first.Add(token);

                    return first;
                }

                // Agrego todos los tokens excepto epsilon
                foreach (var token in firsts[production.Body[current]])
                {
                    if (token != grammar.Epsilon && !first.Contains(token))
                        first.Add(token);
                }

                // Si está epsilon, proceso el siguiente elemento de la producción
                if (firsts[production.Body[current]].Contains(grammar.Epsilon))
                {
                    next = true;
                    current++;
                }
            }

            return first;
        }

        /// <summary>
        /// Aplica el goto y le actualiza el estado de su destino. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gt"></param>
        /// <returns>Retorna verdadero si el estado generado es nuevo.</returns>
        private LrState<T> ApplyGoto(Goto<T> gt)
        {
            var newState = new HashSet<LrItem<T>>();
            //producciones del origen cuyo siguiente símbolo es el del goto 
            foreach (var lrItem in gt.Origin.Where(item => item.NextSymbol == gt.Symbol))
            {
                //agregarlas al nuevo estado
                newState.Add(lrItem.Next());
            }
            //Calcular la clausura
            newState = Clousure(newState);

            LrItem<T>[] temp = newState.ToArray();
            var kernel = new LrKernel<T>(temp);
            //el estado ya existe
            if (states.ContainsKey(kernel))
            {
                //actualizo el goto
                gt.Destination = states[kernel];
                return null;
            }
            //es un nuevo  estado
            var resul = new LrState<T>(states.Count, temp);
            //lo agrego al conjunto de estados
            states.Add(kernel, resul);
            //actualizo el goto
            gt.Destination = resul;
            return resul;
        }
    }
}
