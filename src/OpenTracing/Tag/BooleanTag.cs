namespace OpenTracing.Tag
{
    sealed class BooleanTag : AbstractTag<bool>
    {
        public BooleanTag(string tagKey)
            : base(tagKey)
        {
        }

        protected override void Set<TSpan>(IBaseSpan<TSpan> span, bool tagValue)
        {
            span.SetTag(this.Key, tagValue);
        }
    }
}