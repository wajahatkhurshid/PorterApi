using System.Collections.Generic;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Entities.MasterData;


namespace Gyldendal.Porter.Domain.Contracts.Repositories
{
    public interface IMediaMaterialTypeRepository
    {
        Task<string> UpsertMediaMaterialTypeAsync(MediaMaterialType mediaMaterialType);

        Task<List<MediaMaterialType>> GetMediaMaterialTypesAsync();

        Task<MediaMaterialType> GetMediaTypeByIdAsync(string id);

        Task<MediaMaterialType> GetMaterialTypeByIdAsync(string id);
    }
}
