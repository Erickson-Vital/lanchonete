using lanchonete.Models;

namespace lanchonete.DTOs
{
    public class PedidoDto
    {
        public ICollection<ItemPedidoDto> Itens { get; set; }

    }
}