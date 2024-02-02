using EventifyCommon.Models.AbstractModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventifyCommon.Models
{
    public class Permission : BaseModel
    {
        public string UserId { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool Allowed { get; set; }

        public ApplicationUser User { get; set; }
    }
}
