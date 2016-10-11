using Redux.Database.Repositories;

namespace Redux.Database
{
    public class ServerDatabase
    {
        public static ConquerDataContext Context { get; private set; }

        public static bool InitializeSql()
        {
            Context = new ConquerDataContext();
            NHibernateHelper.BuildSessionFactory();

            Context.Accounts.ResetLoginTokens();
            Context.Characters.ResetOnlineCharacters();
            return true;
        }
    }
}
