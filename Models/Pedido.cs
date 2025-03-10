using System.ComponentModel.DataAnnotations.Schema;

namespace lanchonete.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<ItemPedido> Itens { get; set; }

        // Adicionado: relacionamento com o usuario
        public string UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public ApplicationUser Usuario { get; set; }
    }
}
