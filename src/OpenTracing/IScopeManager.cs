using System;

namespace OpenTracing
{
    /// <summary>
    /// The <see cref="IScopeManager"/> interface abstracts both the activation of <see cref="ISpan"/> instances (via
    /// <see cref="Activate(ISpan, bool)"/>) and access to an active <see cref="ISpan"/>/<see cref="IScope"/> (via
    /// <see cref="Active"/>. <seealso cref="IScope"/> <seealso cref="ITracer.ScopeManager"/>
    /// </summary>
    public interface IScopeManager
    {
        /// <summary>
        /// Return the currently active <see cref="IScope"/> which can be used to access the currently active
        /// <see cref="IScope.Span"/>.
        /// <para>
        /// If there is an non-null <see cref="IScope"/>, its wrapped <see cref="ISpan"/> becomes an implicit parent of any
        /// newly-created <see cref="ISpan"/> at <see cref="ISpanBuilder.StartActive(bool)"/> time (rather than at
        /// <see cref="ITracer.BuildSpan"/> time).
        /// </para>
        /// </summary>
        /// <returns>The active <see cref="IScope"/>, or null if none could be found.</returns>
        IScope Active { get; }

        /// <summary>Make a <see cref="ISpan"/> instance active.</summary>
        /// <param name="span">The <see cref="ISpan"/> that should become the <see cref="Active"/>.</param>
        /// <param name="finishSpanOnDispose">
        /// Whether span should automatically be finished when <see cref="IDisposable.Dispose"/> is called.
        /// </param>
        /// <returns>
        /// A <see cref="IScope"/> instance to control the end of the active period for the <see cref="ISpan"/>.
        /// It is a programming error to neglect to call <see cref="IDisposable.Dispose"/> on the returned instance.
        /// </returns>
        IScope Activate(ISpan span, bool finishSpanOnDispose);
    }
}
