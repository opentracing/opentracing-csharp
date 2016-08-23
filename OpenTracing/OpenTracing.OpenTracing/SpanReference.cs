namespace OpenTracing
{
    /// <summary>
    /// SpanReference is a StartSpanOption that pairs a SpanReferenceType and a
    /// referenced SpanContext. See the SpanReferenceType documentation for
    /// supported relationships.  If SpanReference is created with
    /// ReferencedContext==nil, it has no effect. Thus it allows for a more concise
    /// syntax for starting spans:
    ///
    ///     sc, _ := tracer.Extract(someFormat, someCarrier)
    ///     span := tracer.StartSpan("operation", opentracing.ChildOf(sc))
    ///
    /// The `ChildOf(sc)` option above will not panic if sc == nil, it will just
    /// not add the parent span reference to the options.
    /// </summary>
    public class SpanReference
    {
        private SpanReference(SpanReferenceType type, ISpanContext referencedContext)
        {
            Type = type;
            ReferencedContext = referencedContext;
        }

        public SpanReferenceType Type { get; private set; }
        public ISpanContext ReferencedContext { get; private set; }

        /// <summary>
        /// Creates a Span Reference with a ChildOfRef to parent span.
        /// </summary>
        /// <param name="spanContext">Parent span context</param>
        public static SpanReference ChildOf(ISpanContext spanContext)
        {
            return new SpanReference(SpanReferenceType.ChildOfRef, spanContext);
        }

        /// <summary>
        /// Creates a Span Reference with a followsFromRef to parent span.
        /// </summary>
        /// <param name="spanContext">Parent span context</param>
        public static SpanReference FollowsFrom(ISpanContext spanContext)
        {
            return new SpanReference(SpanReferenceType.FollowsFromRef, spanContext);
        }
    }
}
