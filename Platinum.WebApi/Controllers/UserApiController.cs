using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Platinum.Common;
using Platinum.Model;
using Platinum.Model.Models;
using Platinum.Service.Services.User;
using Platinum.WebApi.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static Platinum.Common.EmailNotification;
using static Platinum.Common.EncryptionDecryption;
using System.Web;

namespace Platinum.WebApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserApiController : ControllerBase
    {
        #region Fields
        private readonly ILoggerManager _logger;
        private IUserService _userService;
        private IConfiguration _config;
        private readonly CommonMessages _commonMessages;
        private readonly DataConfig _dataConfig;
        private readonly ApplicationSettings _appSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Constructor
        public UserApiController(
            ILoggerManager logger,
            IUserService userService,
            IConfiguration config,
            IOptions<CommonMessages> commonMessages,
            IOptions<DataConfig> dataConfig,
            IOptions<ApplicationSettings> appSettings,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _logger = logger;
            _userService = userService;
            _config = config;
            _commonMessages = commonMessages.Value;
            _dataConfig = dataConfig.Value;
            _appSettings = appSettings.Value;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Get
        [HttpPost("GetUserList")]
        public async Task<ApiResponse<UserMasterModel>> GetUserList(CommonPaginationModel model)
        {
            ApiResponse<UserMasterModel> response = new ApiResponse<UserMasterModel>() { Data = new List<UserMasterModel>() };
            try
            {
                var data = await _userService.GetUserList(model);
                response.Data = data;
                response.Success = true;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetUserList", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = _commonMessages.Error;
            }
            return response;
        }

        [HttpGet("GetUserById/{userId}")]
        public async Task<ApiPostResponse<UserMasterModel>> GetUserById(long userId)
        {
            ApiPostResponse<UserMasterModel> response = new ApiPostResponse<UserMasterModel>() { Data = new UserMasterModel() };
            try
            {
                var data = await _userService.GetUserById(userId);
                response.Data = data;
                if (response.Data != null)
                {
                    response.Data.imageURL = !string.IsNullOrEmpty(data.userPhoto) ? _dataConfig.FilePath + "UserProfile/" + data.userPhoto : null;
                    response.Data.password = GetDecrypt(data.password);
                }                
                response.Success = true;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetUserById", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = _commonMessages.Error;
            }
            return response;
        }
        #endregion

        #region Post
        [HttpPost("SaveUser")]
        [Consumes("multipart/form-data")]
        public async Task<BaseApiResponse> SaveUser([FromForm] UserMasterModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            try
            {
                if (model.userPhotoFile != null)
                {
                    Guid guidFile = Guid.NewGuid();
                    var FileName = guidFile + Path.GetExtension(model.userPhotoFile.FileName);
                    var BasePath = Path.Combine(Directory.GetCurrentDirectory(), "Documents/UserProfile");

                    if (!Directory.Exists(BasePath))
                    {
                        Directory.CreateDirectory(BasePath);
                    }
                    var path = Path.Combine(BasePath, FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.userPhotoFile.CopyToAsync(stream);
                    }
                    model.userPhoto = FileName;
                }
                if (model.password != null)
                {
                    model.password = EncryptionDecryption.GetEncrypt(model.password);
                }
                var result = await _userService.SaveUserData(model);
                if (string.IsNullOrEmpty(result))
                {
                    response.Message = _commonMessages.User.SaveSuccess;
                    response.Success = true;
                }
                else
                {
                    response.Message = _commonMessages.User.SaveError;
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("SaveUser", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = _commonMessages.Error;
            }
            return response;
        }
        //[HttpPost("SendInvitation")]
        //public async Task<BaseApiResponse> SendInvitation([FromForm] UserMasterModel model)
        //{
        //    BaseApiResponse response = new BaseApiResponse();
        //    try
        //    {
        //        var result = await _userService.SaveUserData(model);
        //        var UserId = HttpUtility.UrlEncode(GetEncrypt(result.ToString()));
        //        if (result == "User with same email address is already exists.")
        //        {
        //            response.Success = false;
        //            response.Message = result;
        //            return response;
        //        }
        //        string encryptedUserId = model.user_id.ToString();
        //        EmailSetting setting = new EmailSetting
        //        {
        //            EmailEnableSsl = Convert.ToBoolean(_appSettings.EmailEnableSsl),
        //            EmailHostName = _appSettings.EmailHostName,
        //            EmailPassword = _appSettings.EmailPassword,
        //            EmailAppPassword = _appSettings.EmailAppPassword,
        //            EmailPort = Convert.ToInt32(_appSettings.EmailPort),
        //            EmailUsername = _appSettings.EmailUsername,
        //            FromEmail = _appSettings.FromEmail,
        //            FromName = _appSettings.FromName,
        //        };

        //        string emailBody = "";
        //        var BasePath = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates");

        //        if (!Directory.Exists(BasePath))
        //        {
        //            Directory.CreateDirectory(BasePath);
        //        }
        //        using (StreamReader reader = new StreamReader(Path.Combine(BasePath, "WelcomeUser.html")))
        //        {
        //            string host = _httpContextAccessor.HttpContext.Request.Host.Value;
        //            string scheme = _httpContextAccessor.HttpContext.Request.Scheme;
        //            string hosturl = scheme + "://" + host;

        //            string Client_URL = _config["Data:APPURL"].ToString();
        //            emailBody = reader.ReadToEnd();
        //            emailBody = emailBody.Replace("##APILink##", hosturl);
        //            emailBody = emailBody.Replace("##RoleName##", (model.role_name).ToString());//
        //            emailBody = emailBody.Replace("##BackToRegistrationForm##", Client_URL + "/#/register?userId=" + UserId);
        //        }
        //        bool isSuccess = SendMailMessage(model.email, null, null, "Invitation", emailBody, setting, null);
        //        if (isSuccess)
        //        {
        //            response.Message = "Invitation mail sent.";
        //            response.Success = true;
        //        }
        //        else
        //        {
        //            response.Message = "Something went wrong. Please try again after sometime.";
        //            response.Success = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Information(ex.ToString());
        //        response.Success = false;
        //        response.Message = ex.Message;
        //        //throw;
        //    }
        //    return response;
        //}

        //[HttpPost("SaveBoardingStep")]
        //public async Task<BaseApiResponse> SaveBoardingStep([FromBody] UserMasterModel model)
        //{
        //    BaseApiResponse response = new BaseApiResponse();
        //    try
        //    {
        //        var result = await _userService.SaveBoardingStep(model);
        //        if (!string.IsNullOrEmpty(result))
        //        {
        //            response.Success = false;
        //            response.Message = result;
        //        }
        //        else
        //        {
        //            response.Success = true;
        //            response.Message = "";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Information(ex.ToString());
        //        response.Success = false;
        //        response.Message = ex.Message;
        //        //throw;
        //    }
        //    return response;
        //}
        #endregion

        #region Delete
        [HttpDelete("DeleteUser")]
        public async Task<BaseApiResponse> DeleteUser(CommonIdModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            try
            {
                var result = await _userService.DeleteUser(model);
                if (result)
                {
                    response.Message = _commonMessages.User.DeleteSuccess;
                    response.Success = true;
                }
                else
                {
                    response.Message = _commonMessages.User.DeleteError;
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {

                string st = _commonMessages.CreateCommonMessage("DeleteUser", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = _commonMessages.Error;
            }
            return response;
        }
        #endregion
    }
}
