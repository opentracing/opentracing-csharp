using System;

namespace OpenTracing
{
    /// <summary>
    /// A <see cref="IScope"/> formalizes the activation and deactivation of a <see cref="ISpan"/>, usually from a CPU
    /// standpoint.
    /// <para>
    /// Many times a <see cref="ISpan"/> will be extant (in that <see cref="ISpan.Finish()"/> has not been called)
    /// despite being in a non-runnable state from a CPU/scheduler standpoint. For instance, a <see cref="ISpan"/> representing
    /// the child side of an RPC will be unfinished but blocked on IO while the RPC is still outstanding. A
    /// <see cref="IScope"/> defines when a given <see cref="ISpan"/> <em>is</em> scheduled and on the path.
    /// </para>
    /// <para>
    /// Calling <see cref="IDisposable.Dispose"/> marks the end of the active period for the current thread and
    /// <see cref="IScope"/>, updating the <see cref="IScopeManager.Active"/> in the process.
    /// </para>
    /// </summary>
    public interface IScope : IDisposable
    {
        /// <summary>The <see cref="ISpan"/> that's been scoped by this <see cref="IScope"/>.</summary>
        ISpan Span { get; }
    }
}
