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

namespace OpenTracing.Examples.CommonRequestHandler
{
    public class Client
    {
        private readonly RequestHandler requestHandler;

        public Client(RequestHandler requestHandler)
        {
            this.requestHandler = requestHandler;
        }

        public async Task<String> Send(String message)
        {
            var context = new Context();

            await Task.Run(async () =>
            {
                await Task.Delay(50);
                requestHandler.BeforeRequest(message, context);
            });

            await Task.Run(async () =>
            {
                await Task.Delay(50);
                requestHandler.AfterResponse(message, context);
            });

            return $"{message}:response";
        }
    }
}