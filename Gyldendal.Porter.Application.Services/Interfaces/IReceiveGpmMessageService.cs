using System.Threading;
using System.Threading.Tasks;

namespace Gyldendal.Porter.Application.Services.Interfaces
{
    public interface IReceiveGpmMessageService
    {
        Task Receive(CancellationToken cancellationToken);
    }
}
