using System;
using System.Collections.Generic;
using System.Text;

namespace Platinum.Model.Models
{
    public class CommonModel
    {        
        public long LoggedInUserId { get; set; }
        //public long roleId { get; set; }
        //public long menuId { get; set; }
        //public long rightsId { get; set; }
        public long? TotalRecord { get; set; }
        public long? TotalFilteredRecord { get; set; }
        public bool? isActive { get; set; }
        public bool? isDelete { get; set; }
        public long? createdBy { get; set; }
        public DateTime? createdOn { get; set; }
        public long? updatedBy { get; set; }
        public DateTime? updatedOn { get; set; }
    }

    public class CommonDDLModel : CommonIdModel
    {
        public string value { get; set; }
    }

    public class CommonIdModel : CommonModel
    {
        public long id { get; set; }
    }
    public class CommonPaginationModel: CommonModel
    {
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public string StrSearch { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public long id { get; set; }
        //public string kendo_logic_operator { get; set; }
        //public List<KendoFilterModel> kendoFilters { get; set; }
    }

    public class KendoFilterModel
    {
        public string field_name { get; set; }
        public string logic_operator { get; set; }
        public string field_value { get; set; }
    }

    public class GetFileModel
    {
        public string file_name { get; set; }
        public string file_url { get; set; }
        public string file_type { get; set; }
    }
    public class LoginHistoryModel : CommonPaginationModel
    {
        public long login_history_id { get; set; }
        public string user_name { get; set; }
        public string email { get; set; }
        public string Actione_Type { get; set; }
        public string Action { get; set; }
        public DateTime Action_Date { get; set; }
        public string flag { get; set; }

    }
}
