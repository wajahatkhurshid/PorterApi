using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Entities.MasterData;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace Gyldendal.Porter.Infrastructure.Repository
{
    public class MediaMaterialTypeRepository : BaseRepository<MediaMaterialType>, IMediaMaterialTypeRepository
    {
        private readonly IMemoryCache _cache;

        public MediaMaterialTypeRepository(PorterContext context, IMemoryCache cache) : base(context)
        {
            _cache = cache;
        }

        public async Task<string> UpsertMediaMaterialTypeAsync(MediaMaterialType mediaMaterialType)
        {
            return await UpsertAsync(mediaMaterialType);
        }

        public async Task<List<MediaMaterialType>> GetMediaMaterialTypesAsync()
        {
            return await GetAllAsync();
        }

        public async Task<MediaMaterialType> GetMediaTypeByIdAsync(string id)
        {
            var cacheKey = $"media-type-{id}";
            if (_cache.TryGetValue<MediaMaterialType>(cacheKey, out var cachedValue)) return cachedValue;

            var response = await SearchForAsync(x => x.Id.Equals(id) && x.Level == 0);
            var mediaType = response.FirstOrDefault();

            if (mediaType != null)
                _cache.Set(cacheKey, mediaType, TimeSpan.FromHours(3));

            return mediaType;

        }

        public async Task<MediaMaterialType> GetMaterialTypeByIdAsync(string id)
        {
            var cacheKey = $"material-type-{id}";
            if (_cache.TryGetValue<MediaMaterialType>(cacheKey, out var cachedValue)) return cachedValue;

            var response = await SearchForAsync(x => x.Id.Equals(id) && x.Level == 1);
            var materialType = response.FirstOrDefault();

            if (materialType != null)
                _cache.Set(cacheKey, materialType, TimeSpan.FromHours(3));

            return materialType;

        }
    }
}