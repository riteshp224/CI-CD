using Platinum.Data.DBRepository.ServiceType;
using Platinum.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platinum.Service.Services.ServiceType
{
    public class ServiceTypeService : IServiceTypeService
    {
        #region Fields
        private readonly IServiceTypeRepository _repository;
        #endregion

        #region Construtor
        public ServiceTypeService(IServiceTypeRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Get
        public async Task<List<ServiceTypeMasterModel>> GetServiceTypeList(CommonPaginationModel model)
        {
            return await _repository.GetServiceTypeList(model);
        }
        public async Task<List<ServiceTypeMasterModel>> GetParentServiceTypeList()
        {
            return await _repository.GetParentServiceTypeList();
        }
        
        public async Task<ServiceTypeMasterModel> GetServiceTypeById(long ServiceTypeId)
        {
            return await _repository.GetServiceTypeById(ServiceTypeId);
        }
        #endregion

        #region Post

        public async Task<string> SaveServiceTypeData(ServiceTypeMasterModel model)
        {
            return await _repository.SaveServiceTypeData(model);
        }

        #endregion

        #region Delete
        public async Task<bool> DeleteServiceType(CommonIdModel model)
        {
            return await _repository.DeleteServiceType(model);
        }

    
        #endregion
    }
}
