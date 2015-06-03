using SimpleFund.Domain;
using SimpleFund.Domain.Repositories.Fund;
using SimpleFund.Infrastructure.Mongo;

namespace SimpleFund.Infrastructure.Repositories.Fund
{
    public class FundDividendRepository : MongoRepository<FundDividend>, IFundDividendRepository
    {
        public void RemoveAll()
        {
            Collection.RemoveAll();
        }
    }
}
