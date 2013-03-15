using System;
using System.Collections.Generic;

using SciSharp.Collections;


namespace SciSharp.Learning.Classification
{
    public class TrainingResult<TClass>
    {
        private readonly IDictionary<Tuple<TClass, TClass>, int> crossClassification;
        private int correct;
        private int totalItems;

        public TrainingResult()
        {
            crossClassification = new DefaultDictionary<Tuple<TClass, TClass>, int>();
        }

        public int TotalItems
        {
            get { return totalItems; }
        }

        public int Correct
        {
            get { return correct; }
        }

        public int Wrong
        {
            get { return totalItems - correct; }
        }

        public double Effectiveness
        {
            get { return Correct*1d/TotalItems; }
        }

        public void Add(TClass realClass, TClass classifiedClass)
        {
            totalItems++;

            if (realClass.Equals(classifiedClass))
                correct++;

            crossClassification[new Tuple<TClass, TClass>(realClass, classifiedClass)]++;
        }

        public int CrossClassification(TClass realClass, TClass classifiedClass)
        {
            return crossClassification[new Tuple<TClass, TClass>(realClass, classifiedClass)];
        }
    }
}
