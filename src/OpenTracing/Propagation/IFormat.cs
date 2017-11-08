// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFormat.cs">
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
    /// <summary>
    ///     Format instances control the behavior of <see cref="ITracer.Inject{T}" /> and <see cref="ITracer.Extract{T}" />
    ///     (and also constrain the type of the
    ///     carrier parameter to same).
    ///     Most OpenTracing users will only reference the <see cref="BuiltinFormats" /> constants. For example:
    ///     <code>
    /// TODO: This sample looks a bit wonky, missed update at some point?
    /// Tracer tracer = ...
    /// IFormat{TextMap} httpCarrier = new AnHttpHeaderCarrier(httpRequest);
    /// SpanContext spanCtx = tracer.Extract(BuiltinFormats.HttpHeaders, httpHeaderRequest);
    /// </code>
    /// </summary>
    /// <seealso cref="ITracer.Inject{T}" />
    /// <seealso cref="ITracer.Extract{T}" />
    public interface IFormat<T>
    {
    }
}