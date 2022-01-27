using System;
using System.Collections.Generic;

namespace Qualia.Pagination
{
    public static class PageExtension
    {
        public static IPagination Paginate(
            this int items
            , int? page = null
            , int? size = null
            , int? nearPages = null
            )
        {
            var paginator = new Paginator();
            return paginator.Paginate(items, size, page, nearPages);
        }

        public static IPage<T> Paginate<T>(
            this IEnumerable<T> items
            , int? page = null
            , int? size = null
            , int? nearPages = null
            )
        {
            var paginator = new Paginator();
            return paginator.Paginate(items, x => x, size, page, nearPages);
        }

        public static IPage<Tout> Paginate<Tin, Tout>(
            this IEnumerable<Tin> items
            , Func<Tin, Tout> map
            , int? page = null
            , int? size = null
            , int? nearPages = null
            )
        {
            var paginator = new Paginator();
            return paginator.Paginate(items, map, size, page, nearPages);
        }

        public static IPage<T> AsPage<T>(
            this IEnumerable<T> items
            , int totalResults
            , int? page = null
            , int? size = null
            , int? nearPages = null
            )
        {
            var paginator = new Paginator();
            return paginator.AsPage(items, x => x, totalResults, size, page, nearPages);
        }

        public static IPage<Tout> AsPage<Tin, Tout>(
            this IEnumerable<Tin> items
            , Func<Tin, Tout> map
            , int totalResults
            , int? page = null
            , int? size = null
            , int? nearPages = null
            )
        {
            var paginator = new Paginator();
            return paginator.AsPage(items, map, totalResults, size, page, nearPages);
        }
    }

}
