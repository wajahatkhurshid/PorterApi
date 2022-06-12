using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Entities;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using MongoDB.Driver;

namespace Gyldendal.Porter.Infrastructure.Repository
{
    public class BaseRepository<TEntity> : IDataAccessRepository<TEntity> where TEntity : DomainEntityBase
    {
        public readonly IMongoCollection<TEntity> Collection;

        protected BaseRepository(PorterContext context)
        {
            var database = context.Db;
            Collection = database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public async Task<bool> InsertAsync(TEntity entity)
        {
            if (entity == null)
                return false;

            await Collection.InsertOneAsync(entity);

            return true;
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            if (entity == null)
                return false;

            var filter = Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id);
            var response = await Collection.ReplaceOneAsync(filter, entity);
            return response.IsAcknowledged;
        }

        public async Task<bool> BulkOperationAsync(List<WriteModel<TEntity>> entities, IClientSessionHandle session)
        {
            if (entities == null)
                return false;

            var response = await Collection.BulkWriteAsync(session, entities);
            return response.IsAcknowledged;
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            if (entity == null)
                return false;

            var filter = Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id);
            var response = await Collection.DeleteOneAsync(filter);

            return response.IsAcknowledged;
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await Collection.FindAsync(_ => true).Result.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            return await Collection.FindAsync(x => x.Id == id).Result.FirstOrDefaultAsync();
        }

        public async Task<long> SearchForCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Collection.CountDocumentsAsync(predicate);
        }

        public async Task<List<TEntity>> SearchForAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Collection.FindAsync(predicate).Result.ToListAsync();
        }

        public async Task<string> UpsertAsync(TEntity entity)
        {
            if (entity == null)
                return null;

            var existingEntity = await Collection.Find(x => x.Id == entity.Id).FirstOrDefaultAsync();
            if (existingEntity == null)
            {
                entity.Id ??= Guid.NewGuid().ToString();
                await Collection.InsertOneAsync(entity);
                return entity.Id;
            }

            entity.Id = existingEntity.Id;
            var filter = Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id);

            await Collection.ReplaceOneAsync(filter, entity);
            return entity.Id;
        }

        public async Task<List<TEntity>> GetByPaginationAsync(Expression<Func<TEntity, bool>> predicate,
            int pageSize = 100, int pageIndex = 1)
        {
            FindOptions<TEntity> findOptions = new FindOptions<TEntity>
            {
                Skip = (pageIndex - 1) * pageSize,
                Limit = pageSize
            };

            return await Collection.FindAsync(predicate, findOptions).Result.ToListAsync();
        }

        public async Task<List<TEntity>> GetBySortingAndPaginationAsync(Expression<Func<TEntity, bool>> predicate,
            string sortBy, string orderBy, int pageSize = 100, int pageIndex = 1)
        {
            FindOptions<TEntity> findOptions = new FindOptions<TEntity>
            {
                Sort = sortBy == "asc"
                    ? Builders<TEntity>.Sort.Ascending(orderBy)
                    : Builders<TEntity>.Sort.Descending(orderBy),
                Skip = (pageIndex - 1) * pageSize,
                Limit = pageSize
            };

            return await Collection.FindAsync(predicate, findOptions).Result.ToListAsync();
        }

        public async Task<bool> DeleteAllAsync()
        {
            var response = await Collection
                .DeleteManyAsync(Builders<TEntity>.Filter.Empty);
            return response.IsAcknowledged;
        }

        public virtual async Task<long> GetCount()
        {
            return await Collection.CountDocumentsAsync(FilterDefinition<TEntity>.Empty);
        }
    }
}