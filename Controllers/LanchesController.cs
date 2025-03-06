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
            //LancheDto lanche = new LancheDto();

            ViewBag.LanchesDisponiveis = new SelectList(_context.Ingredientes.ToList(), nameof(Ingrediente.Id), nameof(Ingrediente.Name));

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

                // Upload the file if less than 2 MB
                if (memoryStream.Length < 2097152)
                {
                    var lanche = new Lanche()
                    {
                        Image = memoryStream.ToArray(),
                        ImageMimiType = lancheDto.Image.ContentType,
                        Name = lancheDto.Name,
                        Price = lancheDto.Price,
                        Ingredientes = _context.Ingredientes.Where(i => lancheDto.IngredientesSelecionados.Contains(i.Id)).ToList(),
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
                return View(lancheDto); // se der algum erro ele retorna a VIEW
            }

            return RedirectToAction("Index", "Lanches");
        }
    }
}
