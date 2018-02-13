using NSubstitute;
using OpenTracing.Util;
using Xunit;

namespace OpenTracing.Tests.Util
{
    public class AsyncLocalScopeManagerTests
    {
        private AsyncLocalScopeManager _source;


        public AsyncLocalScopeManagerTests()
        {
            _source = new AsyncLocalScopeManager();
        }

        [Fact]
        public void MissingActiveScope()
        {
            IScope missingScope = _source.Active;
            Assert.Null(missingScope);
        }

        [Fact]
        public void DefaultActivate()
        {
            ISpan span = Substitute.For<ISpan>();

            using (IScope scope = _source.Activate(span, finishSpanOnDispose: false))
            {
                Assert.NotNull(scope);
                IScope otherScope = _source.Active;
                Assert.Same(otherScope, scope);
            }

            // Make sure the span is not finished.
            span.DidNotReceive().Finish();

            // And now it's gone:
            IScope missingScope = _source.Active;
            Assert.Null(missingScope);
        }

        [Fact]
        public void FinishSpanOnDispose()
        {
            ISpan span = Substitute.For<ISpan>();

            using (IScope scope = _source.Activate(span, finishSpanOnDispose: true))
            {
                Assert.NotNull(scope);
                Assert.NotNull(_source.Active);
            }

            // Make sure the ISpan got finish()ed.
            span.Received(1).Finish();

            // Verify it's gone.
            Assert.Null(_source.Active);
        }

        [Fact]
        public void DontFinishSpanNoDispose()
        {
            ISpan span = Substitute.For<ISpan>();

            using (IScope scope = _source.Activate(span, finishSpanOnDispose: false))
            {
                Assert.NotNull(scope);
                Assert.NotNull(_source.Active);
            }

            // Make sure the ISpan did *not* get finish()ed.
            span.DidNotReceive().Finish();

            // Verify it's gone.
            Assert.Null(_source.Active);
        }
    }
}
