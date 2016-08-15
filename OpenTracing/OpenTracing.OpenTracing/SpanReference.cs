namespace OpenTracing
{
    public class SpanReference
    {
        private SpanReference(SpanReferenceType type, ISpanContext referencedContext)
        {
            Type = type;
            ReferencedContext = referencedContext;
        }

        public SpanReferenceType Type { get; private set; }
        public ISpanContext ReferencedContext { get; private set; }

        public static SpanReference ChildOf(ISpanContext spanContext)
        {
            return new SpanReference(SpanReferenceType.ChildOfRef, spanContext);
        }

        public static SpanReference FollowsFrom(ISpanContext spanContext)
        {
            return new SpanReference(SpanReferenceType.FollowsFromRef, spanContext);
        }
    }
}
