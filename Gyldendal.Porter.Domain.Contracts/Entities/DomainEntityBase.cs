using System.ComponentModel.DataAnnotations;

namespace Gyldendal.Porter.Domain.Contracts.Entities
{
    public class DomainEntityBase
    {
        [Required]
        public string Id { get; set; }
    }
}
