#if NET45
using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
#else
using System.Threading;
#endif

namespace OpenTracing.Util
{
    /// <summary>
    /// The <see cref="AsyncLocalScopeManager"/> is a simple <see cref="IScopeManager"/> implementation
    /// that relies on C#'s AsyncLocal/CallContext storage primitive.
    /// </summary>
    /// <seealso cref="AsyncLocalScope"/>
    public class AsyncLocalScopeManager : IScopeManager
    {
#if NET45 // AsyncLocal is .NET 4.6+, so fall back to CallContext for .NET 4.5
        private readonly string _logicalDataKey = "__AsyncLocalScope_Current__" + Guid.NewGuid().ToString("D");

        public IScope Active
        {
            get
            {
                var handle = CallContext.LogicalGetData(_logicalDataKey) as ObjectHandle;
                return handle?.Unwrap() as IScope;
            }
            set
            {
                CallContext.LogicalSetData(_logicalDataKey, new ObjectHandle(value));
            }
        }
#else
        private readonly AsyncLocal<IScope> _current = new AsyncLocal<IScope>();

        public IScope Active
        {
            get => _current.Value;
            set => _current.Value = value;
        }
#endif

        public IScope Activate(ISpan span, bool finishSpanOnDispose)
        {
            return new AsyncLocalScope(this, span, finishSpanOnDispose);
        }
    }
}
