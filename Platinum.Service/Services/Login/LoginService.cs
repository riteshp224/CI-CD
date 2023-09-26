using Platinum.Data.DBRepository.Login;
using Platinum.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platinum.Service.Services.Login
{
    public class LoginService : ILoginService
    {
        #region Fields
        private readonly ILoginRepository _repository;
        #endregion
        #region Construtor
        public LoginService(ILoginRepository repository)
        {
            _repository = repository;
        }
        #endregion
        public async Task<LoginModel> LoginUser(LoginModel model)
        {
            return await _repository.LoginUser(model);
        }
        public async Task<long> ValidateUserEmail(string email)
        {
            return await _repository.ValidateUserEmail(email);
        }
        public async Task<string> ResetForgotPassword(UserForgotPasswordModel model)
        {
            return await _repository.ResetForgotPassword(model);
        }
    }
}
