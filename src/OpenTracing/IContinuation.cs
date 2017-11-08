// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IContinuation.cs">
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
    public interface IContinuation
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