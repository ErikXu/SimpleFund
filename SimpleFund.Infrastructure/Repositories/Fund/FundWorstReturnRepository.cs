using SimpleFund.Domain;
using SimpleFund.Domain.Repositories.Fund;
using SimpleFund.Infrastructure.Mongo;

namespace SimpleFund.Infrastructure.Repositories.Fund
{
    public class FundWorstReturnRepository : MongoRepository<FundWorstReturn>, IFundWorstReturnRepository
    {
        public void RemoveAll()
        {
            Collection.RemoveAll();
        }
    }
}
