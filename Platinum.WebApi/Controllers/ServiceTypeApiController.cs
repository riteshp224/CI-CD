using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Platinum.Common;
using Platinum.Model;
using Platinum.Model.Models;
using Platinum.Service.Services.ServiceType;
using Platinum.WebApi.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Platinum.WebApi.Controllers
{
    [Route("api/ServiceType")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ServiceTypeApiController : ControllerBase
    {
        #region Fields
        private readonly ILoggerManager _logger;
        private IServiceTypeService _ServiceTypeService;
        private IConfiguration _config;
        private readonly CommonMessages _commonMessages;
        private readonly DataConfig _dataConfig;
        private readonly ApplicationSettings _appSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string APIBaseURL;
        #endregion

        #region Constructor
        public ServiceTypeApiController(
            ILoggerManager logger,
            IServiceTypeService ServiceTypeService,
            IConfiguration config,
            IOptions<CommonMessages> commonMessages,
            IOptions<DataConfig> dataConfig,
            IOptions<ApplicationSettings> appSettings,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _logger = logger;
            _ServiceTypeService = ServiceTypeService;
            _config = config;
            _commonMessages = commonMessages.Value;
            _dataConfig = dataConfig.Value;
            _appSettings = appSettings.Value;
            _httpContextAccessor = httpContextAccessor;
            APIBaseURL = dataConfig.Value.FilePath;
        }
        #endregion

        #region Get
        [HttpGet("GetParentServiceTypeList")]
        public async Task<ApiResponse<ServiceTypeMasterModel>> GetParentServiceTypeList()
        {
            ApiResponse<ServiceTypeMasterModel> response = new ApiResponse<ServiceTypeMasterModel>() { Data = new List<ServiceTypeMasterModel>() };
            try
            {
                var data = await _ServiceTypeService.GetParentServiceTypeList();
                response.Data = data;
                response.Success = true;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetServiceTypeListCollection", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = _commonMessages.Error;
            }
            return response;
        }

        [HttpPost("GetServiceTypeList")]
        public async Task<ApiResponse<ServiceTypeMasterModel>> GetServiceTypeList(CommonPaginationModel model)
        {
            ApiResponse<ServiceTypeMasterModel> response = new ApiResponse<ServiceTypeMasterModel>() { Data = new List<ServiceTypeMasterModel>() };
            try
            {
                var data = await _ServiceTypeService.GetServiceTypeList(model);
                response.Data = data;
                response.Success = true;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetServiceTypeList", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = _commonMessages.Error;
            }
            return response;
        }

        [HttpGet("GetServiceTypeById/{ServiceTypeId}")]
        public async Task<ApiPostResponse<ServiceTypeMasterModel>> GetServiceTypeById(long ServiceTypeId)
        {
            ApiPostResponse<ServiceTypeMasterModel> response = new ApiPostResponse<ServiceTypeMasterModel>() { Data = new ServiceTypeMasterModel() };
            try
            {
                var data = await _ServiceTypeService.GetServiceTypeById(ServiceTypeId);
                response.Data = data;
                if (response.Data != null)
                {
                    response.Data.ImageURL = !string.IsNullOrEmpty(data.ServiceTypePhoto) ? _dataConfig.FilePath + "ServiceTypes/" + data.ServiceTypePhoto : null;
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetServiceTypeById", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = _commonMessages.Error;
            }
            return response;
        }
        #endregion

        #region Post
        [HttpPost("SaveServiceType")]

        public async Task<BaseApiResponse> SaveServiceType([FromForm] ServiceTypeMasterModel model)
        {

            BaseApiResponse response = new BaseApiResponse();
            try
            {


                if (model.ServiceTypePhotoFile != null)
                {
                    Guid guidFile = Guid.NewGuid();
                    var FileName = guidFile + Path.GetExtension(model.ServiceTypePhotoFile.FileName);
                    var BasePath = Path.Combine(Directory.GetCurrentDirectory(), "Documents/ServiceTypes");

                    if (!Directory.Exists(BasePath))
                    {
                        Directory.CreateDirectory(BasePath);
                    }
                    var path = Path.Combine(BasePath, FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.ServiceTypePhotoFile.CopyToAsync(stream);
                    }
                    model.ServiceTypePhoto = FileName;
                }
                
                if (model.Cost == null)
                {
                    model.Cost = 0;
                }
                var result = await _ServiceTypeService.SaveServiceTypeData(model);
                if (string.IsNullOrEmpty(result))
                {
                    response.Message = _commonMessages.ServiceType.SaveSuccess;
                    response.Success = true;
                }
                else if (Convert.ToInt32(result) == 1)
                {
                    response.Message = _commonMessages.ServiceType.AlreadyExists;
                    response.Success = false;
                }
                else
                {
                    response.Message = _commonMessages.ServiceType.SaveError;
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {

                string st = _commonMessages.CreateCommonMessage("SaveServiceType", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = _commonMessages.Error;
            }
            return response;
        }
        #endregion

        #region Delete
        [HttpDelete("DeleteServiceType")]
        public async Task<BaseApiResponse> DeleteServiceType(CommonIdModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            try
            {
                var result = await _ServiceTypeService.DeleteServiceType(model);
                if (result)
                {
                    response.Message = _commonMessages.ServiceType.DeleteSuccess;
                    response.Success = true;
                }
                else
                {
                    response.Message = _commonMessages.ServiceType.DeleteError;
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {

                string st = _commonMessages.CreateCommonMessage("DeleteServiceType", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = _commonMessages.Error;
            }
            return response;
        }
        #endregion
    }
}
