using System;

namespace OpenTracing
{
    /// <summary>
    /// A <see cref="IScope"/> formalizes the activation and deactivation of a <see cref="ISpan"/>, usually from a CPU
    /// standpoint.
    /// <para>
    /// Many times a <see cref="ISpan"/> will be extant (in that <see cref="ISpan.Finish()"/> has not been called)
    /// despite being in a
    /// non-runnable state from a CPU/scheduler standpoint. For instance, a <see cref="ISpan"/> representing the child
    /// side of an
    /// RPC will be unfinished but blocked on IO while the RPC is still outstanding. A <see cref="IScope"/> defines
    /// when a given
    /// <see cref="ISpan"/> <em>is</em> scheduled and on the path.
    /// </para>
    /// <para>
    /// Calling <see cref="IScope.Dispose"/> marks the end of the active period for the current thread and
    /// <see cref="IScope"/>,
    /// updating the <see cref="IScopeManager.Active"/> in the process.
    /// </para>
    /// </summary>
    public interface IScope : IDisposable
    {
        /// <summary>
        /// The <see cref="ISpan"/> that's been scoped by this <see cref="IScope"/>
        /// </summary>
        ISpan Span { get; }
    }

    /// <summary>
    /// The <see cref="IScopeManager"/> interface abstracts both the activation of <see cref="ISpan"/> instances (via
    /// <see cref="Activate(ISpan)"/>) and access to an active <see cref="ISpan"/>/<see cref="IScope"/>
    /// (via <see cref="Active"/>.
    /// <seealso cref="IScope"/>
    /// <seealso cref="ITracer.ScopeManager"/>
    /// </summary>
    public interface IScopeManager
    {
        /// <summary>
        /// Return the currently active <see cref="IScope"/> which can be used to access the currently active
        /// <see cref="IScope.Span"/>.
        /// <para>
        /// If there is an non-null <see cref="IScope"/>, its wrapped <see cref="ISpan"/> becomes an implicit parent of
        /// any
        /// newly-created <see cref="ISpan"/> at <see cref="ISpanBuilder.StartActive()"/> time (rather than at
        /// <see cref="ITracer.BuildSpan"/> time).
        /// </para>
        /// </summary>
        /// <returns>The active <see cref="IScope"/>, or null if none could be found.</returns>
        IScope Active { get; }

        /// <summary>
        /// Make a <see cref="ISpan"/> instance active.
        /// </summary>
        /// <param name="span">The <see cref="ISpan"/> that should become the <see cref="Active"/></param>
        /// <returns>
        /// A <see cref="IScope"/> instance to control the end of the active period for the <see cref="ISpan"/>.
        /// Span will automatically be finished when <see cref="IScope.Dispose"/> is called. It is a
        /// programming error to neglect to call <see cref="IScope.Dispose"/> on the returned instance.
        /// </returns>
        IScope Activate(ISpan span);

        /// <summary>
        /// Make a <see cref="ISpan"/> instance active.
        /// </summary>
        /// <param name="span">The <see cref="ISpan"/> that should become the <see cref="Active"/></param>
        /// <param name="finishSpanOnClose">
        /// Whether span should automatically be finished when <see cref="IDisposable.Dispose"/>
        /// is called
        /// </param>
        /// <returns>
        /// A <see cref="IScope"/> instance to control the end of the active period for the <see cref="ISpan"/>.
        /// Span will automatically be finished when <see cref="IScope.Dispose"/> is called. It is a
        /// programming error to neglect to call <see cref="IScope.Dispose"/> on the returned instance.
        /// </returns>
        IScope Activate(ISpan span, bool finishSpanOnClose);
    }
}