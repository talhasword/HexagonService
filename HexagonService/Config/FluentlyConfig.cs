using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Cfg;
using HexagonService.Entity;

namespace HexagonService.Config
{
    public class FluentlyConfig
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ISessionFactory SessionFactory { get; set; }

        private FluentlyConfig()
        {
            
            SessionFactory = CreateSessionFactory();
        }

        public static FluentlyConfig Instance
        {
            get
            {
                return FluentNhibernateConfigFactory.instance;
            }
        }

        public class FluentNhibernateConfigFactory
        {
            static FluentNhibernateConfigFactory() { }
            internal static readonly FluentlyConfig instance = new FluentlyConfig();
        }
         
        public ISessionFactory CreateSessionFactory()
        {
            try
            {
                if (this.SessionFactory == null)
                {
                    return Fluently.Configure()
                    .Database(
                    MsSqlConfiguration
                    .MsSql2008
                    .ConnectionString(c => c
                    .FromConnectionStringWithKey("DBSERVER"))
                    .DefaultSchema("dbo")
                    )
                    .Mappings(m =>
                    {
                        m.FluentMappings
                        .AddFromAssemblyOf<Player>()
                       .Conventions.Add(FluentNHibernate.Conventions.Helpers.DefaultLazy.Never());
                    })
                    .BuildSessionFactory();
                }
                else { return SessionFactory; }
            }
            catch (Exception ex)
            {
                logger.Fatal("Bağlantı Oluşturulamadı", ex);
                return null;
            }
        }
    }
}