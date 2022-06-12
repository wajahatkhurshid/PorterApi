using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;

namespace Gyldendal.Porter.Domain.Contracts
{
    public interface IMassTransitHandler
    {
        ContainerType ContainerType { get; }

        Task Handle(string payload);
    }
}
