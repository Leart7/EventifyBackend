using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventifyCommon.Models.ViewModels
{
    public class LanguageIndexVM
    {
        public List<Language> Languages { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
        public TrendingLanguage TrendingFilter { get; set; }
        public TrendingLanguage UnusedFilter { get; set; }

    }
}
