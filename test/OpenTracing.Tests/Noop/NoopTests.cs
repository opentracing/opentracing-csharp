using System.Collections.Generic;
using OpenTracing.Noop;
using OpenTracing.Propagation;
using Xunit;

namespace OpenTracing.Tests.Noop
{
    public class NoopTests
    {
        [Fact]
        public void Factory_returns_the_same_instance()
        {
            var tracer1 = NoopTracerFactory.Create();
            var tracer2 = NoopTracerFactory.Create();

            Assert.Same(tracer1, tracer2);
        }

        [Fact]
        public void Tracer_uses_NoopScopeManager()
        {
            var tracer = NoopTracerFactory.Create();

            Assert.IsType<NoopScopeManager>(tracer.ScopeManager);
        }

        [Fact]
        public void Tracer_initially_has_no_active_span()
        {
            var tracer = NoopTracerFactory.Create();

            Assert.Null(tracer.ActiveSpan);
        }

        [Fact]
        public void BuildSpan_returns_NoopSpanBuilder()
        {
            var tracer = NoopTracerFactory.Create();

            var spanBuilder = tracer.BuildSpan("noop");

            Assert.IsType<NoopSpanBuilder>(spanBuilder);
        }

        [Fact]
        public void BuildSpan_always_returns_same_Builder()
        {
            var tracer = NoopTracerFactory.Create();

            var builder1 = tracer.BuildSpan("noop1");
            var builder2 = tracer.BuildSpan("noop2");

            Assert.Same(builder1, builder2);
        }

        [Fact]
        public void StartActive_returns_NoopScope()
        {
            var tracer = NoopTracerFactory.Create();

            var scope = tracer.BuildSpan("noop")
                .StartActive(finishSpanOnDispose: false);

            Assert.IsType<NoopScopeManager.NoopScope>(scope);
        }

        [Fact]
        public void NoopScope_from_StartActive_has_NoopSpan()
        {
            var tracer = NoopTracerFactory.Create();

            var scope = tracer.BuildSpan("noop")
                .StartActive(finishSpanOnDispose: false);

            Assert.IsType<NoopSpan>(scope.Span);
        }

        [Fact]
        public void StartActive_does_NOT_set_Tracer_ActiveSpan()
        {
            var tracer = NoopTracerFactory.Create();

            var scope = tracer.BuildSpan("noop")
                .StartActive(finishSpanOnDispose: false);

            Assert.Null(tracer.ActiveSpan);
        }

        [Fact]
        public void StartActive_does_NOT_set_ScopeManager_Active()
        {
            var tracer = NoopTracerFactory.Create();

            var scope = tracer.BuildSpan("noop")
                .StartActive(finishSpanOnDispose: false);

            Assert.Null(tracer.ScopeManager.Active);
        }

        [Fact]
        public void Start_returns_NoopSpan()
        {
            var tracer = NoopTracerFactory.Create();

            var span = tracer.BuildSpan("noop").Start();

            Assert.IsType<NoopSpan>(span);
        }

        [Fact]
        public void Start_always_returns_same_Span()
        {
            var tracer = NoopTracerFactory.Create();

            var span1 = tracer.BuildSpan("noop1").Start();
            var span2 = tracer.BuildSpan("noop2").Start();

            Assert.Same(span1, span2);
        }

        [Fact]
        public void Start_does_NOT_set_Tracer_ActiveSpan()
        {
            var tracer = NoopTracerFactory.Create();

            var span = tracer.BuildSpan("noop").Start();

            Assert.Null(tracer.ActiveSpan);
        }

        [Fact]
        public void Start_does_NOT_set_ScopeManager_Active()
        {
            var tracer = NoopTracerFactory.Create();

            var span = tracer.BuildSpan("noop").Start();

            Assert.Null(tracer.ScopeManager.Active);
        }

        [Fact]
        public void ScopeManager_returns_NoopScope_on_Activate()
        {
            var tracer = NoopTracerFactory.Create();

            var span = tracer.BuildSpan("noop").Start();

            var scope = tracer.ScopeManager.Activate(span, finishSpanOnDispose: false);

            Assert.IsType<NoopScopeManager.NoopScope>(scope);
        }

        [Fact]
        public void ScopeManager_does_NOT_set_Active_on_Activate()
        {
            var tracer = NoopTracerFactory.Create();

            var span = tracer.BuildSpan("noop").Start();

            tracer.ScopeManager.Activate(span, finishSpanOnDispose: false);

            Assert.Null(tracer.ScopeManager.Active);
            Assert.Null(tracer.ActiveSpan);
        }

        [Fact]
        public void Span_has_NoopSpanContext()
        {
            var tracer = NoopTracerFactory.Create();

            var span = tracer.BuildSpan("noop").Start();

            Assert.IsType<NoopSpanContext>(span.Context);
        }

        [Fact]
        public void Extract_returns_NoopSpanContext()
        {
            var tracer = NoopTracerFactory.Create();

            var carrier = new Dictionary<string, string>();

            var spanContext = tracer.Extract(BuiltinFormats.TextMap, new TextMapExtractAdapter(carrier));

            Assert.IsType<NoopSpanContext>(spanContext);
        }

        [Fact]
        public void Inject_does_not_modify_carrier()
        {
            var tracer = NoopTracerFactory.Create();

            var span = tracer.BuildSpan("noop").Start();
            span.SetBaggageItem("key", "value");

            var carrier = new Dictionary<string, string>();

            tracer.Inject(span.Context, BuiltinFormats.TextMap, new TextMapInjectAdapter(carrier));

            Assert.Empty(carrier);
        }
    }
}
