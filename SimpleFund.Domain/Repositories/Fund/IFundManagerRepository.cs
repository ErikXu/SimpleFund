namespace SimpleFund.Domain.Repositories.Fund
{
    public interface IFundManagerRepository : IRepository<FundManager>
    {
        void RemoveAll();
    }
}
