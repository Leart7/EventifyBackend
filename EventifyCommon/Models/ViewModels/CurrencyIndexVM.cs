using System;

namespace EventifyCommon.Models.ViewModels
{
    public class CurrencyIndexVM
    {
        public List<Currency> Currencies { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
        public TrendingCurrency TrendingFilter { get; set; }
        public TrendingCurrency UnusedFilter { get; set; }

    }
}
