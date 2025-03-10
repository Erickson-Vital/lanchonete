﻿using lanchonete.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using lanchonete.Services;
using Microsoft.AspNetCore.Authorization;

namespace lanchonete.Controllers
{
    public class PedidoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PedidoController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Regrinha para os desconto dos pedidos 
        private decimal CalcularDesconto(decimal precoOriginal, int quantidade)
        {
            if (quantidade >= 5)
                return precoOriginal * 0.9m; // 10% de desconto
            if (quantidade >= 3)
                return precoOriginal * 0.95m; // 5% de desconto

            return precoOriginal; // Sem desconto
        }

        public IActionResult AdicionarAoPedido(int id)
        {
            var lanche = _context.Lanches.Find(id);
            if (lanche == null)
            {
                return NotFound();
            }

            var pedidoAtual = ObterPedidoAtual();

            if (pedidoAtual == null)
            {
                pedidoAtual = new Pedido
                {
                    Itens = new List<ItemPedido>()
                };
            }

            if (pedidoAtual.Itens == null)
            {
                pedidoAtual.Itens = new List<ItemPedido>();
            }

            var itemExistente = pedidoAtual.Itens.FirstOrDefault(i => i.LancheID == id);

            if (itemExistente != null)
            {
                itemExistente.Quantidade++;
                itemExistente.Price = CalcularDesconto(lanche.Price, itemExistente.Quantidade);
            }
            else
            {
                pedidoAtual.Itens.Add(new ItemPedido
                {
                    LancheID = lanche.Id,
                    Quantidade = 1,
                    Lanche = lanche,
                    Price = CalcularDesconto(lanche.Price, 1)
                });
            }

            SalvarPedidoNaSessao(pedidoAtual);
            return RedirectToAction("Index", "Cardapio");
        }

        public IActionResult ExibirPedido()
        {
            var pedido = ObterPedidoAtual();
            return View("Views/Pedido/ExibirPedido.cshtml", pedido);
        }

        public async Task<IActionResult> FinalizarPedido()
        {
            var pedido = ObterPedidoAtual();

            if (!pedido.Itens.Any())
            {
                return RedirectToAction("Index", "Cardapio");
            }

            var quantidadeTotalLanches = pedido.Itens.Sum(i => i.Quantidade);
            decimal total = pedido.Itens.Sum(i => i.Price * i.Quantidade);
            decimal desconto = 0;

            if (quantidadeTotalLanches == 2)
                desconto = total * 0.03m;
            else if (quantidadeTotalLanches == 3)
                desconto = total * 0.05m;
            else if (quantidadeTotalLanches >= 5)
                desconto = total * 0.10m;

            decimal totalComDesconto = total - desconto;

            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var novoPedido = new Pedido
            {
                Price = totalComDesconto,
                CreatedDate = DateTime.Now,
                Status = 0,
                UsuarioId = usuarioId,
                Itens = pedido.Itens.Select(i => new ItemPedido
                {
                    LancheID = i.LancheID,
                    Quantidade = i.Quantidade,
                    Price = i.Price
                }).ToList()
            };


            _context.Pedidos.Add(novoPedido);
            await _context.SaveChangesAsync();

            // Carrega o pedido com os lanches para exibir na tela
            var pedidoCompleto = await _context.Pedidos
                .Include(p => p.Itens)
                    .ThenInclude(i => i.Lanche)
                .FirstOrDefaultAsync(p => p.Id == novoPedido.Id);

            LimparPedido();

            return View("PedidoFinalizado", pedidoCompleto);
        }

        public IActionResult RemoverDoPedido(int id)
        {
            var pedidoAtual = ObterPedidoAtual();

            if (pedidoAtual.Itens != null)
            {
                var itemParaRemover = pedidoAtual.Itens.FirstOrDefault(i => i.LancheID == id);
                if (itemParaRemover != null)
                {
                    pedidoAtual.Itens.Remove(itemParaRemover);
                    SalvarPedidoNaSessao(pedidoAtual);
                }
            }

            return RedirectToAction("Index", "Cardapio");
        }

        private Pedido ObterPedidoAtual()
        {
            var pedido = HttpContext.Session.GetString("Pedido");

            if (pedido != null)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Pedido>(pedido);
            }
            return new Pedido();
        }

        private void SalvarPedidoNaSessao(Pedido pedido)
        {
            HttpContext.Session.SetString("Pedido", Newtonsoft.Json.JsonConvert.SerializeObject(pedido));
        }

        private void LimparPedido()
        {
            HttpContext.Session.Remove("Pedido");
        }

        [Authorize]
        public IActionResult MeusPedidos()
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("admin");

            var pedidos = _context.Pedidos
                .Include(p => p.Itens)
                    .ThenInclude(i => i.Lanche)
                .Where(p => isAdmin || p.UsuarioId == usuarioId)
                .OrderByDescending(p => p.CreatedDate)
                .ToList();

            return View("MeusPedidos", pedidos);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Concluir(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
                return NotFound();

            pedido.Status = 1;
            await _context.SaveChangesAsync();

            return RedirectToAction("MeusPedidos");
        }
    }
}
