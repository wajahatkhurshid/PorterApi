using Gyldendal.Porter.Application.Contracts.Enums;

namespace Gyldendal.Porter.Application.Contracts.Models
{
    public class SubArea
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public WebShop WebShop { get; set; }
        public int SubjectId { get; set; }
    }
}

