namespace lanchonete.Models
{
    public class ItemPedido
    {
        public int Id { get; set; }
        public int LancheID { get; set; }
        public int Quantidade { get; set; }
        public decimal Price { get; set; }
        public Lanche Lanche { get; set; }
    }
}
