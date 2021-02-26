using CasaDoCodigo.Models;
using CasaDoCodigo.Models.ViewModels;
using CasaDoCodigo.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IProdutoRepository produtoRepository;
        private readonly IPedidoRepository pedidoRepository;
        private readonly IItemPedidoRepository itemPedidoRepository;

        public PedidoController(IProdutoRepository produtoRepository,
            IPedidoRepository pedidoRepository,
            IItemPedidoRepository itemPedidoRepository)
        {
            this.produtoRepository = produtoRepository;
            this.pedidoRepository = pedidoRepository;
            this.itemPedidoRepository = itemPedidoRepository;
        }

        public IActionResult Carrossel()
        {
            return View(produtoRepository.GetProdutos());
        }

        public IActionResult Carrinho(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                pedidoRepository.AddItem(codigo);
            }

            var itens = pedidoRepository.GetPedido().Itens;
            CarrinhoViewModel carrinhoViewModel = new CarrinhoViewModel(itens);
            return base.View(carrinhoViewModel);
        }

        public IActionResult Cadastro()
        {
            var pedido = pedidoRepository.GetPedido();

            if ( (pedido == null) || (pedido.Itens.Count == 0))
            {
                return RedirectToAction("Carrossel");
            }

            return View(pedido.Cadastro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //Aceitar requisição apenas com Token válido
        public IActionResult Resumo(Cadastro cadastro)
        {
            if (ModelState.IsValid)
            {
                return View(pedidoRepository.UpdateCadastro(cadastro));
            }

            return RedirectToAction("Cadastro");
        }

        [HttpPost] //Atributo para permitir apenas requisições do tipo POST
        [ValidateAntiForgeryToken] //Aceitar requisição apenas com Token válido
        public UpdateQuantidadeResponse UpdateQuantidade([FromBody] ItemPedido itemPedido)
        /*O atributo [FromBody] sinalizar que o valor, enviado
          na requisição, faz parte do corpo da requisição.*/
        {
            return pedidoRepository.UpdateQuantidade(itemPedido);
        }

    }
}
