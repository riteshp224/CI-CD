using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Platinum.Model;
using Platinum.Model.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Platinum.Data.DBRepository.Login
{
    public class LoginRepository : BaseRepository, ILoginRepository
    {
        #region Fields
        private IConfiguration _config;
        #endregion

        #region Constructor
        public LoginRepository(IConfiguration config, IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
            _config = config;
        }
        #endregion

        #region Post
        public async Task<LoginModel> LoginUser(LoginModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@email", model.email);
                param.Add("@password", model.password);
                return await QueryFirstOrDefaultAsync<LoginModel>("SP_UserMaster_Login", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<long> ValidateUserEmail(string email)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@email", email);
                return await QueryFirstOrDefaultAsync<long>("SP_UserMaster_ValidateEmail", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> ResetForgotPassword(UserForgotPasswordModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@userId", model.userId);
                param.Add("@password", model.password);
                return await QueryFirstOrDefaultAsync<string>("SP_UserMaster_UpdatePassword", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
