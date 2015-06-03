using SimpleFund.Domain;
using SimpleFund.Domain.Repositories.Fund;
using SimpleFund.Infrastructure.Mongo;

namespace SimpleFund.Infrastructure.Repositories.Fund
{
    public class FundFeeRepository : MongoRepository<FundFee>, IFundFeeRepository
    {
        public void RemoveAll()
        {
            Collection.RemoveAll();
        }
    }
}
