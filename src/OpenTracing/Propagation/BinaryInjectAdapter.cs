using System;
using System.IO;

namespace OpenTracing.Propagation
{
    /// <summary>
    /// A <see cref="IBinary"/> carrier for use with <see cref="ITracer.Inject{TCarrier}"/> only. It cannot be read, only set.
    /// </summary>
    /// <seealso cref="ITracer.Extract{TCarrier}"/>
    public class BinaryInjectAdapter : IBinary
    {
        private MemoryStream _stream;

        public BinaryInjectAdapter(MemoryStream stream)
        {
            _stream = stream;
        }
        
        /// <inheritdoc />
        public void Set(MemoryStream stream)
        {
            stream.Position = 0;
            stream.CopyTo(_stream);
        }
        
        /// <inheritdoc />
        public MemoryStream Get()
        {
            throw new NotSupportedException($"{nameof(BinaryInjectAdapter)} should only be used with {nameof(ITracer)}.{nameof(ITracer.Inject)}");
        }
        
    }
}
