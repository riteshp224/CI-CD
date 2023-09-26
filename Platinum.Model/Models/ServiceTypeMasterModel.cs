using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platinum.Model.Models
{
    public class ServiceTypeMasterModel:CommonModel
    {
        public long ServiceTypeId { get; set; }
        public long? ParentServiceTypeId { get; set; }
        public string ParentServiceName { get; set; }
        public string ServiceName { get; set; }
        public decimal? Cost { get; set; }
        public Int32? Sortno { get; set; }
        public string ServiceTypePhoto { get; set; }
        public string ImageURL { get; set; }
        public IFormFile ServiceTypePhotoFile { get; set; }
        public string parentSortNo { get; set; }
        public string ChildSortNo { get; set; }
    }
}
