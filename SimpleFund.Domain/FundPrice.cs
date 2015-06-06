using System;

namespace SimpleFund.Domain
{
    public class FundPrice : FundBase
    {
        /// <summary>
        /// 净值日期
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// 昨收
        /// </summary>
        public double ClosePrice { get; set; }

        /// <summary>
        /// 累计净值
        /// </summary>
        public double? AccNav { get; set; }
    }
}
