using System.ComponentModel.DataAnnotations;

namespace lanchonete.DTOs
{
    public class LancheDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        public IFormFile Image { get; set; }
        public List<int> IngredientesSelecionados { get; set; }
    }
}
