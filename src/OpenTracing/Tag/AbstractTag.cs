namespace OpenTracing.Tag
{
    public abstract class AbstractTag<T>
    {
        protected AbstractTag(string tagKey)
        {
            Key = tagKey;
        }

        public string Key { get; }

        protected abstract void Set<TSpan>(ISpan span, T tagValue)
            where TSpan : ISpan;
    }
}