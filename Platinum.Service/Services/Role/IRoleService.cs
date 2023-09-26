using Platinum.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platinum.Service.Services.Role
{
    public interface IRoleService
    {
        #region Get
        Task<List<RoleMasterModel>> GetRoleList();
        Task<RoleMasterModel> GetRoleById(long roleId);
        #endregion

        #region Post
        Task<string> SaveRoleData(RoleMasterModel model);
        #endregion

        #region Delete
        Task<bool> DeleteRole(CommonIdModel model);
        #endregion
    }
}
