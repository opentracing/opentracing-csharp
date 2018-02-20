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
        private static readonly string s_logicalDataKey = "__AsyncLocalScope_Current__" + AppDomain.CurrentDomain.Id;

        public IScope Active
        {
            get
            {
                var handle = CallContext.LogicalGetData(s_logicalDataKey) as ObjectHandle;
                return handle?.Unwrap() as IScope;
            }
            set
            {
                CallContext.LogicalSetData(s_logicalDataKey, new ObjectHandle(value));
            }
        }
#else
        private static AsyncLocal<IScope> s_current = new AsyncLocal<IScope>();

        public IScope Active
        {
            get => s_current.Value;
            set => s_current.Value = value;
        }
#endif

        public IScope Activate(ISpan span, bool finishSpanOnDispose)
        {
            return new AsyncLocalScope(this, span, finishSpanOnDispose);
        }
    }
}
