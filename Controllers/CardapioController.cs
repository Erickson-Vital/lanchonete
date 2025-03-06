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
        public IActionResult Index()
        {
            var lanches = _context.Lanches.ToList();

            return View(lanches);
        }

        [HttpGet]
        public async Task<FileContentResult> GetPhoto(int id)
        {
            var lanche = await _context.Lanches.SingleOrDefaultAsync(l => l.Id == id);
            
            return File(lanche.Image, lanche.ImageMimiType);
        }
    }
}
