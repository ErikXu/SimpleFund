using SimpleFund.Domain;
using SimpleFund.Domain.Repositories.Fund;
using SimpleFund.Infrastructure.Mongo;

namespace SimpleFund.Infrastructure.Repositories.Fund
{
    public class FundPriceRepository : MongoRepository<FundPrice>, IFundPriceRepository
    {
        public void RemoveAll()
        {
            Collection.RemoveAll();
        }
    }
}
