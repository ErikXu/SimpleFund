using SimpleFund.Domain;
using SimpleFund.Domain.Repositories.Fund;
using SimpleFund.Infrastructure.Mongo;

namespace SimpleFund.Infrastructure.Repositories.Fund
{
    public class FundPurchaseInfoRepository : MongoRepository<FundPurchaseInfo>, IFundPurchaseInfoRepository
    {
        public void RemoveAll()
        {
            Collection.RemoveAll();
        }
    }
}
