using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Platinum.Data.DBRepository.Enquiry;
using Platinum.Model;
using Platinum.Model.Models;
namespace Platinum.Data.DBRepository.Enquiry
{
    public class EnquiryRepository : BaseRepository, IEnquiryRepository
    {
        #region Fields
        private IConfiguration _config;
        private readonly DataConfig _dataConfig;
        #endregion

        #region Constructor
        public EnquiryRepository(IConfiguration config, IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
            _config = config;
            _dataConfig = dataConfig.Value;
        }
        #endregion

        #region Get
        public async Task<List<EnquiryModel>> GetEnquiryList(CommonPaginationModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Type", 1);
                param.Add("@PageNumber", model.PageNumber);
                param.Add("@PageSize", model.PageSize);
                param.Add("@strSearch", model.StrSearch);
                //param.Add("@orderBy", model.SortColumn);
                //param.Add("@sortOrder", model.SortOrder);
                param.Add("@Path", _dataConfig.FilePath + "ServiceTypes/");
                var data = await QueryAsync<EnquiryModel>("SP_Enquiry_v2", param, commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<EnquiryModel> GetEnquiryById(long EnquiryId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EnquiryId", EnquiryId);
                param.Add("@Type", 2);
                return await QueryFirstOrDefaultAsync<EnquiryModel>("SP_Enquiry_v2", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CrmStatusModel>> GetCrmStatusList()
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Type", 7);
                var data = await QueryAsync<CrmStatusModel>("SP_Enquiry_v2", param, commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Post
        public async Task<string> SaveEnquiryData(EnquiryModel model)
        {
            try
            {

                DataTable dtEnquiryDetails = new DataTable("tbl_EnquiryDetail");
                dtEnquiryDetails.Columns.Add("EnquiryDetailId");
                dtEnquiryDetails.Columns.Add("EnquiryId");
                dtEnquiryDetails.Columns.Add("ServiceTypeId");
                dtEnquiryDetails.Columns.Add("Cost");
                dtEnquiryDetails.Columns.Add("Qty");
                dtEnquiryDetails.Columns.Add("isDelete");

                if (model.EnquiryDetails.Count > 0)
                {
                    foreach (var item in model.EnquiryDetails)
                    {
                        DataRow dtRow = dtEnquiryDetails.NewRow();
                        dtRow["EnquiryDetailId"] = item.EnquiryDetailId;
                        dtRow["EnquiryId"] = item.EnquiryId;
                        dtRow["ServiceTypeId"] = item.ServiceTypeId;
                        dtRow["Cost"] = item.Cost;
                        dtRow["Qty"] = item.Qty;
                        dtRow["isDelete"] = 0;
                        dtEnquiryDetails.Rows.Add(dtRow);
                    }
                }

                var param = new DynamicParameters();
                param.Add("@EnquiryId", model.EnquiryId);
                param.Add("@StatusId", model.StatusId);
                param.Add("@CustomerName", model.CustomerName);
                param.Add("@email", model.Email);
                param.Add("@phone", model.Phone);
                param.Add("@Address", model.Address);
                param.Add("@City", model.City);
                param.Add("@Pincode", model.Pincode);
                param.Add("@Message", model.Message);
                param.Add("@isActive", model.isActive);
                param.Add("@userId", model.LoggedInUserId);
                param.Add("@EnquiryDetail", dtEnquiryDetails.AsTableValuedParameter("[dbo].[tbl_EnquiryDetail]"));
                if (model.EnquiryId != 0)
                    param.Add("@Type", 4);
                else
                    param.Add("@Type", 3);
                return await QueryFirstOrDefaultAsync<string>("SP_Enquiry_v2", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Delete
        public async Task<bool> DeleteEnquiry(CommonIdModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EnquiryId", model.id);
                param.Add("@Type", 5);
                var result = await QueryFirstOrDefaultAsync<string>("SP_Enquiry_v2", param, commandType: CommandType.StoredProcedure);
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
