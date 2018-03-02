using System.Threading.Tasks;
using NSubstitute;
using OpenTracing.Util;
using Xunit;

namespace OpenTracing.Tests.Util
{
    public class AsyncLocalScopeTests
    {
        private readonly AsyncLocalScopeManager _scopeManager;

        public AsyncLocalScopeTests()
        {
            _scopeManager = new AsyncLocalScopeManager();
        }

        [Fact]
        public void ImplicitSpanStack()
        {
            ISpan backgroundSpan = Substitute.For<ISpan>();
            ISpan foregroundSpan = Substitute.For<ISpan>();

            using (IScope backgroundActive = _scopeManager.Activate(backgroundSpan, finishSpanOnDispose: true))
            {
                Assert.NotNull(backgroundActive);

                // Activate a new Scope on top of the background one.
                using (IScope foregroundActive = _scopeManager.Activate(foregroundSpan, finishSpanOnDispose: true))
                {
                    IScope shouldBeForeground = _scopeManager.Active;
                    Assert.Same(foregroundActive, shouldBeForeground);
                }

                // And now the backgroundActive should be reinstated.
                IScope shouldBeBackground = _scopeManager.Active;
                Assert.Same(backgroundActive, shouldBeBackground);
            }

            // The background and foreground spans should be finished.
            backgroundSpan.Received(1).Finish();
            foregroundSpan.Received(1).Finish();

            // And now nothing is active.
            IScope missingSpan = _scopeManager.Active;
            Assert.Null(missingSpan);
        }

        [Fact]
        public async Task ImplicitSpanStack_with_Async()
        {
            ISpan backgroundSpan = Substitute.For<ISpan>();
            ISpan foregroundSpan = Substitute.For<ISpan>();

            using (IScope backgroundActive = _scopeManager.Activate(backgroundSpan, finishSpanOnDispose: true))
            {
                Assert.NotNull(backgroundActive);

                await Task.Delay(10);

                // Activate a new Scope on top of the background one.
                using (IScope foregroundActive = _scopeManager.Activate(foregroundSpan, finishSpanOnDispose: true))
                {
                    await Task.Delay(10);

                    IScope shouldBeForeground = _scopeManager.Active;
                    Assert.Same(foregroundActive, shouldBeForeground);
                }

                await Task.Delay(10);

                // And now the backgroundActive should be reinstated.
                IScope shouldBeBackground = _scopeManager.Active;
                Assert.Same(backgroundActive, shouldBeBackground);
            }

            await Task.Delay(10);

            // The background and foreground spans should be finished.
            backgroundSpan.Received(1).Finish();
            foregroundSpan.Received(1).Finish();

            // And now nothing is active.
            IScope missingSpan = _scopeManager.Active;
            Assert.Null(missingSpan);
        }

        [Fact]
        public void TestDeactivateWhenDifferentSpanIsActive()
        {
            ISpan span = Substitute.For<ISpan>();

            using (_scopeManager.Activate(span, finishSpanOnDispose: false))
            {
                _scopeManager.Activate(Substitute.For<ISpan>(), finishSpanOnDispose: false);
            }

            span.DidNotReceive().Finish();
        }
}
}
