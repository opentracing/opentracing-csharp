namespace OpenTracing.Tag
{
    public sealed class IntOrStringTag : IntTag
    {
        public IntOrStringTag(string tagKey)
            : base(tagKey)
        {
        }

        public void Set(ISpan span, string tagValue)
        {
            span.SetTag(Key, tagValue);
        }
    }
}
