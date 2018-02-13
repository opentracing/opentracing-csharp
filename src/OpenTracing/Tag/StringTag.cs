namespace OpenTracing.Tag
{
    public sealed class StringTag : AbstractTag<string>
    {
        public StringTag(string tagKey)
            : base(tagKey)
        {
        }

        public override void Set(ISpan span, string tagValue)
        {
            span.SetTag(Key, tagValue);
        }
    }
}
