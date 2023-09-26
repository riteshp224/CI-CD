using Platinum.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platinum.Data.DBRepository.User
{
    public interface IUserRepository
    {
        #region Get
        Task<List<UserMasterModel>> GetUserList(CommonPaginationModel model);
        Task<UserMasterModel> GetUserById(long userId);
        #endregion

        #region Post
        Task<string> SaveUserData(UserMasterModel model);
        //Task<string> SaveBoardingStep(UserMasterModel model);
        #endregion

        #region Delete
        Task<bool> DeleteUser(CommonIdModel model);
        #endregion

        
    }
}
