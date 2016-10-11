using System.Collections.Generic;
using System.Linq;


namespace Redux.Database.Repositories
{
    public interface IRepository<in TKey, TValue>
        where TValue : class
    {
        /// <summary>
        /// Return the persistent instance of the given entity class with the given identifier, obtaining the specified lock mode.
        /// </summary>
        /// <param name="id">A valid identifier of an existing persistent instance of the class</param>
        /// <returns>the persistent instance</returns>
        TValue GetById(TKey id);

        /// <summary>
        /// Persist the given transient instance, first assigning a generated identifier.
        /// </summary>
        /// <param name="obj">A transient instance of a persistent class</param>
        void Add(TValue obj);

        /// <summary>
        /// Update the persistent instance with the identifier of the given transient instance.
        /// </summary>
        /// <param name="obj">A transient instance containing updated state</param>
        void Update(TValue obj);

        /// <summary>
        /// Removes a persistent instance from the datastore.
        /// </summary>
        /// <param name="obj">The instance to be removed</param>
        void Remove(TValue obj);

        /// <summary>
        /// Either <c>Add()</c> or <c>Update()</c> the given instance, depending upon the value of its identifier property.
        /// </summary>
        /// <param name="obj">A transient instance containing new or updated state</param>
        void AddOrUpdate(TValue obj);

        void Add(ICollection<TValue> collection);
        void Update(ICollection<TValue> collection);
        void Remove(ICollection<TValue> collection);
        void AddOrUpdate(ICollection<TValue> collection);

        void Add(IQueryable<TValue> queryable);
        void Update(IQueryable<TValue> queryable);
        void Remove(IQueryable<TValue> queryable);
        void AddOrUpdate(IQueryable<TValue> queryable);
    }
}

