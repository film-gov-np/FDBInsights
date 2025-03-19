using System.Linq.Expressions;
using FDBInsights.Data;
using Microsoft.EntityFrameworkCore;

namespace FDBInsights.Repositories.Implementation;

public class GenericRepository<T>(ApplicationDbContext dbContext) : IGenericRepository<T>
    where T : class
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Update(T entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Remove(T entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<TResult?> GetByPropertyAsync<TResult>(
        Expression<Func<T, bool>> expression,
        Expression<Func<T, TResult>>? selector = null,
        CancellationToken cancellationToken = default)
    {
        return await (selector == null
            ? _dbContext.Set<T>()
                .AsNoTracking()
                .Where(expression)
                .TagWith(typeof(T).Name)
                .Select(x => (TResult)(object)x)
                .FirstOrDefaultAsync(cancellationToken)
            : _dbContext.Set<T>()
                .AsNoTracking()
                .Where(expression)
                .TagWith(typeof(T).Name)
                .Select(selector)
                .FirstOrDefaultAsync(cancellationToken));
    }

    public async Task<List<TResult>> GetAllAsync<TResult>(Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, TResult>>? selector = null,
        string? filterPropertyName = null,
        string? filterKeyword = null,
        string? orderByPropertyName = null,
        bool ascending = true,
        int? pageNumber = null,
        int? pageSize = null,
        CancellationToken cancellationToken = default)
    {
        // Start the query from the DbContext set
        var query = _dbContext.Set<T>().AsNoTracking();
        // Apply the additional filter predicate if provided
        if (predicate != null)
            query = query.Where(predicate)
                .TagWith(typeof(T).Name);
        // Apply dynamic filter (LIKE functionality) if filterValue is provided
        if (!string.IsNullOrEmpty(filterKeyword) && !string.IsNullOrEmpty(filterPropertyName))
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, filterPropertyName);

            if (property.Type == typeof(string))
            {
                var containsMethod = typeof(string).GetMethod("Contains", [typeof(string)]);
                var filterValueExpression = Expression.Constant(filterKeyword);
                if (containsMethod != null)
                {
                    var containsExpression = Expression.Call(property, containsMethod, filterValueExpression);
                    var lambda = Expression.Lambda<Func<T, bool>>(containsExpression, parameter);
                    query = query.Where(lambda);
                }
            }
            else
            {
                throw new InvalidOperationException("Filter can only be applied to string properties.");
            }
        }

        // Dynamic sorting (if propertyName is provided)
        if (!string.IsNullOrEmpty(orderByPropertyName))
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, orderByPropertyName);
            var lambda = Expression.Lambda(property, parameter);

            var orderByMethod = ascending
                ? "OrderBy"
                : "OrderByDescending";

            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == orderByMethod && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.Type);

            query = (IQueryable<T>)method.Invoke(null, [query, lambda])!;
        }

        // Apply the projection if the selector is provided
        var resultQuery = selector != null
            ? query.Select(selector)
            : query.Select(x => (TResult)(object)x);

        // Handle pagination if pageNumber and pageSize are provided
        if (pageNumber.HasValue && pageSize.HasValue)
            resultQuery = resultQuery
                .Skip((pageNumber.Value - 1) * pageSize.Value)
                .Take(pageSize.Value);

        // Execute the query and return the result as a list
        return await resultQuery.ToListAsync(cancellationToken);
    }
}