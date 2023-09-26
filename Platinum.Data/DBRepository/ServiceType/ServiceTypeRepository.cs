using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Platinum.Model;
using Platinum.Model.Models;

namespace Platinum.Data.DBRepository.ServiceType
{
    public class ServiceTypeRepository : BaseRepository, IServiceTypeRepository
    {
        #region Fields
        private IConfiguration _config;
        private readonly DataConfig _dataConfig;
        #endregion

        #region Constructor
        public ServiceTypeRepository(IConfiguration config, IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
            _config = config;
            _dataConfig = dataConfig.Value;
        }
        #endregion

        #region Get
        public async Task<List<ServiceTypeMasterModel>> GetServiceTypeList(CommonPaginationModel model)
        {
            try
            {
                var param = new DynamicParameters();
            
                param.Add("@PageNumber", model.PageNumber);
                param.Add("@PageSize", model.PageSize);
                param.Add("@strSearch", model.StrSearch);
                //param.Add("@orderBy", model.SortColumn);
                //param.Add("@sortOrder", model.SortOrder);
                param.Add("@Type", 1);
                param.Add("@Path", _dataConfig.FilePath + "ServiceTypes/");
                var data = await QueryAsync<ServiceTypeMasterModel>("SP_ServiceTypeMaster_v1", param, commandType: CommandType.StoredProcedure);
                //var data = await QueryAsync<ServiceTypeMasterModel>("SP_ServiceTypeMaster_GetList", commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ServiceTypeMasterModel>> GetParentServiceTypeList()
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Type", 6);
                var data = await QueryAsync<ServiceTypeMasterModel>("SP_ServiceTypeMaster_v1", param, commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ServiceTypeMasterModel> GetServiceTypeById(long ServiceTypeId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ServiceTypeId", ServiceTypeId);
                param.Add("@Type", 2);
                return await QueryFirstOrDefaultAsync<ServiceTypeMasterModel>("SP_ServiceTypeMaster_v1", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region Post
        public async Task<string> SaveServiceTypeData(ServiceTypeMasterModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ServiceTypeId", model.ServiceTypeId);
                param.Add("@ParentServiceTypeId", model.ParentServiceTypeId);
                param.Add("@ServiceName", model.ServiceName);
                param.Add("@Cost", model.Cost);
                param.Add("@Sortno", model.Sortno);
                param.Add("@ItemPhoto", model.ServiceTypePhoto);
                param.Add("@isActive", model.isActive);
                param.Add("@userId", model.LoggedInUserId);
                if (model.ServiceTypeId != 0)
                    param.Add("@Type", 4);
                else
                    param.Add("@Type", 3);
                return await QueryFirstOrDefaultAsync<string>("SP_ServiceTypeMaster_v1", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Delete
        public async Task<bool> DeleteServiceType(CommonIdModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ServiceTypeId", model.id);
                param.Add("@Type", 5);
                var result = await QueryFirstOrDefaultAsync<string>("SP_ServiceTypeMaster_v1", param, commandType: CommandType.StoredProcedure);
                if (string.IsNullOrEmpty(result))
                {
                    return true;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return false;
        }
        #endregion
    }
}
