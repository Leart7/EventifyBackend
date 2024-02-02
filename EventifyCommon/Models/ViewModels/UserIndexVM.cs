using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventifyCommon.Models.ViewModels
{
    public class UserIndexVM
    {
        public List<IdentityUser> Users { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
        public string EmailSortOrder { get; set; }
        public string FirstNameSortOrder { get; set; }
        public string LastNameSortOrder { get; set; }
        public string EventsSortOrder { get; set; }
        public string FollowersSortOrder { get; set; }
        public string ReportsSortOrder { get; set; }

    }
}
