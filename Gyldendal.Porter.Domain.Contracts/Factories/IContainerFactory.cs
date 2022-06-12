using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;

namespace Gyldendal.Porter.Domain.Contracts.Factories
{
    public interface IContainerFactory
    {
        IMassTransitHandler GetMassTransitHandler(ContainerType containerType);
    }
}
