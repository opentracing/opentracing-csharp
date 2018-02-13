using System;
using OpenTracing.Noop;
using OpenTracing.Propagation;

namespace OpenTracing.Util
{
    /// <summary>
    /// Global tracer that forwards all methods to another tracer that can be
    /// configured by calling <see cref="Register(ITracer)"/>.
    /// <para/>
    /// The <see cref="Register(ITracer)"/> method should only be called once
    /// during the application initialization phase.<br/>
    /// If the <see cref="Register(ITracer)"/> method is never called,
    /// the default <see cref="NoopTracer"/> is used.
    /// <para/>
    /// Where possible, use some form of dependency injection (of which there are
    /// many) to access the <see cref="ITracer"/> instance. For vanilla application code, this is
    /// often reasonable and cleaner for all of the usual DI reasons.
    /// <para/>
    /// That said, instrumentation for packages that are themselves statically
    /// configured (e.g., JDBC drivers) may be unable to make use of said DI
    /// mechanisms for <see cref="ITracer"/> access, and as such they should fall back on
    /// <see cref="GlobalTracer"/>. By and large, OpenTracing instrumentation should
    /// always allow the programmer to specify a <see cref="ITracer"/> instance to use for
    /// instrumentation, though the <see cref="GlobalTracer"/> is a reasonable fallback or
    /// default value.
    /// </summary>
    public sealed class GlobalTracer : ITracer
    {
        private static readonly object s_lock = new object();

        /// <summary>
        /// Singleton instance.
        /// <para/>
        /// Since we cannot prevent people using <see cref="Instance"/> as a constant,
        /// this guarantees that references obtained before, during or after initialization
        /// all behave as if obtained <em>after</em> initialization once properly initialized.<br/>
        /// As a minor additional benefit it makes it harder to circumvent the <see cref="ITracer"/> API.
        /// </summary>
        private static readonly GlobalTracer s_instance = new GlobalTracer();

        /// <summary>
        /// Returns the constant <see cref="GlobalTracer"/>.
        /// <para/>
        /// All methods are forwarded to the currently configured tracer.<br/>
        /// Until a tracer is explicitly configured via <see cref="Register(ITracer)"/>,
        /// the <see cref="NoopTracer"/> is used.
        /// </summary>
        /// <returns>The global tracer constant.</returns>
        /// <seealso cref="Register(ITracer)"/>
        public static ITracer Instance => s_instance;

        /// <summary>
        /// Identify whether a <see cref="ITracer"/> has previously been registered.
        /// <para/>
        /// This check is useful in scenarios where more than one component may be responsible
        /// for registering a tracer.
        /// </summary>
        /// <returns>Whether a tracer has been registered.</returns>
        public static bool IsRegistered()
        {
            return !(s_instance._tracer is NoopTracer);
        }

        /// <summary>
        /// Register a <see cref="ITracer"/> to back the behaviour of the global tracer (<see cref="Instance"/>).
        /// <para/>
        /// Registration is a one-time operation, attempting to call it more often will result in a runtime exception.
        /// <para/>
        /// Every application intending to use the global tracer is responsible for registering it once
        /// during its initialization.
        /// </summary>
        /// <param name="tracer">Tracer to use as global tracer.</param>
        public static void Register(ITracer tracer)
        {
            if (tracer == null)
                throw new ArgumentNullException(nameof(tracer), "Cannot register GlobalTracer <null>.");

            if (tracer is GlobalTracer)
                throw new ArgumentException("Attempted to register the GlobalTracer as delegate of itself.", nameof(tracer));

            lock (s_lock)
            {
                if (tracer == s_instance._tracer)
                    return;

                if (IsRegistered())
                    throw new InvalidOperationException("There is already a current global Tracer registered.");

                s_instance._tracer = tracer;
            }
        }

        private ITracer _tracer = NoopTracerFactory.Create();

        /// <inheritdoc/>
        public IScopeManager ScopeManager => _tracer.ScopeManager;

        /// <inheritdoc/>
        public ISpan ActiveSpan => _tracer.ActiveSpan;

        private GlobalTracer()
        {
        }

        /// <inheritdoc/>
        public ISpanBuilder BuildSpan(string operationName)
        {
            return _tracer.BuildSpan(operationName);
        }

        /// <inheritdoc/>
        public ISpanContext Extract<TCarrier>(IFormat<TCarrier> format, TCarrier carrier)
        {
            return _tracer.Extract(format, carrier);
        }

        /// <inheritdoc/>
        public void Inject<TCarrier>(ISpanContext spanContext, IFormat<TCarrier> format, TCarrier carrier)
        {
            _tracer.Inject(spanContext, format, carrier);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return nameof(GlobalTracer) + "{" + _tracer + "}";
        }
    }
}
