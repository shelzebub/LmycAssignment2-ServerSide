using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace LmycWeb.Models
{
    public class RoleViewModel
    {
        public string RoleId { get; set; }
        [DisplayName("Role Name")]
        public string RoleName { get; set; }

        public string UserId { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}
