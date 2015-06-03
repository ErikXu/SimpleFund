using System;

namespace SimpleFund.Domain
{
    public class FundDividend : FundBase
    {
        /// <summary>
        /// 除息日
        /// </summary>
        public DateTime ExcludingDate { get; set; }

        /// <summary>
        /// 再投资日
        /// </summary>
        public DateTime ReinvestDate { get; set; }

        /// <summary>
        /// 分红（Regular:元/10份,Daily:元/万份）
        /// </summary>
        public double TotalDistribution { get; set; }

        /// <summary>
        /// 再投资日净值（元）
        /// </summary>
        public double ReinvestPrice { get; set; }

    }
}
