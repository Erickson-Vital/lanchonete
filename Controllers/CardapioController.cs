using lanchonete.DTOs;
using lanchonete.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lanchonete.Controllers
{
    public class CardapioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CardapioController(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<IActionResult> Index()
        {
            var lanches = await _context.Lanches
                .Include(l => l.Ingredientes)
                .ToListAsync();

            return View(lanches);
        }

        [HttpGet]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var lanche = await _context.Lanches.SingleOrDefaultAsync(l => l.Id == id);

            if (lanche == null || lanche.Image == null)
            {
                return NotFound();
            }

            return File(lanche.Image, lanche.ImageMimiType);
        }
    }
}
