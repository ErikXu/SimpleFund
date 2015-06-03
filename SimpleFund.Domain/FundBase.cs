namespace SimpleFund.Domain
{
    public abstract class FundBase : Entity
    {
        /// <summary>
        /// 基金代码
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 基金Id
        /// </summary>
        public string ShareClassId { get; set; }

        /// <summary>
        /// 基金名称
        /// </summary>
        public string Name { get; set; }
    }
}
