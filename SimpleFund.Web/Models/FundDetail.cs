using System;
using System.Collections.Generic;
using SimpleFund.Domain;

namespace SimpleFund.Web.Models
{
    public class FundDetail
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public DateTime? PriceDate { get; set; }
        public double? Price { get; set; }
        public double? PriceChange { get; set; }
        public double? PriceChangeRatio { get; set; }

        public string Category { get; set; }
        public DateTime? InceptionDate { get; set; }
        public DateTime? OpenDate { get; set; }
        public DateTime? TradingDate { get; set; }
        public string Subscribe { get; set; }
        public string Redeem { get; set; }
        public string Box { get; set; }
        public double? FundTotalNetAsset { get; set; }
        public double? MinimumInvestment { get; set; }
        public string Exchange { get; set; }
        public string FrontFee { get; set; }
        public string DeferFee { get; set; }

        public IEnumerable<FundPerformance> Performances { get; set; }

        public FundReturn CurrentReturn { get; set; }

        public FundReturn MonthEndReturn { get; set; }

        public FundWorstReturn WorstReturn { get; set; }

        public FundRisk Risk { get; set; }
    }
}