using SimpleFund.Domain;
using SimpleFund.Domain.Repositories.Fund;
using SimpleFund.Infrastructure.Mongo;

namespace SimpleFund.Infrastructure.Repositories.Fund
{
    public class FundSplitRepository : MongoRepository<FundSplit>, IFundSplitRepository
    {
        public void RemoveAll()
        {
            Collection.RemoveAll();
        }
    }
}
