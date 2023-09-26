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

namespace Platinum.Data.DBRepository.RoleRights
{
    public class RoleRightsRepository : BaseRepository, IRoleRightsRepository
    {
        #region Fields
        private IConfiguration _config;
        #endregion

        #region Constructor
        public RoleRightsRepository(IConfiguration config, IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
            _config = config;
        }
        #endregion

        #region Get
        public async Task<List<RoleRightsMasterModel>> GetRoleRightsByRoleId(long roleId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@roleId", roleId);
                var data = await QueryAsync<RoleRightsMasterModel>("SP_RoleRights_GetByRoleId", param, commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<RoleRightsMasterModel>> GetRoleRightsByUserId(long userId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@userId", userId);
                var data = await QueryAsync<RoleRightsMasterModel>("SP_RoleRights_GetByUserId", param, commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public async Task<List<RoleRightsMasterModel>> GetMenuListByRoleId(RoleRightsMasterModel model)
        //{
        //    try
        //    {
        //        var param = new DynamicParameters();
        //        param.Add("@roleId", model.roleId);
        //        var data = await QueryAsync<RoleRightsMasterModel>("sp_role_rights_getbyroleid", param, commandType: CommandType.StoredProcedure);
        //        return data.ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion

        #region Post
        public async Task<string> SaveRoleRightsData(RoleRightMasterModel model)
        {
            try
            {
                DataTable dtRoleRights = new DataTable("tbl_RoleRights");
                dtRoleRights.Columns.Add("roleId");
                dtRoleRights.Columns.Add("menuId");
                dtRoleRights.Columns.Add("userId");
                dtRoleRights.Columns.Add("isAdd");
                dtRoleRights.Columns.Add("isEdit");
                dtRoleRights.Columns.Add("isDelete");
                dtRoleRights.Columns.Add("isView");

                if (model.RoleRightsMasterModel.Count > 0)
                {
                    foreach (var item in model.RoleRightsMasterModel)
                    {
                        DataRow dtRow = dtRoleRights.NewRow();
                        dtRow["roleId"] = model.roleId;
                        dtRow["menuId"] = item.menuId;
                        dtRow["userId"] = item.userId;
                        dtRow["isAdd"] = item.isAdd;
                        dtRow["isEdit"] = item.isEdit;
                        dtRow["isDelete"] = item.isDelete;
                        dtRow["isView"] = item.isView;
                        dtRoleRights.Rows.Add(dtRow);
                    }
                }

                var param = new DynamicParameters();
                param.Add("@roleRights", dtRoleRights.AsTableValuedParameter("[dbo].[tbl_RoleRights]"));
                param.Add("@UserId", model.userId);
                return await QueryFirstOrDefaultAsync<string>("SP_RoleRights_Add", param, commandType: CommandType.StoredProcedure);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
