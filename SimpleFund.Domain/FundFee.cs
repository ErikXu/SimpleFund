namespace SimpleFund.Domain
{
    public class FundFee : FundBase
    {
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
