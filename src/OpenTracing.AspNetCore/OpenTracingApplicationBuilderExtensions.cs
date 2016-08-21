using System;
using OpenTracing.AspNetCore;

namespace Microsoft.AspNetCore.Builder
{
    public static class OpenTracingApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseOpenTracing(this IApplicationBuilder builder, OpenTracingOptions options = null)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            options = options ?? new OpenTracingOptions();

            builder.UseMiddleware<OpenTracingMiddleware>(options);

            return builder;
        }
    }
}