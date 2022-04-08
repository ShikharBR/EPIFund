using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inview.Epi.EpiFund.Domain.Helpers
{
    public static class LinqHelper
    {
        public static IQueryable<T> AddSearchParameter<T>(this IQueryable<T> query, bool condition, System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            if (condition)
                query = query.Where(predicate);
            return query;
        }
    }
}
