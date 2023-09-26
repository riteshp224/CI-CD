using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Platinum.Common;
using Platinum.Model;
using Platinum.Model.Models;
using Platinum.Service.Services.Login;
using Platinum.WebApi.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static Platinum.Common.EmailNotification;
using static Platinum.Common.EncryptionDecryption;

namespace Platinum.WebApi.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginApiController : ControllerBase
    {
        #region Fields
        private readonly ILoggerManager _logger;
        private ILoginService _loginService;
        private IConfiguration _config;
        private readonly CommonMessages _commonMessages;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationSettings _appSettings;
        #endregion

        #region Constructor
        public LoginApiController(ILoggerManager logger, ILoginService loginService, IConfiguration config, IHttpContextAccessor httpContextAccessor, IOptions<CommonMessages> commonMessages, IOptions<ApplicationSettings> appSettings)
        {
            _logger = logger;
            _loginService = loginService;
            _config = config;
            _commonMessages = commonMessages.Value;
            _httpContextAccessor = httpContextAccessor;
            _appSettings = appSettings.Value;
        }
        #endregion

        #region Post
        [HttpPost("LoginUser")]
        public async Task<ApiPostResponse<LoginModel>> LoginUser([FromBody] LoginModel model)
        {
            ApiPostResponse<LoginModel> response = new ApiPostResponse<LoginModel>();
            try
            {
                model.password = EncryptionDecryption.GetEncrypt(model.password);
                var result = await _loginService.LoginUser(model);
                if (result != null && string.IsNullOrEmpty(result.error))
                {
                    string host = _httpContextAccessor.HttpContext.Request.Host.Value;
                    string scheme = _httpContextAccessor.HttpContext.Request.Scheme;
                    string hosturl = scheme + "://" + host;
                    result.userPhoto = hosturl + "/Documents/UserProfile/" + result.userPhoto;
                    result.logoUrl = hosturl + "/Documents/Organization/Logo/" + result.logoUrl;
                    result.JWTToken = JWTToken.GenerateJSONWebToken(result.email, result.userId.ToString(), result.organizationId.ToString(), result.roleId.ToString(), _appSettings.JWT_Secret);
                    result.email = "";
                    response.Data = result;
                    response.Message = "You are logged in successfully.";
                    response.Success = true;
                }
                else
                {
                    response.Message = result.error;
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                _logger.Information(ex.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost("ForgotPassword")]
        public async Task<BaseApiResponse> ForgotPassword([FromBody] LoginModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            try
            {
                var UserId = await _loginService.ValidateUserEmail(model.email);
                if (UserId > 0)
                {
                    string encryptedUserId = HttpUtility.UrlEncode(GetEncrypt(UserId.ToString()));

                    EmailSetting setting = new EmailSetting
                    {
                        EmailEnableSsl = Convert.ToBoolean(_appSettings.EmailEnableSsl),
                        EmailHostName = _appSettings.EmailHostName,
                        EmailPassword = _appSettings.EmailPassword,
                        EmailAppPassword = _appSettings.EmailAppPassword,
                        EmailPort = Convert.ToInt32(_appSettings.EmailPort),
                        EmailUsername = _appSettings.EmailUsername,
                        FromEmail = _appSettings.FromEmail,
                        FromName = _appSettings.FromName,
                    };

                    string emailBody = "";
                    var BasePath = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates");

                    if (!Directory.Exists(BasePath))
                    {
                        Directory.CreateDirectory(BasePath);
                    }
                    using (StreamReader reader = new StreamReader(Path.Combine(BasePath, "ForgotPassword.html")))
                    {
                        string host = _httpContextAccessor.HttpContext.Request.Host.Value;
                        string scheme = _httpContextAccessor.HttpContext.Request.Scheme;
                        string hosturl = scheme + "://" + host;

                        string Client_URL = _config["Data:APPURL"].ToString();
                        emailBody = reader.ReadToEnd();
                        emailBody = emailBody.Replace("##APILink##", hosturl);
                        emailBody = emailBody.Replace("##ResetPasswordLink##", Client_URL + "#/reset-forgot-password?userId=" + encryptedUserId);
                    }
                    bool isSuccess = SendMailMessage(model.email, null, null, "Forgot Password", emailBody, setting, null);
                    if (isSuccess)
                    {
                        response.Message = "Password sent to your registered email address.";
                        response.Success = true;
                    }
                    else
                    {
                        response.Message = "Something went wrong. Please try again after sometime.";
                        response.Success = false;
                    }
                }
                else
                {
                    response.Message = "Email address doesn't exists.";
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                _logger.Information(ex.ToString());
                response.Success = false;
                response.Message = ex.Message;
                //throw;
            }
            return response;
        }

        [HttpPost("ResetForgotPassword")]
        public async Task<BaseApiResponse> ResetForgotPassword([FromBody] UserForgotPasswordModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            try
            {
                if (!string.IsNullOrEmpty(model.encryptedUserId))
                {
                    model.encryptedUserId = HttpUtility.UrlDecode(model.encryptedUserId);
                    model.userId = Convert.ToInt64(GetDecrypt(model.encryptedUserId));
                }
                else
                {
                    model.userId = model.LoggedInUserId;
                }
                model.password = GetEncrypt(model.password);
                var result = await _loginService.ResetForgotPassword(model);
                if (string.IsNullOrEmpty(result))
                {
                    response.Message = "Password successfully changed.";
                    response.Success = true;
                }
                else
                {
                    response.Message = result;
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                _logger.Information(ex.ToString());
                response.Success = false;
                response.Message = ex.Message;
                //throw;
            }
            return response;
        }
        #endregion
    }
}
