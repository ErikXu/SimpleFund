using System;

namespace SimpleFund.Domain
{
    public class FundRisk : FundBase
    {
        /// <summary>
        /// 生效日期
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// 评估日期
        /// </summary>
        public DateTime? RatingDate { get; set; }

        /// <summary>
        /// 一年评价
        /// </summary>
        public int? Rating1Y { get; set; }

        /// <summary>
        /// 二年评价
        /// </summary>
        public int? Rating2Y { get; set; }

        /// <summary>
        /// 三年评价
        /// </summary>
        public int? Rating3Y { get; set; }

        /// <summary>
        /// 五年评价
        /// </summary>
        public int? Rating5Y { get; set; }

        /// <summary>
        /// 十年评价
        /// </summary>
        public int? Rating10Y { get; set; }

        /// <summary>
        /// 平均回报（%）-3年
        /// </summary>
        public double? AverageReturn3Y { get; set; }

        /// <summary>
        /// 平均回报（%）-5年
        /// </summary>
        public double? AverageReturn5Y { get; set; }

        /// <summary>
        /// 平均回报（%）-10年
        /// </summary>
        public double? AverageReturn10Y { get; set; }

        /// <summary>
        /// 标准差-3年
        /// </summary>
        public double? StandardDeviation3Y { get; set; }

        /// <summary>
        /// 标准差-5年
        /// </summary>
        public double? StandardDeviation5Y { get; set; }

        /// <summary>
        /// 标准差-10年
        /// </summary>
        public double? StandardDeviation10Y { get; set; }

        /// <summary>
        /// 晨星风险系数-3年
        /// </summary>
        public double? MSRiskFactor3Y { get; set; }

        /// <summary>
        /// 晨星风险系数-5年
        /// </summary>
        public double? MSRiskFactor5Y { get; set; }

        /// <summary>
        /// 晨星风险系数-10年
        /// </summary>
        public double? MSRiskFactor10Y { get; set; }

        /// <summary>
        /// 夏普比率3年
        /// </summary>
        public double? SharpeRatio3Y { get; set; }

        /// <summary>
        /// 夏普比率5年
        /// </summary>
        public double? SharpeRatio5Y { get; set; }

        /// <summary>
        /// 夏普比率10年
        /// </summary>
        public double? SharpeRatio10Y { get; set; }

        /// <summary>
        /// 阿尔法系数-3年（+/-基准指数）
        /// </summary>
        public double? AlphaToIndex3Y { get; set; }

        /// <summary>
        /// 阿尔法系数-3年（+/-同类平均）
        /// </summary>
        public double? AlphaToCategory3Y { get; set; }

        /// <summary>
        /// 贝塔系数-3年（+/-基准指数）
        /// </summary>
        public double? BetaToIndex3Y { get; set; }

        /// <summary>
        /// 贝塔系数-3年（+/-同类平均）
        /// </summary>
        public double? BetaToCategory3Y { get; set; }

        /// <summary>
        /// R平方-3年（+/-基准指数）
        /// </summary>
        public double? RSquareToIndex3Y { get; set; }

        /// <summary>
        /// R平方-3年（+/-同类平均）
        /// </summary>
        public double? RSquareToCategory3Y { get; set; }
    }
}
