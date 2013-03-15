using System;


namespace SciSharp.Language.Grammars
{
    public class ParameterList<T>
    {
        private readonly T[] parameters;

        public ParameterList(params T[] parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            this.parameters = parameters;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= parameters.Length)
                    throw new ArgumentOutOfRangeException("index");

                return parameters[index];
            }
        }
    }
}
