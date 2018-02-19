namespace OpenTracing.Tag
{
    public class IntTag : AbstractTag<int>
    {
        public IntTag(string tagKey)
            : base(tagKey)
        {
        }

        public override void Set(ISpan span, int tagValue)
        {
            span.SetTag(Key, tagValue);
        }
    }
}
