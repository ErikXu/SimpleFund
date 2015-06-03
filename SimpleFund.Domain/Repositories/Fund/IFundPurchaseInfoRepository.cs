namespace SimpleFund.Domain.Repositories.Fund
{
    public interface IFundPurchaseInfoRepository : IRepository<FundPurchaseInfo>
    {
        void RemoveAll();
    }
}
