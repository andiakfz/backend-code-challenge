using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
