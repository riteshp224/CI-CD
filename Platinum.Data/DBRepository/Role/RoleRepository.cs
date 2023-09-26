using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Platinum.Model;
using Platinum.Model.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platinum.Data.DBRepository.Role
{
    public class RoleRepository : BaseRepository, IRoleRepository
    {
        #region Fields
        private IConfiguration _config;
        #endregion

        #region Constructor
        public RoleRepository(IConfiguration config, IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
            _config = config;
        }
        #endregion

        #region Get
        public async Task<List<RoleMasterModel>> GetRoleList()
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Type", 1);
                var data = await QueryAsync<RoleMasterModel>("SP_RoleMaster",param, commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<RoleMasterModel> GetRoleById(long roleId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@roleId", roleId);
                param.Add("@Type", 2);
                return await QueryFirstOrDefaultAsync<RoleMasterModel>("SP_RoleMaster", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Post
        public async Task<string> SaveRoleData(RoleMasterModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@roleId", model.roleId);
                param.Add("@roleName", model.roleName);
                param.Add("@isActive", model.isActive);
                param.Add("@userId", model.LoggedInUserId);
                if (model.roleId != 0)
                    param.Add("@Type", 4);
                else
                    param.Add("@Type", 3);
                return await QueryFirstOrDefaultAsync<string>("SP_RoleMaster", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Delete
        public async Task<bool> DeleteRole(CommonIdModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@roleId", model.id);
                param.Add("@Type", 5);
                var result = await QueryFirstOrDefaultAsync<string>("SP_RoleMaster", param, commandType: CommandType.StoredProcedure);
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
