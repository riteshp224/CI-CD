using Platinum.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platinum.Data.DBRepository.Login
{
    public interface ILoginRepository
    {
        #region Post
        Task<LoginModel> LoginUser(LoginModel model);
        Task<long> ValidateUserEmail(string email);
        Task<string> ResetForgotPassword(UserForgotPasswordModel model);
        #endregion
    }
}
