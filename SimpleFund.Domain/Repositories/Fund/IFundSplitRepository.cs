namespace SimpleFund.Domain.Repositories.Fund
{
    public interface IFundSplitRepository : IRepository<FundSplit>
    {
        void RemoveAll();
    }
}
