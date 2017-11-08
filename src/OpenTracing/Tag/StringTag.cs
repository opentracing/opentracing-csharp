﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringTag.cs">
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

namespace OpenTracing.Tag
{
    internal sealed class StringTag : AbstractTag<string>
    {
        public StringTag(string tagKey)
            : base(tagKey)
        {
        }

        protected override void Set<TSpan>(IBaseSpan<TSpan> span, string tagValue)
        {
            span.SetTag(this.Key, tagValue);
        }

        public void Set<TSpan>(IBaseSpan<TSpan> span, StringTag tag)
            where TSpan : IBaseSpan<TSpan>
        {
            // TODO: That doesn't look right?
            span.SetTag(this.Key, tag.Key);
        }
    }
}