using CasaDoCodigo.DbConfiguration;
using CasaDoCodigo.Repositories;
using CasaDoCodigo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private ApplicationContext context;

        public ProdutoRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public IList<Produto> GetProdutos()
        {
            return this.context.Set<Produto>().ToList();
        }

        public void SaveProdutos(List<Livro> livros)
        {
            foreach (var livro in livros)
            {
                this.context.Set<Models.Produto>().Add(new Models.Produto(livro.Codigo, livro.Nome, livro.Preco));
            }

            this.context.SaveChanges();
        }
    }
}
