using Platinum.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platinum.Service.Services.Enquiry
{
    public interface IEnquiryDetailService
    {
        #region Get
        Task<List<EnquiryDetailModel>> GetEnquiryDetailList(CommonPaginationModel model);
        Task<List<EnquiryDetailModel>> GetServiceTypeDetailList(string strServiceTypeId = "0");
        Task<EnquiryDetailModel> GetEnquiryDetailById(long EnquiryDetailId);
        #endregion

        #region Post
        Task<string> SaveEnquiryDetailData(EnquiryDetailModel model);
        #endregion

        #region Delete
        Task<bool> DeleteEnquiryDetail(CommonIdModel model);
        #endregion
    }
}
