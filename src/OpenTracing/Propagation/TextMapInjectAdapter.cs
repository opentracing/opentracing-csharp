// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextMapInjectAdapter.cs">
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
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    ///     A TextMap carrier for use with <see cref="ITracer.Inject{T}" /> ONLY (it has no read methods).
    ///     Note that the TextMap interface can be made to wrap around arbitrary data types (not just Dictionary{string,
    ///     string}
    ///     as illustrated here).
    /// </summary>
    /// <seealso cref="ITracer.Inject{T}" />
    internal sealed class TextMapInjectAdapter : TextMap
    {
        private readonly IDictionary<string, string> dictionary;

        public TextMapInjectAdapter(IDictionary<string, string> dictionary)
        {
            this.dictionary = dictionary;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            throw new InvalidOperationException(
                $"{nameof(TextMapExtractAdapter)} should only be used with {nameof(ITracer)}.{nameof(ITracer.Extract)}");
        }

        public void Put(string key, string value)
        {
            this.dictionary.Add(key, value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}