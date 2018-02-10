namespace OpenTracing.Noop
{
    public static class NoopTracerFactory
    {
        public static ITracer Create()
        {
            return NoopTracer.Instance;
        }
    }
}
