namespace OpenTracing.Noop
{
    public static class NoopTracerFactory
    {
        /// <summary>
        /// Returns the singleton <see cref="NoopTracer"/> instance.
        /// </summary>
        public static ITracer Create()
        {
            return NoopTracer.Instance;
        }
    }
}
