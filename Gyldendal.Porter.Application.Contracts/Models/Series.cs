using System;
using System.Collections.Generic;
using Gyldendal.Porter.Application.Contracts.Enums;

namespace Gyldendal.Porter.Application.Contracts.Models
{
    public class Series
    {
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public string Url { get; set; }
        
        //TODO: Missing in domain model
        public Series ParentSeries { get; set; }
        
        public int? ParentSerieId { get; set; }
        
        //TODO: Missing in domain model
        public List<Series> ChildSeries { get; set; }
        
        public List<Area> Areas { get; set; }
        
        public List<SubArea> SubAreas { get; set; }
        
        public List<EducationLevel> EducationLevels { get; set; }
        
        public List<Subject> Subjects { get; set; }
        
        public string ImageUrl { get; set; }
        
        public DateTime UpdatedTimestamp { get; set; }
        
        public bool IsSystemSeries { get; set; }
        
        public WebShop WebShop { get; set; }
    }
}
