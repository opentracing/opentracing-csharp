/*
 * Copyright 2016-2018 The OpenTracing Authors
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except
 * in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under the License
 * is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
 * or implied. See the License for the specific language governing permissions and limitations under
 * the License.
 */
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenTracing.Tag;

namespace OpenTracing.Examples.ListenerPerRequest
{
    public class Client
    {
        private readonly ITracer tracer;

        public Client(ITracer tracer)
        {
            this.tracer = tracer;
        }

        // Async execution
        private Task<String> Execute(String message, ResponseListener responseListener)
        {
            return Task.Run(() =>
            {
                // send via wire and get response
                String response = $"{message}:response";
                responseListener.OnResponse(response);
                return response;
            });
        }

        public Task<String> Send(String message)
        {
            ISpan span = tracer.BuildSpan("send").
                    WithTag(Tags.SpanKind.Key, Tags.SpanKindClient)
                    .Start();
            return Execute(message, new ResponseListener(span));
        }
    }
}