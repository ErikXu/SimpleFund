namespace SimpleFund.Domain.Repositories.Fund
{
    public interface IFundPerformanceRepository : IRepository<FundPerformance>
    {
        void RemoveAll();
    }
}
