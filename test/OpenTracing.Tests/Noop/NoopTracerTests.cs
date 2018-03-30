using OpenTracing.Noop;
using OpenTracing.Tag;
using Xunit;

namespace OpenTracing.Tests.Noop
{
    public class NoopTracerTest
    {

        [Fact]
        public void ActiveSpanValueToleratesUse()
        {
            ISpan activeSpan = NoopTracer.Instance.ActiveSpan;
            Assert.NotNull(activeSpan);
            Tags.Error.Set(activeSpan, true);
        }
    }
}
