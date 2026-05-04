using Domain.Models.Filters;
using System.Linq.Dynamic.Core;

namespace Infraestructure.Extensions
{
    public static class PaginationExtension
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, SortedFiltersModel paginationOptions)
        {
            if (paginationOptions == null) return query;
            if (HasValidOrder(paginationOptions.Order)) query = query.OrderBy(paginationOptions.Order);
            if (paginationOptions.Page == 0 && paginationOptions.PageSize == 0) return query;
            if (paginationOptions.Page <= 0) paginationOptions.Page = 1;
            if (paginationOptions.PageSize <= 0) paginationOptions.PageSize = 10;

            var skipQuantity = (paginationOptions.Page - 1) * paginationOptions.PageSize;
            query = query.Skip(skipQuantity).Take(paginationOptions.PageSize);

            return query;
        }

        private static bool HasValidOrder(string order)
        {
            return !string.IsNullOrEmpty(order) && !string.IsNullOrWhiteSpace(order);
        }
    }
}
