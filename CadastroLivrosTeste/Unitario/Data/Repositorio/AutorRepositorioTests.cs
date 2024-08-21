using CadastroLivros.Data.Repositorio;
using CadastroLivros.Data;
using CadastroLivrosTeste.Unitario.Fixture;
using Microsoft.EntityFrameworkCore;
using CadastroLivros.Models;

namespace CadastroLivrosTeste.Unitario.Data.Repositorio
{
    public class AutorRepositorioTests: IDisposable
    {
        private readonly AutorRepositorio _autorRepositorio;
        private readonly BancoContext _bancoContext;

        public AutorRepositorioTests()
        {
            var options = new DbContextOptionsBuilder<BancoContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _bancoContext = new BancoContext(options);
            _bancoContext.Database.EnsureCreated(); 
            _autorRepositorio = new AutorRepositorio(_bancoContext);
        }

        public void Dispose()
        {
            _bancoContext.Dispose();
        }

        [Fact(DisplayName = "Adicionar Autor deve adicionar um autor ao banco de dados")]
        [Trait("Repositorio", "Adicionar")]
        public async Task AdicionarAsync_DeveAdicionarAutor()
        {
            // Arrange
            var autor = AutorFaker.GerarLista(1).First();

            // Act
            var result = await _autorRepositorio.AdicionarAsync(autor);
            var autorNoBanco = await _bancoContext.Autores.FindAsync(autor.CodAu);

            // Assert
            Assert.True(result);
            Assert.NotNull(autorNoBanco);
            Assert.Equal(autor.Nome, autorNoBanco.Nome);
        }

        [Fact(DisplayName = "Atualizar Autor deve atualizar um autor no banco de dados")]
        [Trait("Repositorio", "Atualizar")]
        public async Task Atualizar_DeveAtualizarAutor()
        {
            // Arrange
            var autor = AutorFaker.GerarLista(1).First();
            await _autorRepositorio.AdicionarAsync(autor);

            autor.Nome = "Nome Atualizado";

            // Act
            var resultado = await _autorRepositorio.Atualizar(autor);
            var autorAtualizado = await _bancoContext.Autores.FindAsync(autor.CodAu);

            // Assert
            Assert.Equal(autor.Nome, autorAtualizado.Nome);
        }

        [Fact(DisplayName = "BuscarPorNomeAsync deve retornar verdadeiro se o autor existir")]
        [Trait("Repositorio", "Buscar Por Nome")]
        public async Task BuscarPorNomeAsync_DeveRetornarVerdadeiroSeAutorExistir()
        {
            // Arrange
            var autor = AutorFaker.GerarLista(1).First();
            autor.Ativo = true;
            await _autorRepositorio.AdicionarAsync(autor);

            // Act
            var existe = await _autorRepositorio.BuscarPorNomeAsync(autor.Nome);

            // Assert
            Assert.True(existe);
        }

        [Fact(DisplayName = "BuscarPorCodAsync deve retornar o autor se existir")]
        [Trait("Repositorio", "Buscar por Cod")]
        public async Task BuscarPorCodAsync_DeveRetornarAutorSeExistir()
        {

            // Arrange
            var autor = AutorFaker.GerarLista(1).First();
            autor.Ativo = true;
            var adicionou = await _autorRepositorio.AdicionarAsync(autor);
            Assert.True(adicionou); 

            // Act
            var autorEncontrado = await _autorRepositorio.BuscarPorCodAsync(autor.CodAu);

            // Assert
            Assert.NotNull(autorEncontrado); 
            Assert.Equal(autor.Nome, autorEncontrado.Nome); 
        }

        [Fact(DisplayName = "BuscarTodosAsync deve retornar todos os autores ativos")]
        [Trait("Repositorio", "Buscar Todos")]
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
                await _autorRepositorio.AdicionarAsync(novoAutor);
            }

            // Act
            var todosAutoresAtivos = await _autorRepositorio.BuscarTodosAsync();

            // Assert
            Assert.Equal(3, todosAutoresAtivos.Count());
            Assert.All(todosAutoresAtivos, autor => Assert.True(autor.Ativo));
        }

        [Fact(DisplayName = "DeletarAsync deve marcar um autor como inativo")]
        [Trait("Repositorio", "Deletar")]
        public async Task DeletarAsync_DeveMarcarAutorComoInativo()
        {
            // Arrange
            var autor = AutorFaker.GerarLista(1).First();
            await _autorRepositorio.AdicionarAsync(autor);

            // Act
            autor.Ativo = false;
            var resultado = await _autorRepositorio.DeletarAsync(autor);
            var autorNoBanco = await _bancoContext.Autores.FindAsync(autor.CodAu);

            // Assert
            Assert.True(resultado);
            Assert.False(autorNoBanco.Ativo);
        }

        [Fact(DisplayName = "ObterPorNomeAsync deve retornar o autor se existir")]
        [Trait("Categoria", "Obter Por Nome")]
        public async Task ObterPorNomeAsync_DeveRetornarAutorSeExistir()
        {
            // Arrange
            var autorExistente = AutorFaker.AutorFakerComCampoObrigatorioNulo();
            autorExistente.Nome = "Autor Teste";
            autorExistente.Ativo = true;

            await _autorRepositorio.AdicionarAsync(autorExistente);

            // Act
            var autorObtido = await _autorRepositorio.ObterPorNomeAsync("Autor Teste");

            // Assert
            Assert.NotNull(autorObtido);
            Assert.Equal(autorExistente.Nome, autorObtido.Nome);
        }

        [Fact(DisplayName = "ObterPorNomeAsync deve retornar null se o autor não existir")]
        [Trait("Categoria", "Obter Por nome null")]
        public async Task ObterPorNomeAsync_DeveRetornarNullSeNaoExistir()
        {
            // Act
            var autorObtido = await _autorRepositorio.ObterPorNomeAsync("Autor Inexistente");

            // Assert
            Assert.Null(autorObtido);
        }

    }
}
