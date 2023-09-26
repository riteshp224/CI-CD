using System;
using System.Collections.Generic;
using Platinum.Service.Services.Login;
using Platinum.Service.Services.Role;
using Platinum.Service.Services.RoleRights;
using Platinum.Service.Services.User;
using Platinum.Service.Services.ServiceType;
using Platinum.Service.Services.Enquiry;

namespace Platinum.Service
{
    public static class ServiceRegister
    {
        public static Dictionary<Type, Type> GetTypes()
        {
            var serviceDictonary = new Dictionary<Type, Type>
            {
                { typeof(IUserService), typeof(UserService) },
                { typeof(IRoleService), typeof(RoleService) },
                { typeof(IRoleRightsService), typeof(RoleRightsService) },
                { typeof(ILoginService), typeof(LoginService) },
                { typeof(IServiceTypeService), typeof(ServiceTypeService) },
                { typeof(IEnquiryService), typeof(EnquiryService) },
                { typeof(IEnquiryDetailService), typeof(EnquiryDetailService) }

            };
            return serviceDictonary;
        }
    }
}
