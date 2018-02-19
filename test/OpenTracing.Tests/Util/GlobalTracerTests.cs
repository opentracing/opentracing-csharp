using System;
using NSubstitute;
using OpenTracing.Noop;
using OpenTracing.Propagation;
using OpenTracing.Util;
using Xunit;

namespace OpenTracing.Tests.Util
{
    public class GlobalTracerTests : IDisposable
    {
        public GlobalTracerTests()
        {
            GlobalTracerTestUtil.ResetGlobalTracer();
        }

        public void Dispose()
        {
            GlobalTracerTestUtil.ResetGlobalTracer();
        }

        [Fact]
        public void Instance_returns_singleton_reference()
        {
            ITracer tracer1 = GlobalTracer.Instance;
            ITracer tracer2 = GlobalTracer.Instance;

            Assert.IsType<GlobalTracer>(tracer1);
            Assert.Same(tracer1, tracer2);
        }

        [Fact]
        public void Multiple_calls_to_Register_fail()
        {
            ITracer tracer1 = Substitute.For<ITracer>();
            ITracer tracer2 = Substitute.For<ITracer>();

            GlobalTracer.Register(tracer1);

            Assert.Throws<InvalidOperationException>(() => GlobalTracer.Register(tracer2));
        }

        [Fact]
        public void Registering_the_same_tracer_twice_does_not_throw()
        {
            ITracer tracer = Substitute.For<ITracer>();

            GlobalTracer.Register(tracer);
            GlobalTracer.Register(tracer);
        }

        [Fact]
        public void Registering_GlobalTracer_fails()
        {
            Assert.Throws<ArgumentException>(() => GlobalTracer.Register(GlobalTracer.Instance));
        }

        [Fact]
        public void Registering_null_fails()
        {
            Assert.Throws<ArgumentNullException>(() => GlobalTracer.Register(null));
        }

        [Fact]
        public void NoopTracer_is_set_by_default()
        {
            ISpanBuilder spanBuilder = GlobalTracer.Instance.BuildSpan("my-operation");
            Assert.IsType<NoopSpanBuilder>(spanBuilder);
        }

        [Fact]
        public void BuildSpan_is_forwarded_to_underlying_tracer()
        {
            ITracer tracer = Substitute.For<ITracer>();

            GlobalTracer.Register(tracer);

            GlobalTracer.Instance.BuildSpan("my-operation");

            tracer.Received(1).BuildSpan("my-operation");
        }

        [Fact]
        public void Inject_is_forwarded_to_underlying_tracer()
        {
            ITracer tracer = Substitute.For<ITracer>();
            ISpanContext spanContext = Substitute.For<ISpanContext>();
            IFormat<object> format = Substitute.For<IFormat<object>>();
            object carrier = Substitute.For<object>();

            GlobalTracer.Register(tracer);

            GlobalTracer.Instance.Inject(spanContext, format, carrier);

            tracer.Received(1).Inject(spanContext, format, carrier);
        }

        [Fact]
        public void Extract_is_forwarded_to_underlying_tracer()
        {
            ITracer tracer = Substitute.For<ITracer>();
            IFormat<object> format = Substitute.For<IFormat<object>>();
            object carrier = Substitute.For<object>();

            GlobalTracer.Register(tracer);

            GlobalTracer.Instance.Extract(format, carrier);

            tracer.Received(1).Extract(format, carrier);
        }

        [Fact]
        public void IsRegistered_returns_true_after_registration()
        {
            Assert.False(GlobalTracer.IsRegistered());

            GlobalTracer.Register(Substitute.For<ITracer>());

            Assert.True(GlobalTracer.IsRegistered());
        }
    }
}
