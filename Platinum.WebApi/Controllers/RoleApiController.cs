using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Platinum.Common;
using Platinum.Model;
using Platinum.WebApi.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Platinum.Model.Models;
using Microsoft.Extensions.Options;
using Platinum.Service.Services.Role;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Platinum.WebApi.Controllers
{
    [Route("api/role")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RoleApiController : ControllerBase
    {
        #region Fields
        private readonly ILoggerManager _logger;
        private IRoleService _roleService;
        private IConfiguration _config;
        private readonly CommonMessages _commonMessages;
        private readonly DataConfig _dataConfig;
        private readonly ApplicationSettings _appSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructor
        public RoleApiController(
            ILoggerManager logger,
            IRoleService roleService,
            IConfiguration config,
            IOptions<CommonMessages> commonMessages,
            IOptions<DataConfig> dataConfig,
            IOptions<ApplicationSettings> appSettings,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _logger = logger;
            _roleService = roleService;
            _config = config;
            _commonMessages = commonMessages.Value;
            _dataConfig = dataConfig.Value;
            _appSettings = appSettings.Value;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Get
        [HttpGet("GetRoleList")]
        public async Task<ApiResponse<RoleMasterModel>> GetRoleList()
        {
            ApiResponse<RoleMasterModel> response = new ApiResponse<RoleMasterModel>() { Data = new List<RoleMasterModel>() };
            try
            {
                var data = await _roleService.GetRoleList();
                response.Data = data;
                response.Success = true;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetRoleList", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = _commonMessages.Error;
            }
            return response;
        }

        [HttpGet("GetRoleById/{roleId}")]
        public async Task<ApiPostResponse<RoleMasterModel>> GetRoleById(long roleId)
        {
            ApiPostResponse<RoleMasterModel> response = new ApiPostResponse<RoleMasterModel>() { Data = new RoleMasterModel() };
            try
            {
                var data = await _roleService.GetRoleById(roleId);
                response.Data = data;
                response.Success = true;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetRoleById", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = _commonMessages.Error;
            }
            return response;
        }
        #endregion

        #region Post
        [HttpPost("SaveRole")]
        public async Task<BaseApiResponse> SaveRole([FromForm] RoleMasterModel model)
        {

            BaseApiResponse response = new BaseApiResponse();
            try
            {
                var result = await _roleService.SaveRoleData(model);
                if (string.IsNullOrEmpty(result))
                {
                    response.Message = _commonMessages.Role.SaveSuccess;
                    response.Success = true;
                }
                else if (Convert.ToInt32(result) == 1)
                {
                    response.Message = _commonMessages.Role.AlreadyExists;
                    response.Success = false;
                }
                else
                {
                    response.Message = _commonMessages.Role.SaveError;
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {

                string st = _commonMessages.CreateCommonMessage("SaveRole", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = _commonMessages.Error;
            }
            return response;
        }
        #endregion

        #region Delete
        [HttpDelete("DeleteRole")]
        public async Task<BaseApiResponse> DeleteRole(CommonIdModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            try
            {
                var result = await _roleService.DeleteRole(model);
                if (result)
                {
                    response.Message = _commonMessages.Role.DeleteSuccess;
                    response.Success = true;
                }
                else
                {
                    response.Message = _commonMessages.Role.DeleteError;
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {

                string st = _commonMessages.CreateCommonMessage("DeleteRole", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = _commonMessages.Error;
            }
            return response;
        }
        #endregion
    }
}
