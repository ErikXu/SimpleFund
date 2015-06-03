using System;

namespace SimpleFund.Domain
{
    public class FundSplit : FundBase
    {
        /// <summary>
        /// 有效日期
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// 拆分比率
        /// </summary>
        public double Ratio { get; set; }
    }
}
