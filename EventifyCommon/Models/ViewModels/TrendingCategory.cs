using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventifyCommon.Models.ViewModels
{
    public class TrendingCategory
    {
        public Category MostTrendingFilter { get; set; }
        public int NumberOfEvents { get; set; }
    }
}
