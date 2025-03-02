using System.ComponentModel.DataAnnotations;

namespace lanchonete.Models
{
    public class Ingrediente
    {
        public int Id { get; set; }
        [Display(Name = "Nome")]
        public string Name { get; set; }
        [Display(Name="Preço")]
        public decimal Price { get; set; }
        public ICollection<Lanche> Lanches { get; set; }
    }
}
