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
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static Platinum.Common.EmailNotification;

namespace Platinum.WebApi.Controllers
{
    [Route("api/Enquiry")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EnquiryApiController : ControllerBase
    {
        #region Fields
        private readonly ILoggerManager _logger;
        private IEnquiryService _EnquiryService;
        private IEnquiryDetailService _EnquiryDetailService;
        private IConfiguration _config;
        private readonly CommonMessages _commonMessages;
        private readonly DataConfig _dataConfig;
        private readonly ApplicationSettings _appSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructor
        public EnquiryApiController(
            ILoggerManager logger,
            IEnquiryService EnquiryService,
            IEnquiryDetailService EnquiryDetailService,
            IConfiguration config,
            IOptions<CommonMessages> commonMessages,
            IOptions<DataConfig> dataConfig,
            IOptions<ApplicationSettings> appSettings,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _logger = logger;
            _EnquiryService = EnquiryService;
            _EnquiryDetailService = EnquiryDetailService;
            _config = config;
            _commonMessages = commonMessages.Value;
            _dataConfig = dataConfig.Value;
            _appSettings = appSettings.Value;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Get
        [HttpPost("GetEnquiryList")]
        public async Task<ApiResponse<EnquiryModel>> GetEnquiryList(CommonPaginationModel model)
        {
            ApiResponse<EnquiryModel> response = new ApiResponse<EnquiryModel>() { Data = new List<EnquiryModel>() };
            try
            {
                var data = await _EnquiryService.GetEnquiryList(model);
                int i = 0;
                foreach(var data1 in data)
                {
                    //if(data1.ImageURL!=null && data1.ImageURL.Split(",").Count()==5)
                    //{
                        data[i].ListImgCount = data1.ImageURL.Split(",").Count();
                    data[i].ImageURL = data1.ImageURL.Replace(",","");
                    //data[i].ImageURL= data[i].ImageURL+ "<a class='plus-circle' id='aOpen_"+data1.EnquiryId+"' (click)='openPopup("+ data1.EnquiryId + ")'><i class='fa fa-plus'></i></a>";
                    //}
                    i++;
                }
                response.Data = data;
                response.Success = true;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetEnquiryList", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = _commonMessages.Error;
            }
            return response;
        }

        [HttpGet("GetEnquiryById/{EnquiryId}")]
        public async Task<ApiPostResponse<EnquiryModel>> GetEnquiryById(long EnquiryId)
        {
            ApiPostResponse<EnquiryModel> response = new ApiPostResponse<EnquiryModel>() { Data = new EnquiryModel() };

            try
            {
                var data = await _EnquiryService.GetEnquiryById(EnquiryId);
                response.Data = data;
                response.Success = true;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetEnquiryById", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = _commonMessages.Error;
            }
            return response;
        }
        [HttpGet("GetCrmStatusList")]
        public async Task<ApiResponse<CrmStatusModel>> GetCrmStatusList()
        {
            ApiResponse<CrmStatusModel> response = new ApiResponse<CrmStatusModel>() { Data = new List<CrmStatusModel>() };
            try
            {
                var data = await _EnquiryService.GetCrmStatusList();
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

        #endregion

        #region Post
        [HttpPost("SaveEnquiry")]
        [AllowAnonymous]
        public async Task<BaseApiResponse> SaveEnquiry(EnquiryModel model)
        {

            BaseApiResponse response = new BaseApiResponse();
            try
            {
                var result = await _EnquiryService.SaveEnquiryData(model);
                string savesuccess =string.Empty;
                if (!string.IsNullOrEmpty(result))
                {
                    savesuccess = result.Split("|")[0];
                    
                }
               
                if (string.IsNullOrEmpty(savesuccess) && model.EnquiryId==0)
                {
                    int enquiryId = Convert.ToInt32(result.Split("|")[1]);
                    response.Message = _commonMessages.Enquiry.SaveSuccess;
                    response.Id = enquiryId.ToString();
                    response.Success = true;

                    var enquiryDetail = await _EnquiryService.GetEnquiryById(enquiryId);
                    var enquiryDetails = await _EnquiryDetailService.GetEnquiryDetailList(new CommonPaginationModel { id = enquiryId });
                    string emailBody = "";
                    var BasePath = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates");
                    decimal Totalamt = 0;
                    decimal TaxAmount = 0;
                    decimal Total = 0;
                    string serviceDetailBody = "";
                    //string ParentserviceDetailBody = "";
                    string OldParentService = "";
                    foreach (var item in enquiryDetails)
                    {
                        decimal amt = item.Qty.Value * item.Cost.Value;
                        string ParentserviceDetailBody = "";
                        if (OldParentService!= item.ParentServiceTypeName)
                        {
                            ParentserviceDetailBody = "<tr style='border-collapse:collapse'>"+
"<td align=left style='padding:0;Margin:0;padding-left:20px;padding-right:20px'>"+
"<table width=100% cellspacing=0 cellpadding=0 style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'>"+
"<tbody><tr style='border-collapse:collapse'>"+
"<td valign=top align=center style='padding:0;Margin:0;width:560px'>"+
"<table width=100% cellspacing=0 cellpadding=0 role=presentation style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px ;'>"+
"<tbody><tr style='border-collapse:collapse'>"+
"<td align=center style='padding:0;Margin:0;padding-bottom:10px;'>"+
"<table width=100% height=100% cellspacing=0 cellpadding=0 border=0 role=presentation style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;'>"+
"<tbody><tr style='border-collapse:collapse'>"+
"<td style='padding:10px 10px;Margin:0;height:1px;width:100%;margin:0px; color:#000;'>"+
"<strong>"+item.ParentServiceTypeName +"</strong>"+
"</td>"+
"</tr>"+
"</tbody></table>"+
"</td>"+
"</tr>"+
"</tbody></table>"+
"</td>"+
"</tr>"+
"</tbody></table>"+
"</td>"+
"</tr>";

                            
                            OldParentService = item.ParentServiceTypeName;
                        }

                        serviceDetailBody = serviceDetailBody + ParentserviceDetailBody +
                                                                "<tr style='border-collapse:collapse'> "+
"<td align=left style='Margin:0;padding-top:5px;padding-left:20px;padding-right:20px'>" +

"<table class='es-left' cellspacing='0' cellpadding='0' align='left' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left;width:10%;'> " +
"<tr style='border-collapse:collapse'> " +
"<td class='es-m-p0r es-m-p20b' valign='top' align='center' style='padding:0;Margin:0;width:178px'> " +
"<table width='100%' cellspacing='0' cellpadding='0' role='presentation' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'> " +
"<tr style='border-collapse:collapse'> " +
"<td align='center' style='padding:0;Margin:0;font-size:0'><a href='#' style='-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;text-decoration:underline;color:#D48344;font-size:14px'><img src='" + ImageToBase64(item.ImageURL) + "' alt='Natural Balance L.I.D., sale 30%' class='adapt-img' title='Natural Balance L.I.D., sale 30%' width='40' style='display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic;border-radius:100%'></a></td> " +
"</tr> " +
"</table></td> " +
"</tr> " +
"</table>" +

"<table cellspacing=0 cellpadding=0 width=90% style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;'>" +
"<tbody><tr style='border-collapse:collapse'>" +
"<td align=left style='padding:0;Margin:0;width:560px'>" +
"<table width=100% cellspacing=0 cellpadding=0 role=presentation style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;'>" +
"<tbody><tr style='border-collapse:collapse'>" +
"<td align=left style='padding: 0 10px;Margin:0'>" +
"<table style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:100%;font-size: 14px;'" +
"font-weight: 500; class='cke_show_border' cellspacing=1 cellpadding=1 border=0 role=presentation>" +
"<tbody><tr style='border-collapse:collapse'>" +
//"<td style='padding:0;Margin:0; width:15%;'> <img src='"+ item.ImageURL+"' style='width: 55px; height: 55px; border-radius:100%; margin: 5px 8px; object-fit:cover;'></td>" +
"<td style='padding:0;Margin:0; width:20%;'>" + item.ServiceName + " </td>" +
"<td style='padding:0;Margin:0;width:20%;text-align:center'> £ "+ item.Cost.ToString() + "</td>" +
"<td style='padding:0;Margin:0;width:10%;text-align:center'>" + item.Qty.ToString() + "</td>" +
"<td style='padding:0;Margin:0;width:20%;text-align:center'> £ " + amt.ToString() + "</td>" +
"</tr>" +
"</tbody></table>" +
"<p style='Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;line-height:21px;color:#333333;font-size:14px'><br></p>" +
"</td>" +
"</tr>" +
"</tbody></table>" +
"</td>" +
"</tr>" +
"</tbody></table>" +
"</td>" +
"</tr>" +
"<tr style='border-collapse:collapse'>" +
"<td align=left style='padding:0;Margin:0;padding-left:20px;padding-right:20px'>" +
"<table width=100% cellspacing=0 cellpadding=0 style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'>" +
"<tbody><tr style='border-collapse:collapse'>" +
"<td valign=top align=center style='padding:0;Margin:0;width:560px'>" +
"<table width=100% cellspacing=0 cellpadding=0 role=presentation style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'>" +
"<tbody><tr style='border-collapse:collapse'>" +
"<td align=center style='padding:0;Margin:0;padding-bottom:10px;font-size:0'>" +
"<table width=100% height=100% cellspacing=0 cellpadding=0 border=0 role=presentation style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'>" +
"<tbody><tr style='border-collapse:collapse'>" +
"<td style='padding:0;Margin:0;border-bottom:1px solid #efefef;background:#FFFFFF none repeat scroll 0% 0%;height:1px;width:100%;margin:0px'>" +
"</td>" +
"</tr>" +
"</tbody></table>" +
"</td>" +
"</tr>" +
"</tbody></table>" +
"</td>" +
"</tr>" +
"</tbody></table>" +
"</td>" +
"</tr>";


                        

                        //serviceDetailBody = serviceDetailBody + "<tr style='text-align: center;'><td>" + item.ParentServiceTypeName +": "+ item.ServiceName + "</td><td>" + item.Cost.ToString() + "</td><td>" + item.Qty.ToString() + "</td><td style='text-align:right;padding:5px 10px'>" + amt.ToString() + "</td></tr>";
                        Totalamt = Totalamt + amt;
                    }
                    decimal taxper = (_dataConfig.TaxPer == null ? 20 : Convert.ToDecimal(_dataConfig.TaxPer));
                    TaxAmount = (Totalamt * taxper) / 100;
                    Total = Totalamt + TaxAmount;

                    if (!Directory.Exists(BasePath))
                    {
                        Directory.CreateDirectory(BasePath);
                    }
                    using (StreamReader reader = new StreamReader(Path.Combine(BasePath, "Enquire_v2.html")))
                    {
                        string host = _httpContextAccessor.HttpContext.Request.Host.Value;
                        string scheme = _httpContextAccessor.HttpContext.Request.Scheme;
                        string hosturl = scheme + "://" + host;

                        emailBody = reader.ReadToEnd();
                        emailBody = emailBody.Replace("##APILink##", hosturl);
                        emailBody = emailBody.Replace("##CustomerName##", model.CustomerName);
                        emailBody = emailBody.Replace("##CustomerAddress##", model.Address + " "+model.City+"-"+model.Pincode);
                        emailBody = emailBody.Replace("##OrderId##", enquiryDetail.OrderNo);
                        emailBody = emailBody.Replace("##OrderDate##", DateTime.Now.ToString("dd/MM/yyyy"));
                        emailBody = emailBody.Replace("##APILink##", hosturl);
                        emailBody = emailBody.Replace("##TableBody##", serviceDetailBody);
                        emailBody = emailBody.Replace("##SubTotal##", Totalamt.ToString());
                        emailBody = emailBody.Replace("##Tax##", TaxAmount.ToString());
                        emailBody = emailBody.Replace("##Total##", Total.ToString());
                    }

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
                    bool isSuccess = SendMailMessage(model.Email, null, null, "Enquiry Receipt", emailBody, setting, null);
                }
                else if(string.IsNullOrEmpty(savesuccess))
                {
                    response.Message = _commonMessages.Enquiry.SaveSuccess;
                    response.Success = true;
                }
                else if (Convert.ToInt32(result) == 1)
                {
                    response.Message = _commonMessages.Enquiry.AlreadyExists;
                    response.Success = false;
                }
                else
                {
                    response.Message = _commonMessages.Enquiry.SaveError;
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {

                string st = _commonMessages.CreateCommonMessage("SaveEnquiry", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = _commonMessages.Error;
            }
            return response;
        }

        private string ImageToBase64(string Path)
        {
            //string path = "D:\\SampleImage.jpg";
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(Path))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }
        #endregion

        #region Delete
        [HttpDelete("DeleteEnquiry")]
        public async Task<BaseApiResponse> DeleteEnquiry(CommonIdModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            try
            {
                var result = await _EnquiryService.DeleteEnquiry(model);
                if (result)
                {
                    response.Message = _commonMessages.Enquiry.DeleteSuccess;
                    response.Success = true;
                }
                else
                {
                    response.Message = _commonMessages.Enquiry.DeleteError;
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {

                string st = _commonMessages.CreateCommonMessage("DeleteEnquiry", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = _commonMessages.Error;
            }
            return response;
        }
        #endregion
    }
}
