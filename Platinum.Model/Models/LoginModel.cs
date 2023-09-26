using System;
using System.Collections.Generic;
using System.Text;

namespace Platinum.Model.Models
{   
    public class LoginModel
    {
        public long? userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public long? roleId { get; set; }
        public string userPhoto { get; set; }
        public long? organizationId { get; set; }
        public string logoUrl { get; set; }
        public string JWTToken { get; set; }
        public string error { get; set; }
    }

    public class UserForgotPasswordModel
    {
        public string encryptedUserId { get; set; }
        public long? userId { get; set; }
        public string password { get; set; }
        public long? LoggedInUserId { get; set; }
    }
}
