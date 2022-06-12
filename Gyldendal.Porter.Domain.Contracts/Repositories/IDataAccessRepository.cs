using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gyldendal.Porter.Domain.Contracts.Repositories
{
    public interface IDataAccessRepository<TEntity>
    {
        Task<bool> InsertAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
        Task<string> UpsertAsync(TEntity entity);
        Task<bool> DeleteAsync(TEntity entity);
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(string id);
        Task<long> SearchForCountAsync(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> SearchForAsync(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> GetByPaginationAsync(Expression<Func<TEntity, bool>> predicate, int pageSize, int pageIndex);
        Task<List<TEntity>> GetBySortingAndPaginationAsync(Expression<Func<TEntity, bool>> predicate, string sortBy,
            string orderBy, int pageSize = 100, int pageIndex = 1);
        Task<bool> DeleteAllAsync();
        Task<long> GetCount();
    }
}