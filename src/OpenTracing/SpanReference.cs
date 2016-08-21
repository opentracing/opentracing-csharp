using System;

namespace OpenTracing
{
    public class SpanReference
    {
        public const string TypeChildOf = "child_of";
        public const string TypeFollowsFrom = "follows_from";

        public static SpanReference ChildOf(ISpanContext parent)
        {
            return parent != null ? new SpanReference(TypeChildOf, parent) : null;
        }

        public static SpanReference FollowsFrom(ISpanContext parent)
        {
            return parent != null ? new SpanReference(TypeFollowsFrom, parent) : null;
        }

        public string Type { get; }
        public ISpanContext Context { get; }

        public SpanReference(string referenceType, ISpanContext context)
        {
            if (string.IsNullOrWhiteSpace(referenceType))
            {
                throw new ArgumentNullException(nameof(referenceType));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Type = referenceType;
            Context = context;
        }
    }
}