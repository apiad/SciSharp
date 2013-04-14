using System;
using System.Collections.Generic;
using System.Linq;

using SciSharp.Probabilities;


namespace SciSharp.Learning.Classification
{
    [Serializable]
    public class FeedForwardNetwork
    {
        private readonly Vector[] activations;
        private readonly int inputLayer = 0;
        private readonly int outputLayer;
        private readonly RandomEx random;
        private readonly int[] sizes;
        private readonly Matrix[] weights;

        public FeedForwardNetwork(params int[] layerSizes)
        {
            if (layerSizes == null)
                throw new ArgumentNullException("layerSizes");

            if (layerSizes.Length < 2)
                throw new ArgumentOutOfRangeException("layerSizes", "Must have length at least 2.");

            // Store the layer sizes
            sizes = new int[layerSizes.Length];
            outputLayer = sizes.Length - 1;

            for (int i = 0; i < layerSizes.Length; i++)
            {
                if (layerSizes[i] <= 0)
                    throw new ArgumentOutOfRangeException("layerSizes", "All values must be greater than zero.");

                sizes[i] = layerSizes[i];
            }

            // Store the activations
            activations = new Vector[sizes.Length];

            // Store the weights
            weights = new Matrix[sizes.Length - 1];

            // Weights going from layer l to l+1
            for (int l = 0; l < weights.Length; l++)
                weights[l] = Matrices.Random(sizes[l], sizes[l + 1]);

            random = new RandomEx();
        }

        public Matrix[] Weights
        {
            get { return weights; }
        }

        public Vector Feed(Vector input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (input.Dimension != sizes[0])
                throw new ArgumentOutOfRangeException("input", "Must have length {0}".Formatted(sizes[0]));

            // Set the activations of the input layer
            activations[0] = input;

            // Propagate foward all the activations
            for (int l = 0; l < weights.Length; l++)
            {
                // Calculate the lth activation
                activations[l + 1] = activations[l] * weights[l];

                // Apply the activation function
                for (int i = 0; i < activations[l + 1].Dimension; i++)
                    activations[l + 1][i] = Sigmoid(activations[l + 1][i]);
            }

            // Return the output values
            return activations[outputLayer];
        }

        public Matrix Feed(Matrix input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (input.Columns != sizes[0])
                throw new ArgumentOutOfRangeException("input", "Must have column length {0}".Formatted(sizes[0]));

            // Set the activations of the input layer
            Matrix values = input;

            // Propagate foward all the activations
            for (int l = 0; l < weights.Length; l++)
            {
                // Calculate the lth activation
                values = values * weights[l];

                // Apply the activation function
                for (int i = 0; i < values.Rows; i++)
                    for (int j = 0; j < values.Columns; j++)
                        values[i, j] = Sigmoid(values[i, j]);
            }

            // Return the output values
            return values;
        }

        private double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }

        public void Train(IEnumerable<TrainingExample> trainingExamples, int epochs, int batchSize = int.MaxValue, double minError = 0)
        {
            if (trainingExamples == null)
                throw new ArgumentNullException("trainingExamples");

            // Initialize temporal data structures
            Matrix[] momentum = BuildMatrices();
            Matrix[] learning = BuildMatrices(1d);

            double damping = 0.9;

            Guid guid = Guid.NewGuid();

            // Copy examples to local array
            TrainingExample[] examples = trainingExamples.ToArray();

            int time = Environment.TickCount;
            double step = 0;

            // for each epoch
            for (int epoch = 0; epoch < epochs; epoch++)
            {
                Logger.Log("Epoch {0}", epoch);
                int batchCount = 0;
                double error = 0;

                // Shuffle inputs
                random.Shuffle(examples);

                // Take in mini-batches
                foreach (var batch in examples.Batches(batchSize))
                {
                    // Initialize the deltas
                    var updates = new Matrix[weights.Length];

                    for (int l = 0; l < updates.Length; l++)
                        updates[l] = new Matrix(weights[l].Rows, weights[l].Columns);

                    var deltas = new Vector[sizes.Length];

                    int samples = 0;

                    // Take every example
                    foreach (TrainingExample example in batch)
                    {
                        // Checkings
                        if (example == null)
                            throw new ArgumentException("One of the examples is null.");

                        if (example.InputSize != sizes[inputLayer])
                            throw new ArgumentOutOfRangeException("trainingExamples", "One example doesn't match the input dimension.");

                        if (example.OutputSize != sizes[outputLayer])
                            throw new ArgumentOutOfRangeException("trainingExamples", "One example doesn't match the ouput dimension.");

                        // Update the sample count
                        samples++;

                        // Calculate the error in the output layer
                        Vector errors = example.Output - Feed(example.Input);

                        // Accumulate the deltas for the output layer
                        deltas[outputLayer] = (~activations[outputLayer]) ._* activations[outputLayer] ._* errors;

                        // Backpropagate the deltas
                        for (int l = outputLayer - 1; l > 0; l--)
                            deltas[l] = (~activations[l]) ._* activations[l] ._* (weights[l] * deltas[l + 1]);

                        // Calculate weight updates for each layer
                        for (int l = 0; l < updates.Length; l++)
                            updates[l] += (activations[l].AsColumn() * deltas[l + 1].AsRow());

                        // Update the error
                        error += errors.LengthSqr;
                    }

                    // Update momentum
                    for (int l = 0; l < momentum.Length; l++)
                        momentum[l] = damping * momentum[l] + learning[l] ._* (updates[l] / samples);

                    // Update learning rates
                    for (int l = 0; l < learning.Length; l++)
                    {
                        // TODO: Make this matrix-based
                        for (int i = 0; i < learning[l].Rows; i++)
                            for (int j = 0; j < learning[l].Columns; j++)
                            {
                                learning[l][i, j] = updates[l][i, j] * momentum[l][i, j] >= 0
                                                        ? Math.Min(1.00, learning[l][i, j] + 0.05)
                                                        : Math.Max(0.01, learning[l][i, j] * 0.95);
                            }
                    }

                    // Actually update the weights
                    for (int l = 0; l < weights.Length; l++)
                        weights[l] += momentum[l];

                    step = (epoch * examples.Length + batchCount * batchSize + samples) * 1d / (epochs * examples.Length);

                    Logger.Log(" - Batch completed -- Samples: {0} ({1:0.00} %)", samples, step * 100);
                }

                // Normalize the error
                error = Math.Sqrt(error / examples.Length);

                int elapsed = Environment.TickCount - time;
                double remaining = (1 - step) * elapsed / step;

                Logger.Log(" Error:             {0}", error);
                Logger.Log(" Elapsed time:      {0}", TimeSpan.FromMilliseconds(elapsed));
                Logger.Log(" ET Remaining time: {0}", TimeSpan.FromMilliseconds(remaining));

                //this.Serialize("FFN [{3}] - Epoch {0} ({1} %) - Error = {2:0.0000}".Formatted(epoch, step*100, error, guid), Serializer.Format.Binary);

                if (error < minError)
                    break;
            }
        }

        private Matrix[] BuildMatrices(double d)
        {
            Matrix[] array = BuildMatrices();

            foreach (Matrix layer in array)
                for (int i = 0; i < layer.Rows; i++)
                    for (int j = 0; j < layer.Columns; j++)
                        layer[i, j] = d;

            return array;
        }

        private Matrix[] BuildMatrices()
        {
            var array = new Matrix[weights.Length];

            for (int l = 0; l < weights.Length; l++)
                array[l] = new Matrix(weights[l].Rows, weights[l].Columns);

            return array;
        }

        public Vector Test(IEnumerable<TrainingExample> examples)
        {
            var errors = new Vector(sizes[outputLayer]);

            foreach (TrainingExample example in examples)
                errors += example.Output - Feed(example.Input);

            return errors;
        }
    }
}
