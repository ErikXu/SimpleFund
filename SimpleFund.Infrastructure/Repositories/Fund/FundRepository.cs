using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using SimpleFund.Domain.Repositories.Fund;
using SimpleFund.Infrastructure.Mongo;

namespace SimpleFund.Infrastructure.Repositories.Fund
{
    public class FundRepository : MongoRepository<Domain.Fund>, IFundRepository
    {
        public void RemoveAll()
        {
            Collection.RemoveAll();
        }

        public void SetOpenFundsFlag(IEnumerable<string> symbols)
        {
            var query = Query<Domain.Fund>.Where(n => symbols.Contains(n.Symbol));
            var update = Update<Domain.Fund>.Set(n => n.IsOpenFund, true);
            Collection.Update(query, update, UpdateFlags.Multi);
        }

        public void SetCloseFundsFlag(IEnumerable<string> symbols)
        {
            var query = Query<Domain.Fund>.Where(n => symbols.Contains(n.Symbol));
            var update = Update<Domain.Fund>.Set(n => n.IsCloseFund, true);
            Collection.Update(query, update, UpdateFlags.Multi);
        }

        public void SetQDIIFundsFlag(IEnumerable<string> symbols)
        {
            var query = Query<Domain.Fund>.Where(n => symbols.Contains(n.Symbol));
            var update = Update<Domain.Fund>.Set(n => n.IsQDIIFund, true);
            Collection.Update(query, update, UpdateFlags.Multi);
        }

        public void SetETFFundsFlag(IEnumerable<string> symbols)
        {
            var query = Query<Domain.Fund>.Where(n => symbols.Contains(n.Symbol));
            var update = Update<Domain.Fund>.Set(n => n.IsETFFund, true);
            Collection.Update(query, update, UpdateFlags.Multi);
        }

        public void SetLOFFundsFlag(IEnumerable<string> symbols)
        {
            var query = Query<Domain.Fund>.Where(n => symbols.Contains(n.Symbol));
            var update = Update<Domain.Fund>.Set(n => n.IsLOFFund, true);
            Collection.Update(query, update, UpdateFlags.Multi);
        }

        public void SetActiveManagementFundsFlag(IEnumerable<string> symbols)
        {
            var query = Query<Domain.Fund>.Where(n => symbols.Contains(n.Symbol));
            var update = Update<Domain.Fund>.Set(n => n.IsActiveManagementFund, true);
            Collection.Update(query, update, UpdateFlags.Multi);
        }

        public void SetIndexFundsFlag(IEnumerable<string> symbols)
        {
            var query = Query<Domain.Fund>.Where(n => symbols.Contains(n.Symbol));
            var update = Update<Domain.Fund>.Set(n => n.IsIndexFund, true);
            Collection.Update(query, update, UpdateFlags.Multi);
        }

        public void SetIndexEnhancedFundsFlag(IEnumerable<string> symbols)
        {
            var query = Query<Domain.Fund>.Where(n => symbols.Contains(n.Symbol));
            var update = Update<Domain.Fund>.Set(n => n.IsIndexEnhancedFund, true);
            Collection.Update(query, update, UpdateFlags.Multi);
        }

        public void SetSmallCapFundsFlag(IEnumerable<string> symbols)
        {
            var query = Query<Domain.Fund>.Where(n => symbols.Contains(n.Symbol));
            var update = Update<Domain.Fund>.Set(n => n.IsSmallCapFund, true);
            Collection.Update(query, update, UpdateFlags.Multi);
        }
    }
}
