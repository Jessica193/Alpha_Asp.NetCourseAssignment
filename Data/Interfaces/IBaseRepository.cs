using DataLibrary.Models;
using System.Linq.Expressions;

namespace DataLibrary.Interfaces
{
    public interface IBaseRepository<TEntity, TModel> where TEntity : class
    {
        Task<RepositoryResult<bool>> CreateAsync(TEntity entity);
        Task<RepositoryResult<bool>> UpdateAsync(TEntity updatedEntity);
        Task<RepositoryResult<bool>> DeleteAsync(TEntity entity);
        Task<RepositoryResult<bool>> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
        Task<RepositoryResult<IEnumerable<TModel>>> GetAllAsync(bool orderByDescendning = false, Expression<Func<TEntity, object>>? sortBy = null, Expression<Func<TEntity, bool>>? where = null, params Expression<Func<TEntity, object>>[] includes);
        Task<RepositoryResult<IEnumerable<TSelect>>> GetAllAsync<TSelect>(Expression<Func<TEntity, TSelect>> selector, bool orderByDescendning = false, Expression<Func<TEntity, object>>? sortBy = null, Expression<Func<TEntity, bool>>? where = null, params Expression<Func<TEntity, object>>[] includes);
        Task<RepositoryResult<TModel>> GetOneAsync(Expression<Func<TEntity, bool>>? where = null, params Expression<Func<TEntity, object>>[] includes);
    }
}