using System.Threading.Tasks;

namespace Gyldendal.Porter.Application.Services.Interfaces
{
    public interface IContainerService
    {
        Task AddContainer(string containerId, string payload);
    }
}
