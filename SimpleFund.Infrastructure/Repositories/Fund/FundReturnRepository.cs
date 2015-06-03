using SimpleFund.Domain;
using SimpleFund.Domain.Repositories.Fund;
using SimpleFund.Infrastructure.Mongo;

namespace SimpleFund.Infrastructure.Repositories.Fund
{
    public class FundReturnRepository : MongoRepository<FundReturn>, IFundReturnRepository
    {
        public void RemoveAll()
        {
            Collection.RemoveAll();
        }
    }
}
