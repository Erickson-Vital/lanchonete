using lanchonete.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using lanchonete.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Conex�o com banco de dados
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connecttionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connecttionString);
});

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Adiciona suporte ao Identity
app.UseAuthorization();

// isso aqui é usado para configurar as rotas dos controllers
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Necessário para as páginas do Identity

app.Run();
