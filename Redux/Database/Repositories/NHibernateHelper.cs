using System;
using NHibernate;
namespace Redux.Database.Repositories
{
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        public static bool BuildSessionFactory()
        {
            if (_sessionFactory != null) return false;

            var configuration = new NHibernate.Cfg.Configuration();
            configuration.Configure(Environment.MachineName + ".cfg.xml");
            configuration.AddAssembly(System.Reflection.Assembly.GetExecutingAssembly());

            _sessionFactory = configuration.BuildSessionFactory();

            return true;
        }

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    BuildSessionFactory();
                }

                return _sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
