namespace OpenTracing.Tag
{
    sealed class IntOrStringTag : IntTag
    {
        public IntOrStringTag(string tagKey)
            : base(tagKey)
        {
        }

        public void Set<TSpan>(IBaseSpan<TSpan> span, string tagValue)
            where TSpan : IBaseSpan<TSpan>
        {
            span.SetTag(this.Key, tagValue);
        }
    }
}