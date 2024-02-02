using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventifyCommon.Models.ViewModels
{
    public class TypeIndexVM
    {
        public List<Type> Types { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
        public TrendingType TrendingFilter { get; set; }
        public TrendingType UnusedFilter { get; set; }
    }
}
