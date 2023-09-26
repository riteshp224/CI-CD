using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Platinum.Common;
using Platinum.Model;
using Platinum.Model.Models;
using Platinum.Service.Services.RoleRights;
using Platinum.WebApi.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platinum.WebApi.Controllers
{
    [Route("api/rolerights")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RoleRightsApiController : ControllerBase
    {
        #region Fields
        private readonly ILoggerManager _logger;
        private IRoleRightsService _rolerightsService;
        private IConfiguration _config;
        private readonly CommonMessages _commonMessages;
        #endregion

        #region Constructor
        public RoleRightsApiController(ILoggerManager logger, IRoleRightsService rolerightsService, IConfiguration config, IOptions<CommonMessages> commonMessages)
        {
            _logger = logger;
            _rolerightsService = rolerightsService;
            _config = config;
            _commonMessages = commonMessages.Value;
        }
        #endregion

        #region Get
        [HttpGet("GetRoleRightsByUserId/{userId}")]
        public async Task<ApiResponse<RoleRightsMasterModel>> GetRoleRightsByUserId(long userId)
        {
            ApiResponse<RoleRightsMasterModel> response = new ApiResponse<RoleRightsMasterModel>() { Data = new List<RoleRightsMasterModel>() };
            try
            {
                var data = await _rolerightsService.GetRoleRightsByUserId(userId);
                response.Data = data;
                response.Success = true;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetRoleRightsByUserId", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = _commonMessages.Error;
            }
            return response;
        }
        [HttpGet("GetRoleRightsByRoleId/{roleId}")]
        public async Task<ApiResponse<RoleRightsMasterModel>> GetRoleRightsByRoleId(long roleId)
        {

            ApiResponse<RoleRightsMasterModel> response = new ApiResponse<RoleRightsMasterModel>() { Data = new List<RoleRightsMasterModel>() };
            try
            {
                var data = await _rolerightsService.GetRoleRightsByRoleId(roleId);
                response.Data = data;
                response.Success = true;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetRoleRightsByRoleId", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = _commonMessages.Error;
            }
            return response;
        }
        #endregion

        #region Post
        //[HttpPost("GetMenuListByRoleId")]
        //public async Task<ApiResponse<RoleRightsMasterModel>> GetMenuListByRoleId(CommonModel model)
        //{

        //    ApiResponse<RoleRightsModel> response = new ApiResponse<RoleRightsModel>() { Data = new List<RoleRightsModel>() };
        //    try
        //    {
        //        var data = await _rolerightsService.GetMenuListByRoleId(model);
        //        response.Data = data;
        //        response.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        string st = _commonMessages.CreateCommonMessage("GetRoleRightsByRoleId", ex.ToString());
        //        _logger.Information(st.ToString());
        //        response.Success = false;
        //        response.Message = _commonMessages.Error;
        //    }
        //    return response;
        //}
        [HttpPost("SaveRoleRights")]
        public async Task<BaseApiResponse> SaveRoleRights(RoleRightMasterModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            try
            {
                var result = await _rolerightsService.SaveRoleRightsData(model);
                if (string.IsNullOrEmpty(result))
                {
                    response.Message = _commonMessages.RoleRights.SaveSuccess;
                    response.Success = true;
                }
                else
                {
                    response.Message = _commonMessages.RoleRights.SaveError;
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("SaveRoleRights", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = _commonMessages.Error;
            }
            return response;
        }
        #endregion
    }
}
