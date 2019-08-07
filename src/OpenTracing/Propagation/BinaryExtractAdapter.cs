using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OpenTracing.Propagation
{
    /// <summary>
    /// A <see cref="IBinary"/> carrier for use with <see cref="ITracer.Extract{TCarrier}"/> only. It cannot be mutated, only read.
    /// </summary>
    /// <seealso cref="ITracer.Extract{TCarrier}"/>
    public class BinaryExtractAdapter : IBinary
    {
        private readonly MemoryStream _stream;

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
            return _stream;
        }
    }
}
