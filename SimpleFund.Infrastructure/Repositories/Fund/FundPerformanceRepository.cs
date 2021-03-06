﻿using SimpleFund.Domain;
using SimpleFund.Domain.Repositories.Fund;
using SimpleFund.Infrastructure.Mongo;

namespace SimpleFund.Infrastructure.Repositories.Fund
{
    public class FundPerformanceRepository : MongoRepository<FundPerformance>, IFundPerformanceRepository
    {
        public void RemoveAll()
        {
            Collection.RemoveAll();
        }
    }
}
