using System.Collections;
using System.Collections.Generic;

namespace LinqExtensions
{
    public class JoinableEnumerable<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _collection;

        public JoinableEnumerable(IEnumerable<T> collection, bool isOuter)
        {
            _collection = collection;
            IsOuter = isOuter;
        }

        public bool IsOuter { get; }

        public IEnumerator<T> GetEnumerator() => _collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _collection.GetEnumerator();
    }
}
