using System;

namespace SimpleFund.Domain
{
    public class FundReturn : FundBase
    {
        public ReturnType Type { get; set; }

        /// <summary>
        /// 生效日期
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// 今年以来业绩总回报
        /// </summary>
        public double? ReturnYTD { get; set; }

        /// <summary>
        /// 今年以来业绩总回报+/-基准指数
        /// </summary>
        public double? ReturnToIndYTD { get; set; }

        /// <summary>
        /// 今年以来业绩总回报+/-同类平均
        /// </summary>
        public double? ReturnToCatYTD { get; set; }

        /// <summary>
        /// 一个月业绩总回报
        /// </summary>
        public double? Return1M { get; set; }

        /// <summary>
        /// 一个月业绩总回报+/-基准指数
        /// </summary>
        public double? ReturnToInd1M { get; set; }

        /// <summary>
        /// 一个月业绩总回报+/-同类平均
        /// </summary>
        public double? ReturnToCat1M { get; set; }

        /// <summary>
        /// 三个月业绩总回报
        /// </summary>
        public double? Return3M { get; set; }

        /// <summary>
        /// 三个月业绩总回报+/-基准指数
        /// </summary>
        public double? ReturnToInd3M { get; set; }

        /// <summary>
        /// 三个月业绩总回报+/-同类平均
        /// </summary>
        public double? ReturnToCat3M { get; set; }

        /// <summary>
        /// 六个月业绩总回报
        /// </summary>
        public double? Return6M { get; set; }

        /// <summary>
        /// 六个月业绩总回报+/-基准指数
        /// </summary>
        public double? ReturnToInd6M { get; set; }

        /// <summary>
        /// 六个月业绩总回报+/-同类平均
        /// </summary>
        public double? ReturnToCat6M { get; set; }

        /// <summary>
        /// 一年业绩总回报
        /// </summary>
        public double? Return1Y { get; set; }

        /// <summary>
        /// 一年业绩总回报+/-基准指数
        /// </summary>
        public double? ReturnToInd1Y { get; set; }

        /// <summary>
        /// 一年业绩总回报+/-同类平均
        /// </summary>
        public double? ReturnToCat1Y { get; set; }

        /// <summary>
        /// 二年业绩总回报
        /// </summary>
        public double? Return2Y { get; set; }

        /// <summary>
        /// 二年业绩总回报+/-基准指数
        /// </summary>
        public double? ReturnToInd2Y { get; set; }

        /// <summary>
        /// 二年业绩总回报+/-同类平均
        /// </summary>
        public double? ReturnToCat2Y { get; set; }

        /// <summary>
        /// 三年业绩总回报
        /// </summary>
        public double? Return3Y { get; set; }

        /// <summary>
        /// 三年业绩总回报+/-基准指数
        /// </summary>
        public double? ReturnToInd3Y { get; set; }

        /// <summary>
        /// 三年业绩总回报+/-同类平均
        /// </summary>
        public double? ReturnToCat3Y { get; set; }

        /// <summary>
        /// 五年业绩总回报
        /// </summary>
        public double? Return5Y { get; set; }

        /// <summary>
        /// 五年业绩总回报+/-基准指数
        /// </summary>
        public double? ReturnToInd5Y { get; set; }

        /// <summary>
        /// 五年业绩总回报+/-同类平均
        /// </summary>
        public double? ReturnToCat5Y { get; set; }

        /// <summary>
        /// 十年业绩总回报
        /// </summary>
        public double? Return10Y { get; set; }

        /// <summary>
        /// 十年业绩总回报+/-基准指数
        /// </summary>
        public double? ReturnToInd10Y { get; set; }

        /// <summary>
        /// 十年业绩总回报+/-同类平均
        /// </summary>
        public double? ReturnToCat10Y { get; set; }
    }

    public enum ReturnType
    {
        Current,
        MonthEnd
    }
}
