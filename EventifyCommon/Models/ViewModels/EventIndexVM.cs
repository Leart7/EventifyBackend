using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventifyCommon.Models.ViewModels
{
    public class EventIndexVM
    {
        public List<Event> Events { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
    }
}
