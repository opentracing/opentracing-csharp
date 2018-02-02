namespace OpenTracing.Tag
{
    public sealed class BooleanTag : AbstractTag<bool>
    {
        public BooleanTag(string tagKey)
            : base(tagKey)
        {
        }

        protected override void Set(ISpan span, bool tagValue)
        {
            span.SetTag(Key, tagValue);
        }
    }
}