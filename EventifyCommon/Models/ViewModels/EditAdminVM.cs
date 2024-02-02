using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventifyCommon.Models.ViewModels
{
    public class EditAdminVM
    {
        public IdentityUser User { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}
