namespace OpenTracing.Tag
{
    abstract class AbstractTag<T>
    {
        public string Key { get; }

        protected AbstractTag(string tagKey)
        {
            this.Key = tagKey;
        }

        protected abstract void Set<TSpan>(IBaseSpan<TSpan> span, T tagValue)
            where TSpan : IBaseSpan<TSpan>;
    }
}