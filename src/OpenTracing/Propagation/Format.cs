using System;

namespace OpenTracing.Propagation
{
    /// <summary>
    /// Format instances control the behavior of <see cref="ITracer.Inject" /> and <see cref="ITracer.Extract" /> 
    /// (and also constrain the type of the carrier parameter to same).
    /// </summary>
    public struct Format<TCarrier>
    {
        /// <summary>
        /// The unique name for this format.
        /// </summary>
        public string Name { get; }

        public Format(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
        }
    }
}