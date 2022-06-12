using System;
using Gyldendal.Porter.Application.Contracts.Enums;

namespace Gyldendal.Porter.Application.Contracts.Models
{
    public class ProductUpdateInfo
    {
        public string Id { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool IsDeleted { get; set; }
        public ProductType ProductType { get; set; }
    }
}
