using SimpleFund.Domain;
using SimpleFund.Domain.Repositories.Fund;
using SimpleFund.Infrastructure.Mongo;

namespace SimpleFund.Infrastructure.Repositories.Fund
{
    public class FundOperationRepository : MongoRepository<FundOperation>, IFundOperationRepository
    {
        public void RemoveAll()
        {
            Collection.RemoveAll();
        }
    }
}
