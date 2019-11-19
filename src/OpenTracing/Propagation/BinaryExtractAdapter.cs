using System;
using System.IO;

namespace OpenTracing.Propagation
{
    /// <summary>
    /// A <see cref="IBinary"/> carrier for use with <see cref="ITracer.Extract{TCarrier}"/> only. It cannot be mutated, only read.
    /// </summary>
    /// <seealso cref="ITracer.Extract{TCarrier}"/>
    public class BinaryExtractAdapter : IBinary
    {
        private MemoryStream _stream;

        public BinaryExtractAdapter(MemoryStream stream)
        {
            _stream = stream;
        }
        
        /// <inheritdoc />
        public void Set(MemoryStream stream)
        {
            throw new NotSupportedException($"{nameof(BinaryExtractAdapter)} should only be used with {nameof(ITracer)}.{nameof(ITracer.Extract)}");
        }

        /// <inheritdoc />
        public MemoryStream Get()
        {
            _stream.Position = 0;
            return _stream;
        }
    }
}
