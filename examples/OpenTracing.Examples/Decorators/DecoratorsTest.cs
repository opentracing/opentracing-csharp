using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OpenTracing.Decorators;
using OpenTracing.Mock;
using OpenTracing.Noop;
using Xunit;
using Xunit.Abstractions;

namespace OpenTracing.Examples.Decorators
{
    public class DecoratorsTest
    {
        private readonly ITracer _tracer = new MockTracer();
        private readonly ITestOutputHelper _output;

        public DecoratorsTest(ITestOutputHelper output)
        {
            _output = output;
        }

        class TestOutputTracerDecorator : TracerDecorator
        {
            private readonly ITestOutputHelper _output;

            public TestOutputTracerDecorator(ITracer tracer, ITestOutputHelper output) : base(tracer)
            {
                _output = output;
            }

            public override ISpanBuilder BuildSpan(string operationName)
            {
                _output.WriteLine($"Building span named {operationName}");
                return base.BuildSpan(operationName);
            }
        }

        class TestOutputSpanBuilderDecorator : SpanBuilderDecorator
        {
            private readonly ITestOutputHelper _output;

            public TestOutputSpanBuilderDecorator(ISpanBuilder spanBuilder, ITestOutputHelper output) : base(spanBuilder)
            {
                _output = output;
            }

            public override ISpan Start()
            {
                var span = base.Start();
                _output.WriteLine($"Span Started: {span}");
                return span;
            }

            public override IScope StartActive()
            {
                var scope = base.StartActive();
                _output.WriteLine($"Scope Started: {scope.Span}");
                return scope;
            }

            public override IScope StartActive(bool finishSpanOnDispose)
            {
                var scope = base.StartActive(finishSpanOnDispose);
                _output.WriteLine($"Scope Started: {scope.Span}");
                return scope;
            }
        }

        class TestOutputScopeDecorator : ScopeDecorator
        {
            private readonly ITestOutputHelper _output;

            public TestOutputScopeDecorator(IScope scope, ITestOutputHelper output) : base(scope)
            {
                _output = output;
            }

            public override void Dispose()
            {
                base.Dispose();
                _output.WriteLine($"Scope disposed: {Span}");
            }
        }

        class TestOutputSpanDecorator : SpanDecorator
        {
            private readonly ITestOutputHelper _output;

            public TestOutputSpanDecorator(ISpan span, ITestOutputHelper output) : base(span)
            {
                _output = output;
            }

            public override void Finish()
            {
                base.Finish();
                _output.WriteLine($"Span Finished: {this}");
            }
        }

        [Fact]
        public async Task Test()
        {
            var builder = new TracerDecoratorBuilder(_tracer)
                .WithTracerDecorator(tracer => new TestOutputTracerDecorator(tracer, _output))
                .WithSpanBuilderDecorator(spanBuilder => new TestOutputSpanBuilderDecorator(spanBuilder, _output))
                .WithScopeDecorator(scope => new TestOutputScopeDecorator(scope, _output))
                .WithSpanDecorator(span => new TestOutputSpanDecorator(span, _output))
                ;

            var sut = builder.Build();

            using (var scope = sut.BuildSpan("StartActive(fasle)").StartActive(false))
            {
                var span = sut.BuildSpan("Start()").Start();

                try
                {
                    _output.WriteLine("--> Doing something 1");
                    await Task.Delay(10);
                }
                finally
                {
                    span.Finish();
                }

                using (sut.BuildSpan("StartActive()").StartActive())
                {
                    await Task.Delay(10);
                    _output.WriteLine("--> Doing something 2");
                }


                scope.Span.Finish();
            }
        }
    }
}
