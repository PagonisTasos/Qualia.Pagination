using System;
using System.Collections.Generic;
using System.Linq;

namespace Qualia.Pagination
{
    internal class Paginator
    {
        private const int _defaultItemsPerPage = 10;
        private const int _defaultCurrentPage = 1;
        private const int _defaultNearPagesMax = 5;

        public IPagination Paginate(int totalItems
                                , int? itemsPerPage = null
                                , int? currentPage = null
                                , int? nearPagesMax = null
                                  )
        {
            EnsureNonNegative(ref totalItems);

            int pageSize = itemsPerPage.GetValueOrDefault(_defaultItemsPerPage);
            EnsureNatural(ref pageSize);

            int lastPage = CalculatePages(totalItems, pageSize);
            EnsureNatural(ref lastPage);

            int page = currentPage.GetValueOrDefault(_defaultCurrentPage);
            EnsureInRange(ref page, 1, lastPage);

            int otherPages = nearPagesMax.GetValueOrDefault(_defaultNearPagesMax);

            int nextPage = page + 1;
            int previousPage = page - 1;
            int previousResultsNr = pageSize * previousPage;
            int forwardResultsNr = totalItems - previousResultsNr;
            int currentPageResultsNr = Math.Min(pageSize, forwardResultsNr);

            var nearPages = CalculateNearPages(page, lastPage, otherPages);
            bool hasMultiplePages = lastPage > 1;

            return new Pagination
            {
                TotalItems = totalItems,
                PageSize = pageSize,
                CurrentPage = page,
                FirstPage = 1,
                NextPage = NullIfOutOfRange(nextPage, 1, lastPage),
                PreviousPage = NullIfOutOfRange(previousPage, 1, lastPage),
                LastPage = lastPage,
                TotalPages = lastPage,
                PreviousResults = previousResultsNr,
                ForwardResults = forwardResultsNr,
                CurrentPageResults = currentPageResultsNr,
                NearPages = nearPages,
                MultiplePages = hasMultiplePages,
            };
        }


        public IPage<Tout> Paginate<Tin, Tout>(IEnumerable<Tin> content
                                        , Func<Tin, Tout> map
                                        , int? itemsPerPage = null
                                        , int? currentPage = null
                                        , int? nearPagesMax = null
                                          )
        {
            content ??= Enumerable.Empty<Tin>();

            var pagination = Paginate(content.Count(), itemsPerPage, currentPage, nearPagesMax);

            var page = new Page<Tout> { Pagination = pagination };
            page.Content = content.Skip(pagination.PreviousResults)
                                    .Take(pagination.CurrentPageResults)
                                    .Select(x => map.Invoke(x))
                                    .ToList();
            return page;
        }

        public IPage<Tout> AsPage<Tin, Tout>(IEnumerable<Tin> content
                                , Func<Tin, Tout> map
                                , int totalResults
                                , int? itemsPerPage = null
                                , int? currentPage = null
                                , int? nearPagesMax = null
                                  )
        {
            content ??= Enumerable.Empty<Tin>();

            var pagination = Paginate(totalResults, itemsPerPage, currentPage, nearPagesMax);

            if (content.Count() > itemsPerPage) throw new ArgumentException("Current page content exceeds the page size.", nameof(content));
            if (content.Count() < itemsPerPage && currentPage < pagination.LastPage) throw new ArgumentException("Current page content is less than the page size.", nameof(content));

            var page = new Page<Tout> { Pagination = pagination };
            page.Content = content.Select(x => map.Invoke(x)).ToList();
            return page;
        }
        private static void EnsureNonNegative(ref int x) => x = Math.Max(x, 0);
        private static void EnsureNatural(ref int x) => x = Math.Max(x, 1);
        private static int CalculatePages(decimal count, decimal size) => (int)Math.Ceiling(count / size);
        private static void EnsureInRange(ref int x, int min, int max) => Math.Max(min, Math.Min(x, max));
        private static int? NullIfOutOfRange(int x, int min, int max) => x < min || max < x ? null : x;
        private static IEnumerable<int> CalculateNearPages(int currentPage, int lastPage, int otherPages)
        {
            bool showAll() => otherPages >= lastPage;
            var maxPagesBeforeCurrentPage = (int)Math.Floor((decimal)otherPages / 2);
            var maxPagesAfterCurrentPage = (int)Math.Ceiling((decimal)otherPages / 2) - 1;
            bool isNearFirstPage() => currentPage - maxPagesBeforeCurrentPage <= 0;
            bool isNearLastPage() => currentPage + maxPagesAfterCurrentPage >= lastPage;

            if (showAll()) return Enumerable.Range(1, lastPage);

            if (isNearFirstPage()) return Enumerable.Range(1, otherPages);

            if (isNearLastPage()) return Enumerable.Range(lastPage - otherPages + 1, otherPages);

            return Enumerable.Range(currentPage - maxPagesBeforeCurrentPage, otherPages);
        }


    }
}
