using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventifyCommon.Models.ViewModels
{
    public class CategoryIndexVM
    {
        public List<Category> Categories { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
        public TrendingCategory TrendingFilter { get; set; }
        public TrendingCategory UnusedFilter { get; set; }

    }
}
