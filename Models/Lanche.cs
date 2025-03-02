using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lanchonete.Models
{
    public class Lanche
    {
        public int Id { get; set; }
        [Display(Name = "Nome")]
        public string Name { get; set; }
        [Display(Name="Preço")]
        public decimal Price { get; set; }
        [Display(Name = "Ingredientes")]

        [Column(TypeName = "varbinary(max)")]
        public byte[] Image { get; set; }

        public ICollection<Ingrediente> Ingredientes { get; set; }
    }
}
