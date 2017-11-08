// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextMap.cs">
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

namespace OpenTracing.Propagation
{
    using System.Collections.Generic;

    /// <summary>
    ///     <see cref="TextMap" /> is a built-in carrier for <see cref="ITracer.Inject{T}" /> and
    ///     <see cref="ITracer.Extract{T}" />. TextMap implementations allows Tracers to
    ///     read and write key:value String pairs from arbitrary underlying sources of data.
    /// </summary>
    /// <seealso cref="ITracer.Inject{T}" />
    /// <seealso cref="ITracer.Extract{T}" />
    public interface TextMap : IEnumerable<KeyValuePair<string, string>>
    {
        /// <summary>
        ///     Puts a key:value pair into the TextMapWriter's backing store.
        /// </summary>
        /// <param name="key">A string, possibly with constraints dictated by the particular Format this TextMap is paired with</param>
        /// <param name="value">A string, possibly with constraints dictated by the particular Format this TextMap is paired with</param>
        /// <seealso cref="ITracer.Inject{T}" />
        /// <seealso cref="BuiltinFormats.TextMap" />
        /// <seealso cref="BuiltinFormats.HttpHeaders" />
        void Put(string key, string value);
    }
}