namespace SimpleFund.Domain.Repositories.Fund
{
    public interface IFundManagementRepository : IRepository<FundManagement>
    {
        void RemoveAll();
    }
}
