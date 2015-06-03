namespace SimpleFund.Domain.Repositories.Fund
{
    public interface IFundWorstReturnRepository : IRepository<FundWorstReturn>
    {
        void RemoveAll();
    }
}
