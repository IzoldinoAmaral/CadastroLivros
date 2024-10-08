using CadastroLivros.Data;
using CadastroLivros.Data.Repositorio;
using CadastroLivros.Interfaces.Repositorios;
using CadastroLivros.Interfaces.Servicos;
using CadastroLivros.Models;
using CadastroLivros.Servicos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BancoContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));


builder.Services.AddTransient<IGenericoRepositorio<Livro>, LivroRepositorio>();
builder.Services.AddTransient<IGenericoRepositorio<Autor>, AutorRepositorio>();
builder.Services.AddTransient<IGenericoRepositorio<Assunto>, AssuntoRepositorio>();
builder.Services.AddTransient<IGenericoRepositorio<FormaCompra>, FormaCompraRepositorio>();
builder.Services.AddTransient<ILivroRepositorio, LivroRepositorio>();
builder.Services.AddTransient<ILivroRelatorioRepositorio, LivroRelatorioRepositorio>();
builder.Services.AddScoped<ILivroServico, LivroServico>();
builder.Services.AddScoped<IAutorServico, AutorServico>();
builder.Services.AddScoped<IAssuntoServico, AssuntoServico>();
builder.Services.AddScoped<ILivroRelatorioServico, LivroRelatorioServico>();
builder.Services.AddScoped<IFormaCompraServico, FormaCompraServico>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
