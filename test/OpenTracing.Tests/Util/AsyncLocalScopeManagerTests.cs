using NSubstitute;
using OpenTracing.Mock;
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
        public void InstancesShouldNotShareData()
        {
            AsyncLocalScopeManager manager1 = new AsyncLocalScopeManager();
            AsyncLocalScopeManager manager2 = new AsyncLocalScopeManager();

            AsyncLocalScope manager1Scope = new AsyncLocalScope(manager1, Substitute.For<ISpan>(), false);
            manager1.Active = manager1Scope;
            AsyncLocalScope manager2Scope = new AsyncLocalScope(manager2, Substitute.For<ISpan>(), false);
            manager2.Active = manager2Scope;

            Assert.Same(manager1Scope, manager1.Active);
            Assert.Same(manager2Scope, manager2.Active);
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
