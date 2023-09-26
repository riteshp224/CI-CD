using Platinum.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platinum.Service.Services.RoleRights
{
    public interface IRoleRightsService
    {
        #region Get
        Task<List<RoleRightsMasterModel>> GetRoleRightsByRoleId(long roleId);
        Task<List<RoleRightsMasterModel>> GetRoleRightsByUserId(long userId);
        //Task<List<RoleRightsModel>> GetMenuListByRoleId(CommonModel model);
        #endregion

        #region Post
        Task<string> SaveRoleRightsData(RoleRightMasterModel model);
        #endregion
    }
}
