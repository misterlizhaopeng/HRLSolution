using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRL.Model
{
    [MetadataType(typeof(Base_UserInfoMetadata))]
    public partial class Base_UserInfo : Pager
    {
        public List<string> RoleIds { get; set; }
        public List<Base_RolesInfo> RoleList { get; set; }
    }
    class Base_UserInfoMetadata 
    {
        [MinLength(36), MaxLength(36)]
        public string Id { get; set; }
        [Required]
        [MaxLength(128)]
        public string UserName { get; set; }
        [Required]
        public string UserPwd { get; set; }
        public string Email { get; set; }
        public Nullable<int> DelFlag { get; set; }
        public string Phone { get; set; }
        public string Addrs { get; set; }

        public Nullable<System.DateTime> BirthDay { get; set; }
        public string DeptId { get; set; }
        [MaxLength(256)]
        public string Bz { get; set; }
    }

}
