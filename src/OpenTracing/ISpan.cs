// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISpan.cs">
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
    ///     Represents an in-flight Span that's <em>manually propagated</em> within the given process. Most of
    ///     the PAI lives in <see cref="IBaseSpan{T}" />
    ///     <para>
    ///         <see cref="ISpan" />s are created by the <see cref="ISpanBuilder.StartManual" /> method; see
    ///         <see cref="IActiveSpan" /> for
    ///         a <see cref="IBaseSpan{T}" /> extension designed for automatic in-process propagation.
    ///     </para>
    ///     <seealso cref="IActiveSpan" /> for automatic propagation (recommended for most instrumentation!)
    /// </summary>
    public interface ISpan : IBaseSpan<ISpan>
    {
        /// <summary>
        ///     Sets the end timestamp to now and records the span.
        ///     <para>
        ///         With the exception of calls to <see cref="IBaseSpan{T}.Context" />, this should be the last call made to the
        ///         span instance.
        ///         Future calls to <see cref="Finish()" /> are defined as noops, and future calls to methods other than
        ///         <see cref="IBaseSpan{T}.Context" />
        ///         lead to undefined behavior (likely an exception).
        ///     </para>
        /// </summary>
        /// <seealso cref="IBaseSpan{T}.Context" />
        void Finish();

        /// <summary>
        ///     Sets an explicit end timestamp and records the span.
        ///     <para>
        ///         With the exception of calls to Span.context(), this should be the last call made to the span instance, and to
        ///         do otherwise leads to undefined behavior.
        ///     </para>
        /// </summary>
        /// <param name="finishTime">An explicit finish time</param>
        /// <seealso cref="IBaseSpan{T}.Context" />
        void Finish(DateTimeOffset finishTime);
    }
}