using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace lanchonete.DTOs
{
    public class LancheDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }

        [Required]
        public IFormFile Image { get; set; }

        //public SelectList IngredientesDisponiveis { get; set; } 


        public List<int> IngredientesSelecionados { get; set; }
    }
}
