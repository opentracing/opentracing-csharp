namespace OpenTracing.Tag
{
    public sealed class StringTag : AbstractTag<string>
    {
        public StringTag(string tagKey)
            : base(tagKey)
        {
        }

        protected override void Set<TSpan>(ISpan span, string tagValue)
        {
            span.SetTag(Key, tagValue);
        }
    }
}