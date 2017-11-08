namespace OpenTracing.Tag
{
    sealed class StringTag : AbstractTag<string>
    {
        public StringTag(string tagKey)
            : base(tagKey)
        {
        }

        protected override void Set<TSpan>(IBaseSpan<TSpan> span, string tagValue)
        {
            span.SetTag(this.Key, tagValue);
        }

        public void Set<TSpan>(IBaseSpan<TSpan> span, StringTag tag)
            where TSpan : IBaseSpan<TSpan>
        {
            // TODO: That doesn't look right?
            span.SetTag(this.Key, tag.Key);
        }
    }
}