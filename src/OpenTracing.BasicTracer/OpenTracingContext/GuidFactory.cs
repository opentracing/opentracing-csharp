namespace OpenTracing.BasicTracer.OpenTracingContext
{
    internal static class GuidFactory
    {
        public static ulong Create()
        {
            // TODO !!!! BitConverter does not exist in .NET Core
            //return BitConverter.ToUInt64(Guid.NewGuid().ToByteArray(), 0);
            return 1;
        }
    }
}
