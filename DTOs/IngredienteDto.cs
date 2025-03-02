using System.ComponentModel.DataAnnotations;

namespace lanchonete.DTOs
{
    public class IngredienteDto
    {
        public string Name { get; set; }
        [Display(Name = "Preço")]
        public decimal Price { get; set; }
    }
}
