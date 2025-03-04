using lanchonete.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace lanchonete.Services
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) // chama o construtor do DbContex
        {

        }

        public DbSet<Lanche> Lanches { get; set; }
        public DbSet<Ingrediente> Ingredientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Chama a configuração base do Identity

            // Configuração da relação do Many-to-Many entre Lanche e Ingredientes(DIFCILLL)
            modelBuilder.Entity<Lanche>()
                .HasMany(l => l.Ingredientes)
                .WithMany(i => i.Lanches)
                .UsingEntity(j => j.ToTable("LancheIngrediente"));

            // Chave para o Identity
            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey });


            // Criando a parte de acessos(Roles)
            var admin = new IdentityRole("admin");
            admin.NormalizedName = "admin";

            var cliente = new IdentityRole("cliente");
            cliente.NormalizedName = "cliente";

            modelBuilder.Entity<IdentityRole>().HasData(admin, cliente);

        }
    }
}
