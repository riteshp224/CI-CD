using Platinum.Data.DBRepository.Enquiry;
using Platinum.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platinum.Service.Services.Enquiry
{
   public class EnquiryService : IEnquiryService
    {
        #region Fields
        private readonly IEnquiryRepository _repository;
        #endregion

        #region Construtor
        public EnquiryService(IEnquiryRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Get
        public async Task<List<EnquiryModel>> GetEnquiryList(CommonPaginationModel model)
        {
            return await _repository.GetEnquiryList(model);
        }
        public async Task<EnquiryModel> GetEnquiryById(long EnquiryId)
        {
            return await _repository.GetEnquiryById(EnquiryId);
        }

        public async Task<List<CrmStatusModel>> GetCrmStatusList()
        {
            return await _repository.GetCrmStatusList();
        }

        #endregion

        #region Post

        public async Task<string> SaveEnquiryData(EnquiryModel model)
        {
            return await _repository.SaveEnquiryData(model);
        }
        #endregion

        #region Delete
        public async Task<bool> DeleteEnquiry(CommonIdModel model)
        {
            return await _repository.DeleteEnquiry(model);
        }
        #endregion
    }
}
