using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Platinum.Model;
using Platinum.Model.Models;

namespace Platinum.Data.DBRepository.User
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        #region Fields
        private IConfiguration _config;
        private readonly DataConfig _dataConfig;
        #endregion

        #region Constructor
        public UserRepository(IConfiguration config, IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
            _config = config;
            _dataConfig = dataConfig.Value;
        }
        #endregion

        #region Get
        public async Task<List<UserMasterModel>> GetUserList(CommonPaginationModel model)
        {
            try
            {
                var param = new DynamicParameters();
            
                param.Add("@PageNumber", model.PageNumber);
                param.Add("@PageSize", model.PageSize);
                param.Add("@strSearch", model.StrSearch);
                param.Add("@Path", _dataConfig.FilePath + "UserProfile/");
                var data = await QueryAsync<UserMasterModel>("SP_UserMaster_GetList", param, commandType: CommandType.StoredProcedure);
                //var data = await QueryAsync<UserMasterModel>("SP_UserMaster_GetList", commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<UserMasterModel> GetUserById(long userId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@userId", userId);
                return await QueryFirstOrDefaultAsync<UserMasterModel>("SP_UserMaster_GetById", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region Post
        public async Task<string> SaveUserData(UserMasterModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@userId", model.userId);
                param.Add("@firstName", model.firstName);
                param.Add("@lastName", model.lastName);
                param.Add("@email", model.email);
                param.Add("@password", model.password);
                param.Add("@roleId", model.roleId);
                param.Add("@phone", model.phone);
                param.Add("@userPhoto", model.userPhoto);
                param.Add("@isActive", model.isActive);
                param.Add("@loggedInUserId", model.LoggedInUserId);
                return await QueryFirstOrDefaultAsync<string>("SP_UserMaster_AddUpdate", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public async Task<string> SaveBoardingStep(UserMasterModel model)
        //{
        //    try
        //    {
        //        var param = new DynamicParameters();
        //        param.Add("@user_id", model.my_user_id);
        //        param.Add("@is_onboarding_complete", model.is_onboarding_complete);
        //        param.Add("@step_number", model.last_completed_onboarding_step);
        //        return await QueryFirstOrDefaultAsync<string>("sp_user_master_update_step", param, commandType: CommandType.StoredProcedure);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion

        #region Delete
        public async Task<bool> DeleteUser(CommonIdModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@userId", model.id);
                var result = await QueryFirstOrDefaultAsync<string>("SP_UserMaster_Delete", param, commandType: CommandType.StoredProcedure);
                if (string.IsNullOrEmpty(result))
                {
                    return true;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return false;
        }
        #endregion
    }
}
