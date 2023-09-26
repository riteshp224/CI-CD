using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Platinum.Model;
using Platinum.Model.Models;

namespace Platinum.Data.DBRepository.Enquiry
{
   public class EnquiryDetailRepository : BaseRepository, IEnquiryDetailRepository
    {
        #region Fields
        private IConfiguration _config;
        private readonly DataConfig _dataConfig;
        #endregion

        #region Constructor
        public EnquiryDetailRepository(IConfiguration config, IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
            _config = config;
            _dataConfig = dataConfig.Value;
        }
        #endregion

        #region Get
        public async Task<List<EnquiryDetailModel>> GetEnquiryDetailList(CommonPaginationModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Type", 1);
                param.Add("@EnquiryId", model.id);
                param.Add("@Path", _dataConfig.FilePath + "ServiceTypes/");
                var data = await QueryAsync<EnquiryDetailModel>("SP_EnquiryDetail", param, commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public async Task<List<EnquiryDetailModel>> GetServiceTypeDetailList(string strServiceTypeId = "0")
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Type", 7);
                param.Add("@strServiceTypeId", strServiceTypeId);
                param.Add("@Path", _dataConfig.FilePath + "ServiceTypes/");
                var data = await QueryAsync<EnquiryDetailModel>("SP_EnquiryDetail", param, commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<EnquiryDetailModel> GetEnquiryDetailById(long EnquiryDetailId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EnquiryDetailId", EnquiryDetailId);
                param.Add("@Type", 2);
                return await QueryFirstOrDefaultAsync<EnquiryDetailModel>("SP_EnquiryDetail", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        


        #endregion

        #region Post
        public async Task<string> SaveEnquiryDetailData(EnquiryDetailModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EnquiryId", model.EnquiryId);
                param.Add("@EnquiryDetailId", model.EnquiryDetailId);
                param.Add("@ServiceTypeId", model.ServiceTypeId);
                param.Add("@Cost", model.Cost);
                param.Add("@Qty", model.Qty);
                param.Add("@userId", model.LoggedInUserId);
                if (model.EnquiryDetailId != 0)
                    param.Add("@Type", 4);
                else
                    param.Add("@Type", 3);
                return await QueryFirstOrDefaultAsync<string>("SP_EnquiryDetail", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Delete
        public async Task<bool> DeleteEnquiryDetail(CommonIdModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EnquiryDetailId", model.id);
                param.Add("@Type", 5);
                var result = await QueryFirstOrDefaultAsync<string>("SP_EnquiryDetail", param, commandType: CommandType.StoredProcedure);
                if (string.IsNullOrEmpty(result))
                {
                    return true;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return false;
        }
        #endregion
    }
}
