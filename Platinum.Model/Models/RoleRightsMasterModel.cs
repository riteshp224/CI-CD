using System;
using System.Collections.Generic;
using System.Text;

namespace Platinum.Model.Models
{
    public class RoleRightMasterModel : RoleRightsMasterModel
    {
        public long roleId { get; set; }
        public List<RoleRightsMasterModel> RoleRightsMasterModel { get; set; }
    }
    public class RoleRightsMasterModel
    {
        public long rightsId { get; set; }
        public long roleId { get; set; }
        public long menuId { get; set; }
        public string menuName { get; set; }
        public long userId { get; set; }
        public bool? isAdd { get; set; }
        public bool? isEdit { get; set; }
        public bool? isDelete { get; set; }
        public bool? isView { get; set; }
        public long? createdBy { get; set; }
        public DateTime? createdOn { get; set; }
        public long? updatedBy { get; set; }
        public DateTime? updatedOn { get; set; }
    }
}
