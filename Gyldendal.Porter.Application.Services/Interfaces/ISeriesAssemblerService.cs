using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Entities.Cooked;
using Gyldendal.Porter.Domain.Contracts.Entities.MasterData;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;

namespace Gyldendal.Porter.Application.Services.Interfaces
{
    public interface ISeriesAssemblerService
    {
        Task<CookedSeries> Assemble(ProductCollectionTitleContainer serie, List<EducationLevel> educationLevels, DateTime updatedTimeStamp);
    }
}
