using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventifyCommon.Models.ViewModels
{
    public class ReportIndexVM
    {
        public List<ReportEvent> Reports { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
    }
}
