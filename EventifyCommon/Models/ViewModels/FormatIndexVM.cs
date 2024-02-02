using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventifyCommon.Models.ViewModels
{
    public class FormatIndexVM
    {
        public List<Format> Formats { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
        public TrendingFormat TrendingFilter { get; set; }
        public TrendingFormat UnusedFilter { get; set; }
    }
}
