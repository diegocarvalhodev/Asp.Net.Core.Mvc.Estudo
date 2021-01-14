using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CasaDoCodigo.DbConfiguration;
using CasaDoCodigo.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CasaDoCodigo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // String de conexão
            string connectionString = Configuration.GetConnectionString("Default");

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connectionString)
            );

            services.AddTransient<IDataService, DataService>();
            services.AddTransient<IProdutoRepository, ProdutoRepository>();
            services.AddTransient<ICadastroRepository, CadastroRepository>();
            services.AddTransient<IItemPedidoRepository, ItemPedidoRepository>();
            services.AddTransient<IPedidoRepository, PedidoRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Pedido}/{action=Carrossel}/{codigo?}");
            });


            /* Garantir que o banco de dados esteja criado ao iniciar a aplicação.
             * Caso o banco de dados não exista, o EF Core vai criar o banco e tabelas.
             * Cria o banco com base nos modelos. Além de não usar migração, não vai 
             * mais permitir que migrações sejam utilizadas no banco criado com o EnsureCreated.
             * O seu uso é recomendado para criar bancos de teste.*/
            /*serviceProvider
                .GetService<ApplicationContext>()
                .Database
                .EnsureCreated();*/

            /* Se o banco ainda não estiver criado, irá gerar o banco utilizando migração.
             * Não impedir que novas migrações sejam aplicadas ao banco de dados gerado.*/
            /*serviceProvider
                .GetService<ApplicationContext>()
                .Database
                .Migrate();*/

            /* Inicializando o banco utilizando a classe DataService,
             * que foi criada para centralizar essa responsabilidade. */
            serviceProvider
                .GetService<IDataService>()
                .InicializeDB();

        }
    }
}
