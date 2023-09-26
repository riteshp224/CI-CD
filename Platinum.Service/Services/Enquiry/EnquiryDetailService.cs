using Platinum.Data.DBRepository.Enquiry;
using Platinum.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace Platinum.Service.Services.Enquiry
{
    public class EnquiryDetailService : IEnquiryDetailService
    {
        #region Fields
        private readonly IEnquiryDetailRepository _repository;
        #endregion

        #region Construtor
        public EnquiryDetailService(IEnquiryDetailRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Get
        public async Task<List<EnquiryDetailModel>> GetEnquiryDetailList(CommonPaginationModel model)
        {
            return await _repository.GetEnquiryDetailList(model);
        }
        public async Task<List<EnquiryDetailModel>> GetServiceTypeDetailList(string strServiceTypeId = "0")
        {
            return await _repository.GetServiceTypeDetailList(strServiceTypeId);
        }
        public async Task<EnquiryDetailModel> GetEnquiryDetailById(long EnquiryDetailId)
        {
            return await _repository.GetEnquiryDetailById(EnquiryDetailId);
        }
        #endregion

        #region Post

        public async Task<string> SaveEnquiryDetailData(EnquiryDetailModel model)
        {
            return await _repository.SaveEnquiryDetailData(model);
        }
        #endregion

        #region Delete
        public async Task<bool> DeleteEnquiryDetail(CommonIdModel model)
        {
            return await _repository.DeleteEnquiryDetail(model);
        }
        #endregion
    }
}
