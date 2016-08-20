using System;

namespace OpenTracing.Propagation
{
    public class BinaryCarrier : IInjectCarrier, IExtractCarrier
    {
        public byte[] Data { get; }

        public BinaryCarrier(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            Data = data;
        }
    }
}