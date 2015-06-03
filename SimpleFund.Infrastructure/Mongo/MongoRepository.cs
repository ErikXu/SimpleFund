using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using SimpleFund.Domain;
using SimpleFund.Domain.Repositories;

namespace SimpleFund.Infrastructure.Mongo
{
    public class MongoRepositoryWithTypedId<T, TId> : MongoBase, IRepositoryWithTypedId<T, TId> where T : EntityWithTypedId<TId>
    {
        /// <summary>
        /// MongoCollection field
        /// </summary>
        private readonly MongoCollection<T> _collection;

        /// <summary>
        /// Initilizes the instance of MongoRepository, Setups the MongoDB and The Collection (i.e T)
        /// Uses the Default App.Config tag names to fetch the connectionString and Database name
        /// Check the MongoSetting class for the App.Config tag names.
        /// </summary>
        public MongoRepositoryWithTypedId()
        {
            var db = Init();
            var collectionName = InferCollectionNameFrom();
            _collection = db.GetCollection<T>(collectionName);
        }

        private string InferCollectionNameFrom()
        {
            var type = typeof(T);
            return type.Name;
        }

        /// <summary>
        /// Gets the Mongo collection (to perform advance operations)
        /// </summary>
        protected internal MongoCollection<T> Collection
        {
            get { return _collection; }
        }

        #region IRepositoryWithTypedId<T,TId> Members

        public virtual T Get(TId id)
        {
            return Collection.FindOneById(BsonValue.Create(id));
        }

        public void Insert(T entity)
        {
            Collection.Insert(entity);
        }

        public void InsertBatch(IEnumerable<T> entities)
        {
            Collection.InsertBatch(entities);
        }

        public T Save(T entity)
        {
            Collection.Save(entity);
            return entity;
        }

        public virtual void Delete(TId id)
        {
            Collection.Remove(Query<T>.EQ(t => t.Id, id));
        }

        public virtual IQueryable<T> AsQueryable()
        {
            return Collection.AsQueryable();
        }

        #endregion
    }

    public class MongoRepository<T> : MongoRepositoryWithTypedId<T, ObjectId>, IRepository<T> where T : Entity
    {

    }
}
