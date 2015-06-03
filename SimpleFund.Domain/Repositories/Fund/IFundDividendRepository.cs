namespace SimpleFund.Domain.Repositories.Fund
{
    public interface IFundDividendRepository : IRepository<FundDividend>
    {
        void RemoveAll();
    }
}
