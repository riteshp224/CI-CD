using Platinum.Data.DBRepository.Role;
using Platinum.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platinum.Service.Services.Role
{
    class RoleService : IRoleService
    {
        #region Fields
        private readonly IRoleRepository _repository;
        #endregion

        #region Construtor
        public RoleService(IRoleRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Get
        public async Task<List<RoleMasterModel>> GetRoleList()
        {
            return await _repository.GetRoleList();
        }
        public async Task<RoleMasterModel> GetRoleById(long roleId)
        {
            return await _repository.GetRoleById(roleId);
        }
        #endregion

        #region Post

        public async Task<string> SaveRoleData(RoleMasterModel model)
        {
            return await _repository.SaveRoleData(model);
        }
        #endregion

        #region Delete
        public async Task<bool> DeleteRole(CommonIdModel model)
        {
            return await _repository.DeleteRole(model);
        }
        #endregion
    }
}
