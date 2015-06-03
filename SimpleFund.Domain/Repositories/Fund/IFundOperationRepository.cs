namespace SimpleFund.Domain.Repositories.Fund
{
    public interface IFundOperationRepository : IRepository<FundOperation>
    {
        void RemoveAll();
    }
}
