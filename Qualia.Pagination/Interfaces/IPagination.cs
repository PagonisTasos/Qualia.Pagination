using System.Collections.Generic;

namespace Qualia.Pagination
{
    public interface IPagination
    {
        public int PageSize { get; }
        public int TotalItems { get; }

        public int FirstPage { get; }
        public int? PreviousPage { get; }
        public int CurrentPage { get; }
        public int? NextPage { get; }
        public int LastPage { get; }

        public int TotalPages { get; }
        public IEnumerable<int>? NearPages { get; }
        public bool MultiplePages { get; }

        /// <summary>
        /// Sum of items in current page
        /// </summary>
        public int CurrentPageResults { get; }

        /// <summary>
        /// Sum of items in previous pages
        /// </summary>
        public int PreviousResults { get; }

        /// <summary>
        /// Sum of items in current and forward pages
        /// </summary>
        public int ForwardResults { get; }
    }
}
