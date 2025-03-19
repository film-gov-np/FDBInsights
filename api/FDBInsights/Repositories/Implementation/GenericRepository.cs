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
                .Select(x => (TResult)(object)x)
                .FirstOrDefaultAsync(cancellationToken)
            : _dbContext.Set<T>()
                .AsNoTracking()
                .Where(expression)
                .Select(selector)
                .FirstOrDefaultAsync(cancellationToken));
    }

    public async Task<List<TResult>> GetAllAsync<TResult>(
        Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, TResult>>? selector = null,
        int? pageNumber = null,
        int? pageSize = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<T>().AsNoTracking();

        if (predicate != null)
            query = query.Where(predicate);

        var resultQuery = selector != null
            ? query.Select(selector)
            : query.Select(x => (TResult)(object)x);

        if (pageNumber.HasValue && pageSize.HasValue)
            resultQuery = resultQuery
                .Skip((pageNumber.Value - 1) * pageSize.Value)
                .Take(pageSize.Value);

        return await resultQuery.ToListAsync(cancellationToken);
    }
}