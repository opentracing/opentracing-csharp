using System.IO;

namespace OpenTracing.Propagation
{
    /// <summary>
    /// <see cref="IBinary"/> is a built-in carrier for <see cref="ITracer.Inject{TCarrier}"/> and
    /// <see cref="ITracer.Extract{TCarrier}"/>. IBinary implementations allow for the reading and writing of arbitrary streams.
    /// </summary>
    /// <seealso cref="ITracer.Inject{TCarrier}"/>
    /// <seealso cref="ITracer.Extract{TCarrier}"/>
    public interface IBinary
    {
        /// <summary>
        /// Sets the backing MemoryStream of an <see cref="IBinary"/> store.
        /// </summary>
        /// <param name="stream">A memory-backed stream.</param>
        void Set(MemoryStream stream);

        /// <summary>
        /// Gets the backing MemoryStream of an <see cref="IBinary"/> store.
        /// </summary>
        /// <returns></returns>
        MemoryStream Get();
    }
}
