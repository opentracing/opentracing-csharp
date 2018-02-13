using System.Reflection;
using OpenTracing.Noop;
using OpenTracing.Util;

namespace OpenTracing.Tests.Util
{
    public static class GlobalTracerTestUtil
    {
        private static FieldInfo s_tracerField = typeof(GlobalTracer).GetField("_tracer", BindingFlags.NonPublic | BindingFlags.Instance);

        /// <summary>
        /// Resets the <see cref="GlobalTracer"/> to its initial, unregistered state.
        /// </summary>
        public static void ResetGlobalTracer()
        {
            SetGlobalTracerUnconditionally(NoopTracerFactory.Create());
        }

        /// <summary>
        /// Unconditionally sets the <see cref="GlobalTracer"/> to the specified <see cref="ITracer"/> instance.
        /// </summary>
        /// <param name="tracer">The tracer to become the GlobalTracer's delegate.</param>
        public static void SetGlobalTracerUnconditionally(ITracer tracer)
        {
            GlobalTracer globalTracer = (GlobalTracer)GlobalTracer.Instance;
            s_tracerField.SetValue(globalTracer, tracer);
        }
    }
}
