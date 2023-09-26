using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Platinum.Common;
using Platinum.Model;
using Platinum.Model.Models;
using Platinum.Service.Services.Enquiry;
using Platinum.WebApi.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platinum.WebApi.Controllers
{
    [Route("api/EnquiryDetail")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EnquiryDetailApiController : ControllerBase
    {
        #region Fields
        private readonly ILoggerManager _logger;
        private IEnquiryDetailService _EnquiryDetailService;
        private IConfiguration _config;
        private readonly CommonMessages _commonMessages;
        private readonly DataConfig _dataConfig;
        private readonly ApplicationSettings _appSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructor
        public EnquiryDetailApiController(
            ILoggerManager logger,
            IEnquiryDetailService EnquiryDetailService,
            IConfiguration config,
            IOptions<CommonMessages> commonMessages,
            IOptions<DataConfig> dataConfig,
            IOptions<ApplicationSettings> appSettings,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _logger = logger;
            _EnquiryDetailService = EnquiryDetailService;
            _config = config;
            _commonMessages = commonMessages.Value;
            _dataConfig = dataConfig.Value;
            _appSettings = appSettings.Value;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Get
        [HttpPost("GetEnquiryDetailList")]
        public async Task<ApiResponse<EnquiryDetailModel>> GetEnquiryDetailList(CommonPaginationModel model)
        {
            ApiResponse<EnquiryDetailModel> response = new ApiResponse<EnquiryDetailModel>() { Data = new List<EnquiryDetailModel>() };
            try
            {
                var data = await _EnquiryDetailService.GetEnquiryDetailList(model);
                response.Data = data;
                response.Success = true;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetEnquiryDetailList", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = _commonMessages.Error;
            }
            return response;
        }


        [HttpPost("GetServiceTypeDetailList")]
        [AllowAnonymous]
        public async Task<ApiResponse<EnquiryDetailModel>> GetServiceTypeDetailList(string ServiceTypeId = "0")
        {
            ApiResponse<EnquiryDetailModel> response = new ApiResponse<EnquiryDetailModel>() { Data = new List<EnquiryDetailModel>() };
            try
            {
                var data = await _EnquiryDetailService.GetServiceTypeDetailList(ServiceTypeId);
                response.Data = data;
                response.Success = true;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetServiceTypeDetailList", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = _commonMessages.Error;
            }
            return response;
        }

        [HttpGet("GetEnquiryDetailById/{EnquiryDetailId}")]
        public async Task<ApiPostResponse<EnquiryDetailModel>> GetEnquiryDetailById(long EnquiryDetailId)
        {
            ApiPostResponse<EnquiryDetailModel> response = new ApiPostResponse<EnquiryDetailModel>() { Data = new EnquiryDetailModel() };
            try
            {
                var data = await _EnquiryDetailService.GetEnquiryDetailById(EnquiryDetailId);
                response.Data = data;
                response.Success = true;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetEnquiryDetailById", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = _commonMessages.Error;
            }
            return response;
        }


        #endregion

        #region Post
        [HttpPost("SaveEnquiryDetail")]
        async Task<BaseApiResponse> SaveEnquiryDetail([FromBody] EnquiryDetailModel model)
        {

            BaseApiResponse response = new BaseApiResponse();
            try
            {
                var result = await _EnquiryDetailService.SaveEnquiryDetailData(model);
                if (string.IsNullOrEmpty(result))
                {
                    response.Message = _commonMessages.EnquiryDetail.SaveSuccess;
                    response.Success = true;
                }
                else if (Convert.ToInt32(result) == 1)
                {
                    response.Message = _commonMessages.EnquiryDetail.AlreadyExists;
                    response.Success = false;
                }
                else
                {
                    response.Message = _commonMessages.EnquiryDetail.SaveError;
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {

                string st = _commonMessages.CreateCommonMessage("SaveEnquiryDetail", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = _commonMessages.Error;
            }
            return response;
        }
        #endregion

        #region Delete
        [HttpDelete("DeleteEnquiryDetail")]
        public async Task<BaseApiResponse> DeleteEnquiryDetail(CommonIdModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            try
            {
                var result = await _EnquiryDetailService.DeleteEnquiryDetail(model);
                if (result)
                {
                    response.Message = _commonMessages.EnquiryDetail.DeleteSuccess;
                    response.Success = true;
                }
                else
                {
                    response.Message = _commonMessages.EnquiryDetail.DeleteError;
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {

                string st = _commonMessages.CreateCommonMessage("DeleteEnquiryDetail", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = _commonMessages.Error;
            }
            return response;
        }
        #endregion
    }
}
