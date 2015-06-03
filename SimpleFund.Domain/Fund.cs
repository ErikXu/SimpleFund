using System;

namespace SimpleFund.Domain
{
    public class Fund : FundBase
    {
        /// <summary>
        /// 基金分类
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 成立日期
        /// </summary>
        public DateTime? InceptionDate { get; set; }

        /// <summary>
        /// 是否为开放式基金
        /// </summary>
        public bool IsOpenFund { get; set; }

        /// <summary>
        /// 是否为封闭式基金
        /// </summary>
        public bool IsCloseFund { get; set; }

        /// <summary>
        /// 是否为QDII基金
        /// </summary>
        public bool IsQDIIFund { get; set; }

        /// <summary>
        /// 是否为ETF基金
        /// </summary>
        public bool IsETFFund { get; set; }

        /// <summary>
        /// 是否为LOF基金
        /// </summary>
        public bool IsLOFFund { get; set; }

        /// <summary>
        /// 是否为主动管理型基金
        /// </summary>
        public bool IsActiveManagementFund { get; set; }

        /// <summary>
        /// 是否为指数型基金
        /// </summary>
        public bool IsIndexFund { get; set; }

        /// <summary>
        /// 是否为指数增强型基金
        /// </summary>
        public bool IsIndexEnhancedFund { get; set; }

        /// <summary>
        /// 是否为中小盘基金
        /// </summary>
        public bool IsSmallCapFund { get; set; }

        /// <summary>
        /// 净值日期
        /// </summary>
        public DateTime? PriceDate { get; set; }

        /// <summary>
        /// 单位净值(元)
        /// </summary>
        public double? Price { get; set; }

        /// <summary>
        /// 净值日变动(元)
        /// </summary>
        public double? PriceChange { get; set; }

        /// <summary>
        /// 今年以来回报(%)
        /// </summary>
        public double? ReturnYtd { get; set; }
    }
}
