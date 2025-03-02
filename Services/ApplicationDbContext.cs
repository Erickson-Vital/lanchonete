using lanchonete.Models;
using Microsoft.EntityFrameworkCore;

namespace lanchonete.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) // chama o construtor do DbContex
        {

        }

        public DbSet<Lanche> Lanches { get; set; }
        public DbSet<Ingrediente> Ingredientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Se desejar customizar a tabela de junção, pode usar o Fluent API:
            modelBuilder.Entity<Lanche>()
                .HasMany(l => l.Ingredientes)
                .WithMany(i => i.Lanches)
                .UsingEntity(j => j.ToTable("LancheIngrediente"));
        }
    }
}
