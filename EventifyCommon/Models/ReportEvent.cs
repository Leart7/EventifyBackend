using EventifyCommon.Models.AbstractModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventifyCommon.Models
{
    public class ReportEvent : BaseModel
    {
        public int ReportEventReasonId { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public int EventId { get; set; }
        public bool Reviewed { get; set; }
        public ReportEventReason ReportEventReason { get; set; }
        public Event Event { get; set; }
    }
}
