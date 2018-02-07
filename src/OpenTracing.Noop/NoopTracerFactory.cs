namespace OpenTracing.Noop
{
    public static class NoopTracerFactory
    {
        public static NoopTracer Create()
        {
            return NoopTracer.Instance;
        }
    }
}