using System;

namespace OpenTracing.Propagation
{
    /// <summary>
    /// A <see cref="ITextMapCarrier"/> which allows a byte array to be used as a carrier object.
    /// </summary>
    public class BinaryCarrier : IInjectCarrier, IExtractCarrier
    {
        // TODO is using byte[] a good solution?

        public byte[] Payload { get; }

        public BinaryCarrier(byte[] payload)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            Payload = payload;
        }
    }
}