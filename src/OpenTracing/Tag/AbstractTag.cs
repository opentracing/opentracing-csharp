namespace OpenTracing.Tag
{
    public abstract class AbstractTag<TTagValue>
    {
        protected AbstractTag(string tagKey)
        {
            Key = tagKey;
        }

        public string Key { get; }

        public abstract void Set(ISpan span, TTagValue tagValue);
    }
}
