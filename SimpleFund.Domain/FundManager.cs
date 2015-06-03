using System;

namespace SimpleFund.Domain
{
    public class FundManager : FundBase
    {
        /// <summary>
        /// 基金经理Id
        /// </summary>
        public string ManagerId { get; set; }

        /// <summary>
        /// 基金经理名称
        /// </summary>
        public string ManagerName { get; set; }

        /// <summary>
        /// 是否离任
        /// </summary>
        public bool? IsLeave { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 管理时间
        /// </summary>
        public string ManagementTime { get; set; }

        /// <summary>
        /// 管理时间年数
        /// </summary>
        public int? ManagementYears { get; set; }

        /// <summary>
        /// 管理时间天数
        /// </summary>
        public int? ManagementDays { get; set; }
    }
}
