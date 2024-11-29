using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace portlocator.Shared.Extensions
{
    public static class QueryExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> source, int page, int limit)
        {
            var skip = (page - 1) * limit;
            skip = skip < 0 ? 0 : skip;

            return source.Skip(skip).Take(limit);
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string? property, bool descending)
        {
            if (string.IsNullOrEmpty(property))
            {
                return source;
            }

            string clause = $"{property} {(descending ? "DESC" : "ASC")}";
            return source.OrderBy(clause);
        }
    }
}
