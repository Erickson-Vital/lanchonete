using lanchonete.DTOs;
using lanchonete.Models;
using lanchonete.Services;
using Microsoft.AspNetCore.Mvc;

namespace lanchonete.Controllers;

public class IngredienteController : Controller
{
    private readonly ApplicationDbContext _context;
    public IngredienteController(ApplicationDbContext context)
    {
        this._context = context;
    }
    public IActionResult Index()
    {
        var ingredientes = _context.Ingredientes.ToList();
        return View(ingredientes);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(IngredienteDto ingredienteDto)
    {
        Ingrediente ingrediente = new Ingrediente()
        {
            Name = ingredienteDto.Name,
            Price = ingredienteDto.Price

        };

        // Aqui a gente salva o OBJ N O B A N C O!!
        _context.Ingredientes.Add(ingrediente);
        _context.SaveChanges();

        return RedirectToAction("Index", "Ingrediente");
    }

    public IActionResult Edit(int id)
    {
        var ingrediente = _context.Ingredientes.Find(id);

        if (ingrediente == null)
        {
            return RedirectToAction("Index", "Ingrediente");
        }

        return View(ingrediente);
    }

    [HttpPost]
    public IActionResult Edit(int id, IngredienteDto ingredienteDto)
    {
        var ingrediente = _context.Ingredientes.Find(id);
        if (ingrediente == null)
        {
            return RedirectToAction("Index", "Ingrediente");
        }

        ingrediente.Name = ingredienteDto.Name;
        ingrediente.Price = ingredienteDto.Price;

        // Aqui salva as alteração
        _context.SaveChanges();

        return RedirectToAction("Index", "Ingrediente");
    }


    public IActionResult Delete(int id)
    {
        var ingrediente = _context.Ingredientes.Find(id);
        if (ingrediente == null)
        {
            return RedirectToAction("Index", "Ingrediente");
        }

        _context.Ingredientes.Remove(ingrediente);
        _context.SaveChanges();

        return RedirectToAction("Index", "Ingrediente");
    }

}
