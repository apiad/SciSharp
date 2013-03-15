namespace SciSharp.Learning.Classification
{
    public struct FeatureCount<TFeature>
    {
        private readonly int count;
        private readonly TFeature feature;

        public FeatureCount(TFeature feature, int count)
        {
            this.feature = feature;
            this.count = count;
        }

        public TFeature Feature
        {
            get { return feature; }
        }

        public int Count
        {
            get { return count; }
        }
    }
}
