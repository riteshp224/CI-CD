using Platinum.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platinum.Service.Services.Enquiry
{
    public interface IEnquiryService
    {
        #region Get
        Task<List<EnquiryModel>> GetEnquiryList(CommonPaginationModel model);
        Task<EnquiryModel> GetEnquiryById(long EnquiryId);
        Task<List<CrmStatusModel>> GetCrmStatusList();
        #endregion

        #region Post
        Task<string> SaveEnquiryData(EnquiryModel model);
        #endregion

        #region Delete
        Task<bool> DeleteEnquiry(CommonIdModel model);
        #endregion
    }
}
