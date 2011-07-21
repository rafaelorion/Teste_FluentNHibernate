using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace TESTE_FluentNHibernate_com_linq.Model
{
    public class Produto
    {
        public virtual int Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Descricao { get; set; }
        public virtual decimal Preco { get; set; }
        public virtual bool Disponivel { get; set; }
    }

    public class ProdutoMap : ClassMap<Produto>
    {
        public ProdutoMap()
        {
            Id(p => p.Id);
            Map(p => p.Nome);
            Map(p => p.Descricao);
            Map(p => p.Preco);
            Map(p => p.Disponivel);
        }
    }
}
