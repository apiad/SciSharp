using System;

using SciSharp.Probabilities;


namespace SciSharp
{
    public static class Engine
    {
        /// <summary>
        /// Una instancia singleton de <see cref="System.Random"/>
        /// para ser usada por el método <see cref="Random(double,double,int)"/>.
        /// </summary>
        public static readonly RandomEx R = new RandomEx();

        /// <summary>
        /// Almacena el valor epsilon.
        /// </summary>
        private static double epsilon = double.Epsilon;

        /// <summary>
        /// Almacena el valor Epsilon, considerado como el valor 
        /// más pequeño representable. Es decir, si |x-y| es menor
        /// que Epsilon, entonces x=y. Si este valor es muy pequeño (o cero)
        /// entonces algunas iteraciones pueden no parar.
        /// </summary>
        public static double Epsilon
        {
            get { return epsilon; }
            set
            {
                epsilon = value;

                if (epsilon < 0)
                    throw new ArgumentException("The value must be greater or equal to zero.", "value");
            }
        }

        /// <summary>
        /// Determina si las operaciones aritméticas se aplican
        /// sobre la instancia o devuelven una nueva instancia. 
        /// En la descripción de cada operación se indica si
        /// su comportamiento depede de este valor.
        /// </summary>
        public static bool OperateOnSelf { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if any arithmetic
        /// operation should throw an <see cref="ArithmeticException"/>
        /// when applied on a <see cref="double.NaN"/> value.
        /// If true, then the first operation on a <see cref="double.NaN"/> will
        /// throw, otherwise, the <see cref="double.NaN"/> will be carried silently
        /// across the operation. This behaviour only works on Debug mode. On
        /// Release mode the behaviour is as if this value were false.
        /// </summary>
        public static bool ThrowOnNaN { get; set; }

        /// <summary>
        /// Determina si se imprime información de debugging.
        /// </summary>
        public static bool Debugging { get; set; }

        /// <summary>
        /// Determina si se optimizarán las operaciones aritméticas
        /// en paralelo siempre que sea posible. Solo active esta
        /// opción si los datos tienen grandes dimensiones.
        /// </summary>
        public static bool UseParallelOptimizations { get; set; }

        public static bool Equals(double d1, double d2)
        {
            return (d2 - d1) > -Epsilon && (d1 - d2) < Epsilon;
        }

        public static int Sign(double d)
        {
            if (d <= -Epsilon)
                return -1;

            if (d >= Epsilon)
                return 1;

            return 0;
        }

        public static bool IsZero(double d)
        {
            return d > -Epsilon && d < Epsilon;
        }

        public static int Meassure(Action action)
        {
            int time = Environment.TickCount;
            action();
            return Environment.TickCount - time;
        }

        #region Nested type: EngineContext

        private class EngineContext : Context
        {
            private readonly bool debugging;
            private readonly bool operateOnSelf;
            private readonly double previousEpsilon;
            private readonly bool throwOnNaN;
            private readonly bool useParallelOptimizations;

            public EngineContext(double previousEpsilon, bool operateOnSelf, bool throwOnNaN, bool debugging, bool useParallelOptimizations)
            {
                this.previousEpsilon = previousEpsilon;
                this.operateOnSelf = operateOnSelf;
                this.throwOnNaN = throwOnNaN;
                this.debugging = debugging;
                this.useParallelOptimizations = useParallelOptimizations;
            }

            protected override void SafeEnd()
            {
                epsilon = previousEpsilon;
                OperateOnSelf = operateOnSelf;
                ThrowOnNaN = throwOnNaN;
                Debugging = debugging;
                UseParallelOptimizations = useParallelOptimizations;
            }
        }

        #endregion
    }
}
