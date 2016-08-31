using System;

namespace OpenTracing.Propagation
{
    /// <summary>
    /// Format instances control the behavior of <see cref="ITracer.Inject" /> and <see cref="ITracer.Extract" /> 
    /// (and also constrain the type of the carrier parameter to same).
    /// </summary>
    public class Format<TCarrier> : IEquatable<Format<TCarrier>>
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
    
        /// <summary>
        /// Two format instances are equal when they have the same <see cref="Name" />.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals(obj as Format<TCarrier>);
        }

        /// <summary>
        /// Two format instances are equal when they have the same <see cref="Name" />.
        /// </summary>
        public bool Equals(Format<TCarrier> other)
        {
            if (other == null)
            {
                return false;
            }

            return string.Equals(Name, other.Name);
        }

        /// <summary>
        /// Two format instances are equal when they have the same <see cref="Name" />.
        /// </summary>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}