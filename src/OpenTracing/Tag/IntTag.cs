namespace OpenTracing.Tag
{
    class IntTag : AbstractTag<int>
    {
        public IntTag(string tagKey)
            : base(tagKey)
        {
        }

        protected override void Set<TSpan>(IBaseSpan<TSpan> span, int tagValue)
        {
            span.SetTag(this.Key, tagValue);
        }
    }
}