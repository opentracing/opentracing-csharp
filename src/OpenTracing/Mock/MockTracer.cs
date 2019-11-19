using System.Collections.Generic;
using OpenTracing.Propagation;
using OpenTracing.Util;

namespace OpenTracing.Mock
{
    /// <summary>
    /// <see cref="MockTracer"/> makes it easy to test the semantics of OpenTracing instrumentation.
    /// <para/>
    /// By using a <see cref="MockTracer"/> as an <see cref="ITracer"/> implementation for unittests, a developer can assert that span
    /// properties and relationships with other spans are defined as expected by instrumentation code.
    /// <para/>
    /// The MockTracerTests class has simple usage examples.
    /// </summary>
    public class MockTracer : ITracer
    {
        private readonly object _lock = new object();

        private readonly List<MockSpan> _finishedSpans = new List<MockSpan>();
        private readonly IPropagator _propagator;
        private readonly IScopeManager _scopeManager;

        public IScopeManager ScopeManager => _scopeManager;

        public ISpan ActiveSpan => _scopeManager?.Active?.Span;

        public MockTracer()
            : this(new AsyncLocalScopeManager(), Propagators.TextMap)
        {
        }

        public MockTracer(IScopeManager scopeManager)
            : this(scopeManager, Propagators.TextMap)
        {
        }

        public MockTracer(IScopeManager scopeManager, IPropagator propagator)
        {
            _scopeManager = scopeManager;
            _propagator = propagator;
        }

        /// <summary>
        /// Create a new <see cref="MockTracer"/> that passes through any calls
        /// to <see cref="ITracer.Inject{TCarrier}"/> and/or <see cref="ITracer.Extract{TCarrier}"/>.
        /// </summary>
        public MockTracer(IPropagator propagator)
            : this(new AsyncLocalScopeManager(), propagator)
        {
        }

        /// <summary>
        /// Clear the <see cref="FinishedSpans"/> queue.
        /// <para/>
        /// Note that this does *not* have any effect on spans created by MockTracer that have not Finish()ed yet; those
        /// will still be enqueued in <see cref="FinishedSpans"/> when they Finish().
        /// </summary>
        public void Reset()
        {
            lock (_lock)
            {
                _finishedSpans.Clear();
            }
        }

        /// <summary>
        /// Returns a copy of all Finish()ed MockSpans started by this MockTracer (since construction or the last call to
        /// <see cref="MockTracer.Reset"/>).
        /// </summary>
        /// <seealso cref="MockTracer.Reset"/>
        public List<MockSpan> FinishedSpans()
        {
            lock (_lock)
            {
                return new List<MockSpan>(_finishedSpans);
            }
        }

        /// <summary>
        /// Noop method called on <see cref="ISpan.Finish()"/>.
        /// </summary>
        protected virtual void OnSpanFinished(MockSpan mockSpan)
        {
        }

        public ISpanBuilder BuildSpan(string operationName)
        {
            return new MockSpanBuilder(this, operationName);
        }

        public void Inject<TCarrier>(ISpanContext spanContext, IFormat<TCarrier> format, TCarrier carrier)
        {
            _propagator.Inject((MockSpanContext)spanContext, format, carrier);
        }

        public ISpanContext Extract<TCarrier>(IFormat<TCarrier> format, TCarrier carrier)
        {
            return _propagator.Extract(format, carrier);
        }

        internal void AppendFinishedSpan(MockSpan mockSpan)
        {
            lock (_lock)
            {
                _finishedSpans.Add(mockSpan);
                OnSpanFinished(mockSpan);
            }
        }
    }
}
