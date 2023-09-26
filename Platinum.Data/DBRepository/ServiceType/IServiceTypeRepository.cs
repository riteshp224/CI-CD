using Platinum.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platinum.Data.DBRepository.ServiceType
{
    public interface IServiceTypeRepository
    {
        #region Get
        Task<List<ServiceTypeMasterModel>> GetServiceTypeList(CommonPaginationModel model);
        Task<List<ServiceTypeMasterModel>> GetParentServiceTypeList();
        Task<ServiceTypeMasterModel> GetServiceTypeById(long ServiceTypeId);
        #endregion

        #region Post
        Task<string> SaveServiceTypeData(ServiceTypeMasterModel model);
        #endregion

        #region Delete
        Task<bool> DeleteServiceType(CommonIdModel model);
        #endregion

        
    }
}
