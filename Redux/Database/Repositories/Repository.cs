using System.Collections.Generic;
using System.Linq;
using System;
namespace Redux.Database.Repositories
{
    public class Repository<TKey, TValue> : IRepository<TKey, TValue>
        where TValue : class
    {
        public TValue GetById(TKey id)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session.Get<TValue>(id);
            }
        }

        public void Add(TValue obj)
        {
            try
            {
                using (var session = NHibernateHelper.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        session.Save(obj);
                        transaction.Commit();
                    }
                }
            }
            catch (Exception p) { Console.WriteLine(p); }
        }

        public void Update(TValue obj)
        {
            try
            {
                using (var session = NHibernateHelper.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        session.Update(obj);
                        transaction.Commit();
                    }
                }
            }
            catch (Exception p) { Console.WriteLine(p); }
        }

        public void Remove(TValue obj)
        {
            try
            {
                using (var session = NHibernateHelper.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        session.Delete(obj);
                        transaction.Commit();
                    }
                }
            }
            catch (Exception p) { Console.WriteLine(p); }
        }

        public void AddOrUpdate(TValue obj)
        {
            try
            {
                using (var session = NHibernateHelper.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(obj);
                        transaction.Commit();
                    }
                }
            }
            catch (Exception p) { Console.WriteLine(p); }
        }

        public void Add(ICollection<TValue> collection)
        {
            foreach (var obj in collection)
            {
                Add(obj);
            }
        }

        public void Update(ICollection<TValue> collection)
        {
            foreach (var obj in collection)
            {
                Update(obj);
            }
        }

        public void Remove(ICollection<TValue> collection)
        {
            foreach (var obj in collection)
            {
                Remove(obj);
            }
        }

        public void AddOrUpdate(ICollection<TValue> collection)
        {
            foreach (var obj in collection)
            {
                AddOrUpdate(obj);
            }
        }

        public void Add(IQueryable<TValue> queryable)
        {
            foreach (var obj in queryable)
            {
                Add(obj);
            }
        }

        public void Update(IQueryable<TValue> queryable)
        {
            foreach (var obj in queryable)
            {
                Update(obj);
            }
        }

        public void Remove(IQueryable<TValue> queryable)
        {
            foreach (var obj in queryable)
            {
                Remove(obj);
            }
        }

        public void AddOrUpdate(IQueryable<TValue> queryable)
        {
            foreach (var obj in queryable)
            {
                AddOrUpdate(obj);
            }
        }
    }
}

