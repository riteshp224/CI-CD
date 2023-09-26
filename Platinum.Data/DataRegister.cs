using System;
using System.Collections.Generic;
using Platinum.Data.DBRepository.Login;
using Platinum.Data.DBRepository.Role;
using Platinum.Data.DBRepository.RoleRights;
using Platinum.Data.DBRepository.User;
using Platinum.Data.DBRepository.ServiceType;
using Platinum.Data.DBRepository.Enquiry;

namespace Platinum.Data
{
    public static class DataRegister
    {
        public static Dictionary<Type, Type> GetTypes()
        {
            var dataDictionary = new Dictionary<Type, Type>
            {
                { typeof(IUserRepository), typeof(UserRepository) },
                { typeof(IRoleRepository), typeof(RoleRepository) },
                { typeof(IRoleRightsRepository), typeof(RoleRightsRepository) },
                { typeof(ILoginRepository), typeof(LoginRepository) },
                { typeof(IServiceTypeRepository), typeof(ServiceTypeRepository) },
                { typeof(IEnquiryRepository), typeof(EnquiryRepository) },
                { typeof(IEnquiryDetailRepository), typeof(EnquiryDetailRepository) },

            };
            return dataDictionary;
        }
    }
}
