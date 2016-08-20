using System;

namespace OpenTracing.BasicTracer.OpenTracingContext
{
    internal static class GuidFactory
    {
        public static ulong Create()
        {
            return BitConverter.ToUInt64(Guid.NewGuid().ToByteArray(), 0);
        }
    }
}
