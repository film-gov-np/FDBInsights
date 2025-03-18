using System.Linq.Expressions;

namespace FDBInsights.Repositories;

public interface IGenericRepository<T> where T : class
{
    Task<TResult?> GetByPropertyAsync<TResult>(
        Expression<Func<T, bool>> expression,
        Expression<Func<T, TResult>>? selector = null,
        CancellationToken cancellationToken = default);

    Task<List<TResult>> GetAllAsync<TResult>(Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, TResult>>? selector = null,
        string? filterPropertyName = null,
        string? filterKeyword = null,
        string? orderByPropertyName = null,
        bool ascending = true,
        int? pageNumber = null,
        int? pageSize = null,
        CancellationToken cancellationToken = default);

    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity, CancellationToken cancellationToken = default);
    void Remove(T entity, CancellationToken cancellationToken = default);
}