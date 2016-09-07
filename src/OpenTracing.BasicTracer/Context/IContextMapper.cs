namespace OpenTracing.BasicTracer.Context
{
    public interface IContextMapper<TFormat>
    {
        SpanContext MapTo(TFormat data);
        TFormat MapFrom(SpanContext spanContext);
    }
}
