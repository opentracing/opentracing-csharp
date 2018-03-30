using OpenTracing.Noop;
using Xunit;

namespace OpenTracing.Tests.Noop
{
    public class NoopScopeManagerTests
    {

        [Fact]
        public void ActiveValueToleratesUse()
        {
            IScope active = NoopScopeManager.Instance.Active;
            Assert.NotNull(active);
            active.Dispose();
        }
    }
}
