using lanchonete.Models;
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
        private readonly UserManager<ApplicationUser> _userManager; // Para obter o usuário logado

        public PedidoController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
            }
            else
            {
                pedidoAtual.Itens.Add(new ItemPedido
                {
                    LancheID = lanche.Id,
                    Lanche = lanche,
                    Quantidade = 1,
                    Price = lanche.Price // preço unitário
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

            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var novoPedido = new Pedido
            {
                CreatedDate = DateTime.Now,
                Status = 0,
                UsuarioId = usuarioId,
                Itens = new List<ItemPedido>()
            };

            foreach (var item in pedido.Itens)
            {
                // Garante que o Lanche já está sendo rastreado, não vai tentar inseri-lo de novo
                var lancheReal = await _context.Lanches.FindAsync(item.LancheID);

                if (lancheReal != null)
                {
                    novoPedido.Itens.Add(new ItemPedido
                    {
                        LancheID = lancheReal.Id,
                        Lanche = lancheReal,
                        Quantidade = item.Quantidade,
                        Price = lancheReal.Price
                    });
                }
            }

            novoPedido.Price = novoPedido.Itens.Sum(i => i.Price * i.Quantidade);

            _context.Pedidos.Add(novoPedido);
            await _context.SaveChangesAsync();

            LimparPedido();

            return View("Views/Pedido/PedidoFinalizado.cshtml", novoPedido);
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
