using System.Collections.Generic;

namespace Qualia.Pagination
{
    internal class Page<T> : IPage<T>
    {
        public IEnumerable<T>? Content { get; set; }

        public IPagination? Pagination { get; set; }
    }
}
