using System.Collections.Generic;

namespace Qualia.Pagination
{
    internal class Pagination : IPagination
    {
        public int PageSize { get; set; }

        public int FirstPage { get; set; }

        public int? PreviousPage { get; set; }

        public int CurrentPage { get; set; }

        public int? NextPage { get; set; }

        public int LastPage { get; set; }

        public int TotalPages { get; set; }

        public int TotalItems { get; set; }

        public IEnumerable<int>? NearPages { get; set; }

        public bool MultiplePages { get; set; }
        public int CurrentPageResults { get; set; }
        public int PreviousResults { get; set; }
        public int ForwardResults { get; set; }
    }
}
