using CasaDoCodigo.Models;
using CasaDoCodigo.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.DbConfiguration
{
    public class DataService : IDataService
    {
        private readonly ApplicationContext context;
        private readonly IProdutoRepository produtoRepository;

        public DataService(ApplicationContext context,
            IProdutoRepository produtoRepository)
        {
            this.context = context;
            this.produtoRepository = produtoRepository;
        }

        public void InicializeDB()
        {
            this.context.Database.EnsureCreated();

            //List<Livro> livrosNovos = FiltrandoLivrosDuplicados();
            //this.produtoRepository.SaveProdutos(livrosNovos);

            List<Livro> livros = GetLivros();

            this.produtoRepository.SaveProdutos(livros);
        }

        private List<Livro> FiltrandoLivrosDuplicados()
        {
            //Não usado. Exemplo de como filtrar por LINQ.

            var livrosNoBanco = this.context.Set<Produto>().ToList();

            List<Livro> livrosImportados = GetLivros();

            var livrosNovos = livrosImportados.Where(x => !livrosNoBanco.Select(y => y.Codigo).Contains(x.Codigo)).ToList();
            return livrosNovos;
        }

        private static List<Livro> GetLivros()
        {
            var json = File.ReadAllText("DbConfiguration/livros.json");
            var livros = JsonConvert.DeserializeObject<List<Livro>>(json);
            return livros;
        }
    }

}
