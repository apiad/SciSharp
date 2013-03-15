using System;


namespace SciSharp.Learning.Classification
{
    public class TrainingExample
    {
        private readonly Vector input;
        private readonly Vector output;

        public TrainingExample(Vector input, Vector output)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (output == null)
                throw new ArgumentNullException("output");

            this.input = input;
            this.output = output;
        }

        public Vector Input
        {
            get { return input; }
        }

        public Vector Output
        {
            get { return output; }
        }

        public int InputSize
        {
            get { return input.Dimension; }
        }

        public int OutputSize
        {
            get { return output.Dimension; }
        }
    }
}
