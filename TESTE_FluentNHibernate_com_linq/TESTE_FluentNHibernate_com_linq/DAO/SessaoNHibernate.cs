using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.Reflection;
using NHibernate.Tool.hbm2ddl;

namespace TESTE_FluentNHibernate_com_linq.DAO
{
    public static class SessaoNHibernate
    {
        private static string strCN = "Data Source=localhost;Initial Catalog=Teste_NH;User Id=sa;Password=orion";
        
        public static ISession CriarBanco()
        {

            var session = Fluently.Configure()
                .Database(MsSqlConfiguration
                    .MsSql2008
                    .ConnectionString(c => c.FromConnectionStringWithKey("ConexaoBanco")))
                    .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                    .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(true, true))
                    .BuildSessionFactory()
                    .OpenSession();

            return session;

        }

        public static ISession ObterSessao()
        {
            var sessao = Fluently.Configure()
            .Database(MsSqlConfiguration
            .MsSql2008
            .ConnectionString(strCN))
            .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
            .BuildSessionFactory()
            .OpenSession();

            return sessao;
        }

    }
}
