namespace SimpleFund.Domain
{
    public class FundWorstReturn : FundBase
    {
        /// <summary>
        /// 最差三个月回报
        /// </summary>
        public double? WorstReturn3M { get; set; }

        /// <summary>
        /// 最差六个月回报
        /// </summary>
        public double? WorstReturn6M { get; set; }
    }
}
