using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;
using ContainerBase = Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;

namespace Gyldendal.Porter.Domain.Contracts.Parser
{
    public interface IContainerParser
    {
        ContainerType ContainerType { get; }

        ContainerBase.ContainerBase Parse(string payload);
    }
}
