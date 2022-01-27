using System.Collections.Generic;

namespace Qualia.Pagination
{
    public interface IPage<T>
    {
        public IEnumerable<T>? Content { get; }
        public IPagination? Pagination { get; }
    }
}
