using System;

namespace Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers
{
    public class ContainerBase : ContainerInstanceBase
    {
        public Guid Id { get; set; }
    }
}
