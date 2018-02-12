using System;

namespace OpenTracing.Propagation
{
    /// <summary>
    /// This is a marker interface to allow <see cref="Format{TCarrier}"/> instances being used without their type constraint.
    /// </summary>
    /// <seealso cref="ITracer.Inject{TCarrier}"/>
    /// <seealso cref="ITracer.Extract{TCarrier}"/>
    public interface IFormat
    {
        /// <summary>
        /// The unique name for this format.
        /// </summary>
        string Name { get; }
    }

    /// <summary>
    /// Format instances control the behavior of <see cref="ITracer.Inject{TCarrier}"/> and
    /// <see cref="ITracer.Extract{TCarrier}"/> (and also constrain the type of the carrier parameter to same). Most
    /// OpenTracing users will only reference the <see cref="BuiltinFormats"/> constants. For example:
    /// <code>
    /// Tracer tracer = ...
    /// IFormat{ITextMap} httpCarrier = new AnHttpHeaderCarrier(httpRequest);
    /// ISpanContext spanCtx = tracer.Extract(BuiltinFormats.HttpHeaders, httpHeaderRequest);
    /// </code>
    /// </summary>
    /// <seealso cref="ITracer.Inject{TCarrier}"/>
    /// <seealso cref="ITracer.Extract{TCarrier}"/>
    public struct Format<TCarrier> : IFormat
    {
        /// <inheritdoc/>
        public string Name { get; }

        public Format(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <summary>Short name for the formats as they tend to show up in exception messages</summary>
        public override string ToString()
        {
            return $"Format.{Name}";
        }
    }
}