using SimpleFund.Domain;
using SimpleFund.Domain.Repositories.Fund;
using SimpleFund.Infrastructure.Mongo;

namespace SimpleFund.Infrastructure.Repositories.Fund
{
    public class FundManagerRepository : MongoRepository<FundManager>, IFundManagerRepository
    {
        public void RemoveAll()
        {
            Collection.RemoveAll();
        }
    }
}
