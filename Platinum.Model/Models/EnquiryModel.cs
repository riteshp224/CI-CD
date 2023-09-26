using System;
using System.Collections.Generic;
using System.Text;

namespace Platinum.Model.Models
{
    public class EnquiryModel : CommonModel
    {
        public long EnquiryId { get; set; }
        public string CustomerName { get; set; }
        public long StatusId { get; set; }
        public string StatusName { get; set; }
        public string OrderNo { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string Message { get; set; }
        public string ImageURL { get; set; }
        public string strQty { get; set; }
        public decimal Cost { get; set; }
        public string Date { get; set; }
        public int ListImgCount { get; set; }
        public List<EnquiryDetailModel> EnquiryDetails { get; set; }
    }
    public class EnquiryDetailModel : CommonModel
    {
        public long EnquiryDetailId { get; set; }
        public long EnquiryId { get; set; }
        public long ServiceTypeId { get; set; }
        public string ServiceName { get; set; }
       
        public string ParentServiceTypeName { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Qty { get; set; }
        public string ServiceTypePhoto { get; set; }
        public string ImageURL { get; set; }
    }
}
