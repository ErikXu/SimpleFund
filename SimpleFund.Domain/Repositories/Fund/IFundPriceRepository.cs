namespace SimpleFund.Domain.Repositories.Fund
{
    public interface IFundPriceRepository : IRepository<FundPrice>
    {
        void RemoveAll();
    }
}
