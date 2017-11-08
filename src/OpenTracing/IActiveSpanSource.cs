// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IActiveSpanSource.cs">
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
    /// <summary>
    ///     <see cref="IActiveSpanSource" /> allows an existing (possibly thread-local-aware) execution context provider to act
    ///     as a
    ///     source for an actively-scheduled OpenTracing Span.
    ///     <para>
    ///         <see cref="IActiveSpanSource" /> is a super-interface to <see cref="ITracer" />, so note that all
    ///         <see cref="ITracer" />s fulfill the
    ///         <see cref="IActiveSpanSource" /> contract.
    ///     </para>
    /// </summary>
    /// <seealso cref="IActiveSpan" />
    internal interface IActiveSpanSource
    {
        /// <summary>
        ///     Return the <see cref="IActiveSpan" />. This does not affect the internal reference count for the
        ///     <see cref="IActiveSpan" />.
        ///     <para>
        ///         If there is an <see cref="IActiveSpan" />, it becomes an implicit parent of any newly-created
        ///         <see cref="IBaseSpan{T}" /> at <see cref="ISpanBuilder.StartActive" /> time (rather than at
        ///         <see cref="ITracer.BuildSpan" /> time).
        ///     </para>
        /// </summary>
        /// <returns>The <see cref="IActiveSpan" />, or null if none could be found.</returns>
        IActiveSpan ActiveSpan();

        /// <summary>
        ///     Wrap and "make active" a <see cref="ISpan" /> by encapsulated it - and any active state (e.g., MDC state) in the
        ///     current thread - in a new <see cref="IActiveSpan" />.
        /// </summary>
        /// <param name="span">The SPan to wrap in a <see cref="IActiveSpan" /></param>
        /// <returns>
        ///     An <see cref="IActiveSpan" /> that encapsulates the given <see cref="ISpan" /> and any other
        ///     <see cref="IActiveSpanSource" />-specific context (e.g., the MDC context map)
        /// </returns>
        IActiveSpan MakeActive(ISpan span);
    }
}