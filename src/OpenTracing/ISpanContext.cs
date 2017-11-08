﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISpanContext.cs">
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
    using System.Collections.Generic;

    /// <summary>
    ///     SpanContext represents Span state that must propagate to descendant Spans and across process boundaries.
    ///     SpanContext is logically divided into two pieces: (1) the user-level "Baggage" that propagates across Span
    ///     boundaries and (2) any Tracer-implementation-specific fields that are needed to identify or otherwise contextualize
    ///     the associated Span instance(e.g., a { trace_id, span_id, sampled } tuple).
    /// </summary>
    /// <seealso cref="IBaseSpan{T}.SetBaggageItem" />
    /// <seealso cref="IBaseSpan{T}.GetBaggaggeItem" />
    internal interface ISpanContext
    {
        /// <returns>All zero or more baggage items propagating along with the associated Span</returns>
        /// <seealso cref="IBaseSpan{T}.SetBaggageItem" />
        /// <seealso cref="IBaseSpan{T}.GetBaggaggeItem" />
        IEnumerable<KeyValuePair<string, string>> BaggageItems();
    }
}