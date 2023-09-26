using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platinum.Model.Models
{
    public class UserMasterModel : CommonModel
    {
        public long userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public long roleId { get; set; }
        public string phone { get; set; }
        public string userPhoto { get; set; }        
        public string imageURL { get; set; }        
        public IFormFile userPhotoFile { get; set; }
    }
}
