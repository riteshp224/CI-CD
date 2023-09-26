using Platinum.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platinum.Service.Services.ServiceType
{
    public interface IServiceTypeService
    {
        #region GetCollection
        Task<List<ServiceTypeMasterModel>> GetServiceTypeList(CommonPaginationModel model);
        Task<List<ServiceTypeMasterModel>> GetParentServiceTypeList();
        Task<ServiceTypeMasterModel> GetServiceTypeById(long userId);
        #endregion

        #region Post
        Task<string> SaveServiceTypeData(ServiceTypeMasterModel model);
        //Task<string> SaveBoardingStep(UserMasterModel model);
        #endregion

        #region Delete
        Task<bool> DeleteServiceType(CommonIdModel model);
        #endregion
    }
}
