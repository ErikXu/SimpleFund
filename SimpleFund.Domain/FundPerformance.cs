namespace SimpleFund.Domain
{
    public class FundPerformance : FundBase
    {
        /// <summary>
        /// 业绩年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 年总回报
        /// </summary>
        public double? ReturnYear { get; set; }

        /// <summary>
        /// 年总回报（+/-基准指数）
        /// </summary>
        public double? ReturnYearToIndex { get; set; }

        /// <summary>
        /// 年总回报（+/-同类平均）
        /// </summary>
        public double? ReturnYearToCategory { get; set; }

        /// <summary>
        /// 一季度总回报
        /// </summary>
        public double? ReturnQ1 { get; set; }

        /// <summary>
        /// 一季度总回报（+/-基准指数）
        /// </summary>
        public double? ReturnQ1ToIndex { get; set; }

        /// <summary>
        /// 一季度总回报（+/-同类平均）
        /// </summary>
        public double? ReturnQ1ToCategory { get; set; }

        /// <summary>
        /// 二季度总回报
        /// </summary>
        public double? ReturnQ2 { get; set; }

        /// <summary>
        /// 二季度总回报（+/-基准指数）
        /// </summary>
        public double? ReturnQ2ToIndex { get; set; }

        /// <summary>
        /// 二季度总回报（+/-同类平均）
        /// </summary>
        public double? ReturnQ2ToCategory { get; set; }

        /// <summary>
        /// 三季度总回报
        /// </summary>
        public double? ReturnQ3 { get; set; }

        /// <summary>
        /// 三季度总回报（+/-基准指数）
        /// </summary>
        public double? ReturnQ3ToIndex { get; set; }

        /// <summary>
        /// 三季度总回报（+/-同类平均）
        /// </summary>
        public double? ReturnQ3ToCategory { get; set; }

        /// <summary>
        /// 四季度总回报
        /// </summary>
        public double? ReturnQ4 { get; set; }

        /// <summary>
        /// 四季度总回报（+/-基准指数）
        /// </summary>
        public double? ReturnQ4ToIndex { get; set; }

        /// <summary>
        /// 四季度总回报（+/-同类平均）
        /// </summary>
        public double? ReturnQ4ToCategory { get; set; }
    }
}
