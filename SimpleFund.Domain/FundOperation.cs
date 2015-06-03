using System;

namespace SimpleFund.Domain
{
    public class FundOperation : FundBase
    {
        /// <summary>
        /// 基金分类
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 基金分类Id
        /// </summary>
        public string CategoryId { get; set; }
        
        /// <summary>
        /// 成立日期
        /// </summary>
        public DateTime? InceptionDate { get; set; }

        /// <summary>
        /// 开放日期
        /// </summary>
        public DateTime? OpenDate { get; set; }

        /// <summary>
        /// 上市日期
        /// </summary>
        public DateTime? TradingDate { get; set; }

        /// <summary>
        /// 申购状态
        /// </summary>
        public string Subscribe { get; set; }

        /// <summary>
        /// 赎回状态
        /// </summary>
        public string Redeem { get; set; }

        /// <summary>
        /// 股票投资风格箱
        /// </summary>
        public string Box { get; set; }

        /// <summary>
        /// 总净资产(亿元)
        /// </summary>
        public double? FundTotalNetAsset { get; set; }

        /// <summary>
        /// 最低投资额
        /// </summary>
        public double? MinimumInvestment { get; set; }

        /// <summary>
        /// 上市交易所
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// 前端收费
        /// </summary>
        public string FrontFee { get; set; }

        /// <summary>
        /// 前端收费数值
        /// </summary>
        public double? FrontFeeValue { get; set; }

        /// <summary>
        /// 前端收费单位
        /// </summary>
        public string FrontFeeUnit { get; set; }

        /// <summary>
        /// 后端收费
        /// </summary>
        public string DeferFee { get; set; }

        /// <summary>
        /// 后端收费数值
        /// </summary>
        public double? DeferFeeValue { get; set; }

        /// <summary>
        /// 后端收费单位
        /// </summary>
        public string DeferFeeUnit { get; set; }
    }
}
