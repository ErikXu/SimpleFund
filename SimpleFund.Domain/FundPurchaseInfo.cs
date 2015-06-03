using System;

namespace SimpleFund.Domain
{
    public class FundPurchaseInfo : FundBase
    {
        /// <summary>
        /// 成立日期
        /// </summary>
        public DateTime? InceptionDate { get; set; }

        /// <summary>
        /// 申购状态
        /// </summary>
        public string PurchaseStatus { get; set; }

        /// <summary>
        /// 赎回状态
        /// </summary>
        public string RedemptionStatus { get; set; }

        /// <summary>
        /// 前端收费
        /// </summary>
        public double? FrontFee { get; set; }

        /// <summary>
        /// 后端收费
        /// </summary>
        public double? DeferFee { get; set; }

        /// <summary>
        /// 赎回费
        /// </summary>
        public double? RedemptionFee { get; set; }

        /// <summary>
        /// 管理费
        /// </summary>
        public double? ManagementFee { get; set; }

        /// <summary>
        /// 托管费
        /// </summary>
        public double? CustodianFee { get; set; }

        /// <summary>
        /// 销售服务费
        /// </summary>
        public double? DistributionFee { get; set; }
    }
}
