using CadastroLivros.Data.Repositorio;
using CadastroLivros.Data;
using CadastroLivrosTeste.Unitario.Fixture;
using Microsoft.EntityFrameworkCore;
using CadastroLivros.Models;

namespace CadastroLivrosTeste.Unitario.Data.Repositorio
{
    public class AutorRepositorioTests
    {
        private readonly AutorRepositorio _repositorio;
        private readonly BancoContext _context;

        public AutorRepositorioTests()
        {
            var options = new DbContextOptionsBuilder<BancoContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new BancoContext(options);
            _context.Database.EnsureCreated(); 
            _repositorio = new AutorRepositorio(_context);
        }

        [Fact(DisplayName = "Adicionar Autor deve adicionar um autor ao banco de dados")]
        [Trait("Categoria", "Repositório")]
        public async Task AdicionarAsync_DeveAdicionarAutor()
        {
            // Arrange
            var autor = AutorFaker.GerarLista(1).First();

            // Act
            var result = await _repositorio.AdicionarAsync(autor);
            var autorNoBanco = await _context.Autores.FindAsync(autor.CodAu);

            // Assert
            Assert.True(result);
            Assert.NotNull(autorNoBanco);
            Assert.Equal(autor.Nome, autorNoBanco.Nome);
        }

        [Fact(DisplayName = "Atualizar Autor deve atualizar um autor no banco de dados")]
        [Trait("Categoria", "Repositório")]
        public async Task Atualizar_DeveAtualizarAutor()
        {
            // Arrange
            var autor = AutorFaker.GerarLista(1).First();
            await _repositorio.AdicionarAsync(autor);

            autor.Nome = "Nome Atualizado";

            // Act
            var resultado = await _repositorio.Atualizar(autor);
            var autorAtualizado = await _context.Autores.FindAsync(autor.CodAu);

            // Assert
            Assert.Equal(autor.Nome, autorAtualizado.Nome);
        }

        [Fact(DisplayName = "BuscarPorNomeAsync deve retornar verdadeiro se o autor existir")]
        [Trait("Categoria", "Repositório")]
        public async Task BuscarPorNomeAsync_DeveRetornarVerdadeiroSeAutorExistir()
        {
            // Arrange
            var autor = AutorFaker.GerarLista(1).First();
            await _repositorio.AdicionarAsync(autor);

            // Act
            var existe = await _repositorio.BuscarPorNomeAsync(autor.Nome);

            // Assert
            Assert.True(existe);
        }

        [Fact(DisplayName = "BuscarPorCodAsync deve retornar o autor se existir")]
        [Trait("Categoria", "Repositório")]
        public async Task BuscarPorCodAsync_DeveRetornarAutorSeExistir()
        {

            // Arrange
            var autor = AutorFaker.GerarLista(1).First();
            var adicionou = await _repositorio.AdicionarAsync(autor);
            Assert.True(adicionou); 

            // Act
            var autorEncontrado = await _repositorio.BuscarPorCodAsync(autor.CodAu);

            // Assert
            Assert.NotNull(autorEncontrado); 
            Assert.Equal(autor.Nome, autorEncontrado.Nome); 
        }

        [Fact(DisplayName = "BuscarTodosAsync deve retornar todos os autores ativos")]
        [Trait("Categoria", "Repositório")]
        public async Task BuscarTodosAsync_DeveRetornarTodosAutoresAtivos()
        {
            // Arrange
            var autoresAtivos = AutorFaker.GerarLista(3);
            foreach (var autor in autoresAtivos)
            {
                var novoAutor = new Autor
                {
                    CodAu = autor.CodAu,
                    Nome = autor.Nome,
                    Ativo = true
                };
                await _repositorio.AdicionarAsync(novoAutor);
            }

            // Act
            var todosAutoresAtivos = await _repositorio.BuscarTodosAsync();

            // Assert
            Assert.Equal(3, todosAutoresAtivos.Count());
            Assert.All(todosAutoresAtivos, autor => Assert.True(autor.Ativo));
        }

        [Fact(DisplayName = "DeletarAsync deve marcar um autor como inativo")]
        [Trait("Categoria", "Repositório")]
        public async Task DeletarAsync_DeveMarcarAutorComoInativo()
        {
            // Arrange
            var autor = AutorFaker.GerarLista(1).First();
            await _repositorio.AdicionarAsync(autor);

            // Act
            autor.Ativo = false;
            var resultado = await _repositorio.DeletarAsync(autor);
            var autorNoBanco = await _context.Autores.FindAsync(autor.CodAu);

            // Assert
            Assert.True(resultado);
            Assert.False(autorNoBanco.Ativo);
        }
    }
}
