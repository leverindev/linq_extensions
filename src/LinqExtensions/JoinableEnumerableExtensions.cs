using System.Collections.Generic;

namespace LinqExtensions
{
    public static class JoinableEnumerableExtensions
    {
        public static JoinableEnumerable<TElement> ToOuter<TElement>(this IEnumerable<TElement> source) =>
            source as JoinableEnumerable<TElement> ?? new JoinableEnumerable<TElement>(source, true);
    }
}
