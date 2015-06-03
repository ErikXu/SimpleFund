namespace SimpleFund.Domain.Repositories.Fund
{
    public interface IFundRiskRepository : IRepository<FundRisk>
    {
        void RemoveAll();
    }
}
