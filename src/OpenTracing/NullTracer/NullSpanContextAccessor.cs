namespace OpenTracing.NullTracer
{
    public class NullSpanContextAccessor : ISpanContextAccessor
    {
        public static readonly NullSpanContextAccessor Instance = new NullSpanContextAccessor();

        private NullSpanContextAccessor()
        {
        }

        public ISpanContext CurrentSpanContext
        {
            get { return NullSpanContext.Instance; }
            set { /*no-op*/ }
        }
    }
}