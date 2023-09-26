using Platinum.Data.DBRepository.RoleRights;
using Platinum.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platinum.Service.Services.RoleRights
{
    class RoleRightsService : IRoleRightsService
    {
        #region Fields
        private readonly IRoleRightsRepository _repository;
        #endregion

        #region Construtor
        public RoleRightsService(IRoleRightsRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Get
        public async Task<List<RoleRightsMasterModel>> GetRoleRightsByRoleId(long roleId)
        {
            return await _repository.GetRoleRightsByRoleId(roleId);
        }
        public async Task<List<RoleRightsMasterModel>> GetRoleRightsByUserId(long userId)
        {
            return await _repository.GetRoleRightsByUserId(userId);
        }
        //public async Task<List<RoleRightsModel>> GetMenuListByRoleId(CommonModel model)
        //{
        //    return await _repository.GetMenuListByRoleId(model);
        //}
        #endregion

        #region Post
        public async Task<string> SaveRoleRightsData(RoleRightMasterModel model)
        {
            return await _repository.SaveRoleRightsData(model);
        }
        #endregion

    }
}
