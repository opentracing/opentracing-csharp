// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IActiveSpan.cs">
//   Copyright 2017-2018 The OpenTracing Authors
//   
//   Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except
//   in compliance with the License. You may obtain a copy of the License at
//   
//   http://www.apache.org/licenses/LICENSE-2.0
// 
//   Unless required by applicable law or agreed to in writing, software distributed under the License
//   is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//   or implied. See the License for the specific language governing permissions and limitations under
//   the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OpenTracing
{
    using System;

    /// <summary>
    ///     <see cref="IActiveSpan" /> inherits all of the OpenTracing functionality in <see cref="IBaseSpan{T}" /> and layers
    ///     on in-process
    ///     propagation capabilities.
    ///     <para>
    ///         In any given thread there is at most one <see cref="IActiveSpan" /> primarily responsible for the work
    ///         accomplished by the surrounding application code. That <see cref="IActiveSpan" /> may be accessed via the
    ///         <see cref="IActiveSpanSource.ActiveSpan" /> method. If the application needs to defer work that sould be part
    ///         of
    ///         the same Span, the Source provides a <see cref="Capture" /> method that returns a <see cref="IContinuation" />;
    ///         this continuation may be used to re-activate and continue the <see cref="ISpan" /> in that other asynchronous
    ///         executor
    ///         and/or thread.
    ///     </para>
    ///     <para>
    ///         <see cref="IActiveSpan" /> are created via <see cref="ISpanBuilder.StartActive" /> or, less commonly,
    ///         <see cref="IActiveSpanSource.MakeActive" />. Per the above, they can be <see cref="Capture" />d as
    ///         <see cref="IContinuation" />s, then re-<see cref="IContinuation.Activate" />d later.
    ///     </para>
    ///     <para>
    ///         NOTE: <see cref="IDisposable.Dispose" /> on this method is equivalent to <see cref="Deactivate" />.
    ///     </para>
    /// </summary>
    /// <seealso cref="ISpanBuilder.StartActive" />
    /// <seealso cref="IContinuation.Activate" />
    /// <seealso cref="IActiveSpanSource" />
    /// <seealso cref="IBaseSpan{T}" />
    /// <seealso cref="ISpan" />
    internal interface IActiveSpan : IBaseSpan<IActiveSpan>, IDisposable
    {
        /// <summary>
        ///     Mark the end of the active period for the current thread and <see cref="IActiveSpan" />. When the last
        ///     <see cref="IActiveSpan" /> is deactivated for a given <see cref="ISpan" />, it is automatically
        ///     <see cref="ISpan.Finish()" />ed.
        ///     <para>
        ///         NOTE: Calling <see cref="Deactivate" /> more than once on a single <see cref="IActiveSpan" /> instance leads to
        ///         undefined
        ///         behavior.
        ///     </para>
        /// </summary>
        /// <seealso cref="IDisposable.Dispose" />
        void Deactivate();

        /// <summary>
        ///     "Capture" a new <see cref="IContinuation" /> associated with this <see cref="IActiveSpan" /> and
        ///     <see cref="ISpan" />, as well as any
        ///     3rd-party execution context of interest. The <see cref="IContinuation" /> may be used as data in a closure or
        ///     callback
        ///     function where the <see cref="IActiveSpan" /> may be resumed and reactivated.
        ///     <para>
        ///         <em>IMPORTANT:</em> the caller MUST <see cref="IContinuation.Activate" /> and <see cref="Deactivate" /> the
        ///         returned <see cref="IContinuation" /> or the associated <see cref="ISpan" /> will never automatically
        ///         <see cref="ISpan.Finish()" />.
        ///         That is, calling <see cref="Capture" /> increments a refcount that must be decremented somewhere else.
        ///     </para>
        ///     <para>
        ///         The associated <see cref="ISpan" /> will not <see cref="ISpan.Finish()" /> while a <see cref="IContinuation" />
        ///         is outstanding; in
        ///         this way, it provides a reference/pin just like an <see cref="IActiveSpan" /> does.
        ///     </para>
        /// </summary>
        /// <returns>A new <see cref="IContinuation" /> to <see cref="IContinuation.Activate" /> at the appropriate time.</returns>
        IContinuation Capture();
    }

    /// <summary>
    ///     A <see cref="IContinuation" /> can be used <em>once</em> to activate a Span along with any non-OpenTracing
    ///     execution
    ///     context (e.g., MDC), then deactivate when processing activity moves on to another Span. (In practice, this
    ///     active period typically extends for the length of a deferred async closure invocation.)
    ///     <para>
    ///         Most users do not directly interact with <see cref="IContinuation" />, <see cref="Activate" /> or
    ///         <see cref="IActiveSpan.Deactivate" />, but rather use <see cref="IActiveSpanSource" />-aware
    ///         Runnables/Callables/Executors.
    ///         Those higher-level primatives do not <em>need</em> to be defined within the OpenTracing core API, and so
    ///         they are not.
    ///     </para>
    /// </summary>
    /// <seealso cref="IActiveSpanSource.MakeActive" />
    internal interface IContinuation
    {
        /// <summary>
        ///     Make the Span (and other execution context) encapsulated by this <see cref="IContinuation" /> active and
        ///     return it.
        ///     <para>
        ///         NOTE: It is an error to call activate() more than once on a single <see cref="IContinuation" /> instance.
        ///     </para>
        /// </summary>
        /// <returns>A handle to the newly-activated <see cref="IActiveSpan" /></returns>
        /// <seealso cref="IActiveSpanSource.MakeActive" />
        IActiveSpan Activate();
    }
}