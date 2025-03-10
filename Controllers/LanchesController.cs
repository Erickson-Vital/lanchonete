using lanchonete.DTOs;
using lanchonete.Models;
using lanchonete.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace lanchonete.Controllers
{
    public class LanchesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public LanchesController(ApplicationDbContext context) 
        {
            this._context = context;
        }

        public IActionResult Index()
        {
            var lanches = _context.Lanches.Include(l => l.Ingredientes).ToList();
            var lanchesIndex = new List<LancheIndexDto>();

            foreach(var lanche in lanches)
            {
                lanchesIndex.Add(new LancheIndexDto()
                {
                    Id = lanche.Id,
                    Name = lanche.Name,
                    Price = lanche.Price,
                    Ingredientes = lanche.Ingredientes.Select(i => i.Name).ToArray()
                });
            }
            
            return View(lanchesIndex);
        }


        public IActionResult Create()
        {
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            var ingredientes = _context.Ingredientes
                .Select(i => new
                {
                    id = i.Id,
                    nome = i.Name,
                    preco = i.Price
                }).ToList();

            ViewBag.IngredientesJson = System.Text.Json.JsonSerializer.Serialize(ingredientes);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(LancheDto lancheDto)
        {
            if (!ModelState.IsValid)
            {
                return View(lancheDto);
            }

            using (var memoryStream = new MemoryStream())
            {
                await lancheDto.Image.CopyToAsync(memoryStream);

                if (memoryStream.Length < 2097152) // 2MB o limite da img
                {
                    var ingredientesSelecionados = _context.Ingredientes
                        .Where(i => lancheDto.IngredientesSelecionados.Contains(i.Id))
                        .ToList();

                    var lanche = new Lanche()
                    {
                        Image = memoryStream.ToArray(),
                        ImageMimiType = lancheDto.Image.ContentType,
                        Name = lancheDto.Name,
                        Ingredientes = ingredientesSelecionados,
                        Price = lancheDto.Price // O valor calculado no Front será salvo aqui!
                    };

                    _context.Lanches.Add(lanche);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ModelState.AddModelError("Image", "O arquivo da imagem é muito grande.");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(lancheDto);
            }

            return RedirectToAction("Index", "Lanches");
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var lanche = await _context.Lanches
                .Include(l => l.Ingredientes)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (lanche == null)
                return NotFound();

            return View(lanche);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lanche = await _context.Lanches
                .Include(l => l.Ingredientes)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lanche != null)
            {
                lanche.Ingredientes.Clear();
                _context.Lanches.Remove(lanche);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
