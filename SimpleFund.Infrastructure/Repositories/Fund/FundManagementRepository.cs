using SimpleFund.Domain;
using SimpleFund.Domain.Repositories.Fund;
using SimpleFund.Infrastructure.Mongo;

namespace SimpleFund.Infrastructure.Repositories.Fund
{
    public class FundManagementRepository : MongoRepository<FundManagement>, IFundManagementRepository
    {
        public void RemoveAll()
        {
            Collection.RemoveAll();
        }
    }
}
