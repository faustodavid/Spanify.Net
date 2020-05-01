using System;
using System.Runtime.CompilerServices;

namespace Spanify.Net
{
    public static class SpanExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CommaSeparatedSpanCollection Split(this ReadOnlySpan<char> value, char separator)
        {
            return new CommaSeparatedSpanCollection(value, separator);
        }
    }
}