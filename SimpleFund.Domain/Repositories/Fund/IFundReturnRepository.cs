namespace SimpleFund.Domain.Repositories.Fund
{
    public interface IFundReturnRepository : IRepository<FundReturn>
    {
        void RemoveAll();
    }
}
