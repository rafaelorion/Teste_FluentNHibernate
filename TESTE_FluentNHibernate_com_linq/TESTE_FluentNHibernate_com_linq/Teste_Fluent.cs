using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.Reflection;
using TESTE_FluentNHibernate_com_linq.Model;
using NHibernate.Linq;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using TESTE_FluentNHibernate_com_linq.DAO;
using NHibernate.Criterion;

namespace TESTE_FluentNHibernate_com_linq
{
    /// <summary>
    /// Dependencias via NuGet:
    /// -------------------------------------------------------------------------
    /// Install-Package NHibernate.Linq
    /// Install-Package FluentNHibernate -version 1.1.0.694 -IgnoreDependencies
    /// --------------------------------------------------------------------------
    /// 
    /// Referencias Bibliograficas
    /// --------------------------------------------------------------------------
    /// http://knol.google.com/k/fabio-maulo/nhibernate/1nr4enxv3dpeq/21#
    /// http://www.hibernate.org/docs
    /// http://mhinze.com/2008/07/22/linq-to-nhibernate-in-10-minutes/
    /// --------------------------------------------------------------------------
    /// </summary>
    [TestClass]
    public class Teste_Fluent
    {
        public Teste_Fluent()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion



        //TODO:Fazer testes de consulta com join
        //TODO:Fazer testes de consulta com sum
        //TODO:Fazer testes de consulta com left e rigth join
        //TODO:Fazer testes de consulta com sub-query
        //TODO:Fazer testes de consulta com Transaçao
        //TODO:Fazer testes de consulta com order by
        //TODO:Fazer testes de consulta com group by
        //TODO:Salvar em cascata
        //TODO:Apagar em cascata
        //TODO:Editar Registro
        //TODO:Apagar Registro


        [Ignore]
        [TestMethod]
        public void InserirRegistros()
        {
            var sessao = SessaoNHibernate.ObterSessao();

            var produto1 = new Produto();
            produto1.Nome = "Prod 1";
            produto1.Descricao = "bla bla bla prod 1 bla bla bla";
            produto1.Disponivel = true;
            produto1.Preco = 10;

            var produto2 = new Produto();
            produto2.Nome = "Prod 2";
            produto2.Descricao = "bla bla bla prod 2 bla bla bla";
            produto2.Disponivel = true;
            produto2.Preco = 20;

            var produto3 = new Produto();
            produto3.Nome = "Prod 3";
            produto3.Descricao = "bla bla bla prod 3 bla bla bla";
            produto3.Disponivel = false;
            produto3.Preco = 30;

            var produto4 = new Produto();
            produto4.Nome = "Prod 4";
            produto4.Descricao = "bla bla bla prod 4 bla bla bla";
            produto4.Disponivel = true;
            produto4.Preco = 40;

            var produto5 = new Produto();
            produto5.Nome = "Prod 5";
            produto5.Descricao = "bla bla bla prod 5 bla bla bla";
            produto5.Disponivel = true;
            produto5.Preco = 50;


            sessao.SaveOrUpdate(produto1);
            sessao.SaveOrUpdate(produto2);
            sessao.SaveOrUpdate(produto3);
            sessao.SaveOrUpdate(produto4);
            sessao.SaveOrUpdate(produto5);

        }

        [Ignore]
        [TestMethod]
        public void EditarRegistros()
        {
        }

        [Ignore]
        [TestMethod]
        public void ExcluirRegistros()
        {
        }

        [Ignore]
        [TestMethod]
        public void ConsultarRegistrosComLinq()
        {
            var sessao = SessaoNHibernate.ObterSessao();

            var query = from p in sessao.Linq<Produto>() where p.Nome.Contains("2") select p;
            var produtos = query.ToList<Produto>();

            Assert.AreEqual(1, produtos.Count());
            Assert.AreEqual("Prod 2", produtos[0].Nome);


            produtos = (from p in sessao.Linq<Produto>() where p.Disponivel == false select p).ToList<Produto>();
            Assert.AreEqual(1, produtos.Count());
            Assert.AreEqual(3, produtos[0].Id);

            produtos = (from p in sessao.Linq<Produto>() where p.Descricao.Contains("prod 4") && p.Disponivel == true select p).ToList<Produto>();
            Assert.AreEqual(1, produtos.Count());
            Assert.AreEqual(4, produtos[0].Id);

            produtos = (from p in sessao.Linq<Produto>() where p.Descricao.Contains("prod 4") || p.Disponivel == false select p).ToList<Produto>();
            Assert.AreEqual(2, produtos.Count());

        }

        [Ignore]
        [TestMethod]
        public void ConsultarRegistrosComNH()
        {
            var sessao = SessaoNHibernate.ObterSessao();

            var produto = sessao.Get<Produto>(2);
            Assert.AreEqual("Prod 2", produto.Nome);

            var produtos = sessao.CreateCriteria(typeof(Produto))
                .AddOrder(Order.Asc("Nome"))
                .SetMaxResults(50)
                .List();
            Assert.AreEqual(5, produtos.Count);


            produtos = sessao.CreateCriteria(typeof(Produto))
                .Add(Expression.Eq("Nome","Prod 5"))
                .Add(Expression.Eq("Disponivel", true))
                .Add(Expression.Like("Descricao","%prod 5%"))
                .List();
            Assert.AreEqual(1, produtos.Count);
            Assert.AreEqual(5, ((Produto)produtos[0]).Id);

        }
        
        [TestMethod]
        public void ConsultarRegistrosComSQLQuery()
        {
            var sessao = SessaoNHibernate.ObterSessao();

            var produtos = sessao.CreateSQLQuery
                (@"select  *  from Produto")
                .List();
            Assert.AreEqual(5, produtos.Count);

            produtos = sessao.CreateSQLQuery("SELECT * FROM Produto WHERE Id = :codigo")
                .AddEntity(typeof(Produto))
                .SetString("codigo", "4")
                .List();  
            Assert.AreEqual(1, produtos.Count);
            Assert.AreEqual("Prod 4", ((Produto)produtos[0]).Nome);

        }

    }
}
