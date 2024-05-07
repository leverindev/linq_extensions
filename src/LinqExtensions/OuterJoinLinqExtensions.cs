using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqExtensions
{
    public static class OuterJoinLinqExtensions
    {
        public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(
            this IEnumerable<TOuter> outer,
            JoinableEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer = null)
        {
            bool isLeftOuter = outer is JoinableEnumerable<TOuter> left && left.IsOuter;
            bool isRightOuter = inner is object && inner.IsOuter;

            return JoinInternal(outer, isLeftOuter, inner, isRightOuter, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(
            this JoinableEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer = null)
        {
            bool isLeftOuter = outer is object && outer.IsOuter;
            bool isRightOuter = inner is JoinableEnumerable<TInner> right && right.IsOuter;

            return JoinInternal(outer, isLeftOuter, inner, isRightOuter, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(
            this JoinableEnumerable<TOuter> outer,
            JoinableEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer = null)
        {
            bool isLeftOuter = outer is object && outer.IsOuter;
            bool isRightOuter = inner is object && inner.IsOuter;

            return JoinInternal(outer, isLeftOuter, inner, isRightOuter, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        private static IEnumerable<TResult> JoinInternal<TOuter, TInner, TKey, TResult>(
            this IEnumerable<TOuter> outer,
            bool isLeftOuter,
            IEnumerable<TInner> inner,
            bool isRightOuter,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer = null)
        {
            if (outer == null)
            {
                throw new ArgumentNullException(nameof(outer));
            }

            if (inner == null)
            {
                throw new ArgumentNullException(nameof(inner));
            }

            if (outerKeySelector == null)
            {
                throw new ArgumentNullException(nameof(outerKeySelector));
            }

            if (innerKeySelector == null)
            {
                throw new ArgumentNullException(nameof(innerKeySelector));
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            if (isLeftOuter && isRightOuter)
            {
                return FullOuterJoin(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
            }

            if (isLeftOuter)
            {
                return LeftOuterJoin(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
            }

            if (isRightOuter)
            {
                return RightOuterJoin(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
            }

            return outer.Join(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        private static IEnumerable<TResult> LeftOuterJoin<TOuter, TInner, TKey, TResult>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer = null)
        {
            return outer
                .GroupJoin(
                    inner,
                    outerKeySelector,
                    innerKeySelector,
                    (outerObj, inners) => new { outerObj, inners = inners.DefaultIfEmpty() })
                .SelectMany(jItem => jItem.inners.Select(innerObj => resultSelector(jItem.outerObj, innerObj)));
        }

        private static IEnumerable<TResult> RightOuterJoin<TOuter, TInner, TKey, TResult>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer = null)
        {
            return inner
                .GroupJoin(
                    outer,
                    innerKeySelector,
                    outerKeySelector,
                    (innerObj, outers) => new { innerObj, outers = outers.DefaultIfEmpty() })
                .SelectMany(jItem => jItem.outers.Select(innerObj => resultSelector(innerObj, jItem.innerObj)));
        }

        private static IEnumerable<TResult> FullOuterJoin<TOuter, TInner, TKey, TResult>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer = null)
        {
            var tOuter = outer.ToList();
            var tInner = inner.ToList();

            return
                LeftOuterJoin(tOuter, tInner, outerKeySelector, innerKeySelector, resultSelector, comparer)
                    .Union(RightOuterJoin(tOuter, tInner, outerKeySelector, innerKeySelector, resultSelector, comparer));
        }
    }
}
