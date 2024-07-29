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


builder.Services.AddScoped<IGenericoRepositorio<Livro>, LivroRepositorio>();
builder.Services.AddScoped<IGenericoRepositorio<Autor>, AutorRepositorio>();
builder.Services.AddScoped<IGenericoRepositorio<Assunto>, AssuntoRepositorio>();
builder.Services.AddScoped<ILivroServico, LivroServico>();
builder.Services.AddScoped<IAutorServico, AutorServico>();
builder.Services.AddScoped<IAssuntoServico, AssuntoServico>();

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
