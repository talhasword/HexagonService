using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HexagonService.Contracts;
using NHibernate;

namespace HexagonService.DAL
{
    public class UnitOfWork : IDisposable
    {
        private readonly ISessionFactory sessionFactory;
        private readonly ITransaction transaction;
        public ISession Session { get; private set; }
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public UnitOfWork(ISessionFactory sessionFactory)
        {
            try
            {
                this.sessionFactory = sessionFactory;
                Session = this.sessionFactory.OpenSession();
                transaction = Session.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            }
            catch (Exception ex)
            {
                //log4net.GlobalContext.Properties["customerinfoid"] = 0;
                //log4net.GlobalContext.Properties["citizenshipnumber"] = "SYSTEM";
                logger.Fatal("Bağlantı oluşturma hatası", ex);
            }
        }
        
        public void Dispose()
        {
            Session.Close();
            Session.Dispose();
        }

        public void RollBack()
        {
            if (transaction.IsActive)
                transaction.Rollback();

            transaction.Dispose();
        }

        public void Commit()
        {
            if (transaction.IsActive)
                transaction.Commit();
        }
    }
}
