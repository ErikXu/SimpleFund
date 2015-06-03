using SimpleFund.Domain;
using SimpleFund.Domain.Repositories.Fund;
using SimpleFund.Infrastructure.Mongo;

namespace SimpleFund.Infrastructure.Repositories.Fund
{
    public class FundRiskRepository : MongoRepository<FundRisk>, IFundRiskRepository
    {
        public void RemoveAll()
        {
            Collection.RemoveAll();
        }
    }
}
