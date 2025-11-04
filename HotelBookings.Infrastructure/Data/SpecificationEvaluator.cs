using HotelBookings.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBookings.Infrastructure.Data
{
    public class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            var Query = inputQuery.AsQueryable();

            if (spec.Criteria != null)
            {
                Query = Query.Where(spec.Criteria);
            }

            if (spec.OrderBy != null)
            {
                Query = Query.OrderBy(spec.OrderBy);
            }

            if (spec.OrderByDescending != null)
            {
                Query = Query.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.Skip.HasValue && spec.Take.HasValue && spec.Take.Value > 0)
            {
                Query = Query.Skip(spec.Skip.Value).Take(spec.Take.Value);
            }

            if (spec.Includes != null && spec.Includes.Count != 0)
            {
                Query = spec.Includes.Aggregate(Query, (current, include) => current.Include(include));
            }

            return Query;
        }
    }
}
