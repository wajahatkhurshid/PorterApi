using System.Collections.Generic;
using Gyldendal.Porter.Application.Contracts.Enums;

namespace Gyldendal.Porter.Application.Contracts.Models
{
    public class Work
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public WebShop WebShop { get; set; }
        public List<Product> Products { get; set; }
        public List<SubjectCode> SubjectCodes { get; set; }
        public List<Subject> Subjects { get; set; }
        public List<Area> Areas { get; set; }
        public List<Level> Levels { get; set; }
        public List<SubArea> SubAreas { get; set; }
    }
}
