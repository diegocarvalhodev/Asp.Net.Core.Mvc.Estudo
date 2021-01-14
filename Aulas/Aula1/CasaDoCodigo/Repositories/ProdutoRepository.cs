using CasaDoCodigo.DbConfiguration;
using CasaDoCodigo.Repositories;
using CasaDoCodigo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CasaDoCodigo.Repositories
{
    public interface IProdutoRepository
    {
        void SaveProdutos(List<Livro> livros);
        IList<Produto> GetProdutos();
    }

    public class ProdutoRepository : BaseRepository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(ApplicationContext context) : base(context)
        {
        }

        public IList<Produto> GetProdutos()
        {
            return dbSets.ToList();
        }

        public void SaveProdutos(List<Livro> livros)
        {
            foreach (var livro in livros)
            {
                if (!dbSets.Where(p => p.Codigo == livro.Codigo).Any()) //Verifica se o livro já existe para não duplicar
                {   
                    dbSets.Add(new Produto(livro.Codigo, livro.Nome, livro.Preco));
                }
            }

            this.context.SaveChanges();
        }
    }
}
