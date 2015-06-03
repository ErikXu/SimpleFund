namespace SimpleFund.Domain
{
    public class FundManagement : FundBase
    {
        /// <summary>
        /// 投资目标
        /// </summary>
        public string InvestmentObjective { get; set; }

        /// <summary>
        /// 托管银行
        /// </summary>
        public string CustodianBank { get; set; }

        /// <summary>
        /// 基金管理公司
        /// </summary>
        public string ManagementCompany { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// 基金管理公司网址
        /// </summary>
        public string ManagementCompanyWebSite { get; set; }

        /// <summary>
        /// 托管银行网址
        /// </summary>
        public string CustodianBankWebSite { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
    }
}
