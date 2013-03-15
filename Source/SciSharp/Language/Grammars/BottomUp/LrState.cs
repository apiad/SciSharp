using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace SciSharp.Language.Grammars.BottomUp
{
    public class LrState<T> : IEnumerable<LrItem<T>>
        where T : Node, new()
    {
        private readonly List<Goto<T>> gotos;
        private readonly int index;
        private readonly LrItem<T>[] items;

        public LrState(int index, params LrItem<T>[] items)
        {
            this.index = index;
            this.items = items;
            gotos = new List<Goto<T>>();
        }

        public int Index
        {
            get { return index; }
        }

        public List<Goto<T>> Gotos
        {
            get { return gotos; }
            //set { gotos = value; }
        }

        public LrItem<T> this[int index]
        {
            get { return items[index]; }
        }

        #region IEnumerable<LrItem<T>> Members

        public IEnumerator<LrItem<T>> GetEnumerator()
        {
            return ((IEnumerable<LrItem<T>>) items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Devuelve el estdo para el que se salta con este símbolo.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public LrState<T> Goto(Def<T> symbol)
        {
            foreach (var g in gotos)
            {
                if (g.Symbol == symbol)
                {
                    return g.Destination;
                }
            }
            return null;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendFormat("State: {0}\n", Index);

            foreach (var lrItem in items)
                sb.AppendLine(lrItem.ToString());

            foreach (var gt in Gotos)
                sb.AppendFormat("Goto({0}) = {1}\n", gt.Symbol, gt.Destination.Index);

            return sb.ToString();
        }
    }
}
