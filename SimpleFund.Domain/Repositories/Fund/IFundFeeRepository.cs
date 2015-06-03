namespace SimpleFund.Domain.Repositories.Fund
{
    public interface IFundFeeRepository : IRepository<FundFee>
    {
        void RemoveAll();
    }
}
