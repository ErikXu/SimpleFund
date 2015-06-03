using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;

namespace SimpleFund.Domain.Repositories
{
    public interface IRepositoryWithTypedId<T, in TId> where T : EntityWithTypedId<TId>
    {
        T Get(TId id);

        /// <summary>
        /// <![CDATA[Returns an instance of type IQueryable<T>]]>
        /// </summary>
        /// <remarks>
        /// http://devlicio.us/blogs/billy_mccafferty/archive/2011/11/11/s-arp-lite-the-basicss.aspx
        /// For performance reasons, it's obviously better to put as much filtering work on the shoulders of the database.
        /// Before LINQ providers, it was difficult to find the appropriate balance between filtering on the domain side or filtering on the database side.
        /// But with LINQ and IQueryable, filtering can be developed within the domain while it's still executed on the database.
        /// 
        /// Note: Don't make up complicated query because the underlying implementation may not support it. 
        /// Read "IQueryable is Tight Coupling"(http://blog.ploeh.dk/2012/03/26/IQueryableIsTightCoupling.aspx) for more information.
        /// </remarks>
        /// <returns></returns>
        IQueryable<T> AsQueryable();

        void Insert(T entity);
        void InsertBatch(IEnumerable<T> entities);
        T Save(T entity);
        void Delete(TId id);
    }

    public interface IRepository<T> : IRepositoryWithTypedId<T, ObjectId> where T : Entity
    {

    }
}
