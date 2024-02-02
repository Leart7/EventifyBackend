using EventifyCommon.Models.AbstractModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventifyCommon.Models
{
    public class ClosedAccount : BaseModel
    {
        public int ClosedAccountReasonId { get; set; }
        public string? Description { get; set; }

        public ClosedAccountReason ClosedAccountReason { get; set; }
    }
}
