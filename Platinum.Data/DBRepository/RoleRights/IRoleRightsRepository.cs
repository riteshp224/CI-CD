using Platinum.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platinum.Data.DBRepository.RoleRights
{
    public interface IRoleRightsRepository
    {
        #region Get
        Task<List<RoleRightsMasterModel>> GetRoleRightsByRoleId(long roleId);
        Task<List<RoleRightsMasterModel>> GetRoleRightsByUserId(long userId);
        //Task<List<RoleRightsMasterModel>> GetMenuListByRoleId(RoleRightsMasterModel model);
        #endregion

        #region Post
        Task<string> SaveRoleRightsData(RoleRightMasterModel model);
        #endregion
    }
}
