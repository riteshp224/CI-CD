using Platinum.Data.DBRepository.User;
using Platinum.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platinum.Service.Services.User
{
    public class UserService : IUserService
    {
        #region Fields
        private readonly IUserRepository _repository;
        #endregion

        #region Construtor
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Get
        public async Task<List<UserMasterModel>> GetUserList(CommonPaginationModel model)
        {
            return await _repository.GetUserList(model);
        }
        public async Task<UserMasterModel> GetUserById(long userId)
        {
            return await _repository.GetUserById(userId);
        }
        #endregion

        #region Post

        public async Task<string> SaveUserData(UserMasterModel model)
        {
            return await _repository.SaveUserData(model);
        }

        //public async Task<string> SaveBoardingStep(UserMasterModel model)
        //{
        //    return await _repository.SaveBoardingStep(model);
        //}
        #endregion

        #region Delete
        public async Task<bool> DeleteUser(CommonIdModel model)
        {
            return await _repository.DeleteUser(model);
        }
        #endregion
    }
}
