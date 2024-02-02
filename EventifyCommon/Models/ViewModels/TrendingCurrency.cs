using System;

namespace EventifyCommon.Models.ViewModels
{
    public class TrendingCurrency
    {
        public Currency MostTrendingFilter { get; set; }
        public int NumberOfEvents { get; set; }
    }
}