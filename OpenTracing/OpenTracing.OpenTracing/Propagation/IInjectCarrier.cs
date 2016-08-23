namespace OpenTracing.Propagation
{
    public interface IInjectCarrier<TFormat>
    {
        /// <summary>
        /// MapFrom takes the SpanContext instance and injects it for
        /// propagation within `carrier`. The actual format of context depends on
        /// the type of TFormat.
        /// </summary>
        void MapFrom(TFormat context);
    }
}
