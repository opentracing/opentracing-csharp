namespace OpenTracing.Tag
{
    public abstract class AbstractTag<TTagValue>
    {
        protected AbstractTag(string tagKey)
        {
            Key = tagKey;
        }

        public string Key { get; }

        protected abstract void Set<TSpan>(ISpan span, TTagValue tagValue)
            where TSpan : ISpan;
    }
}