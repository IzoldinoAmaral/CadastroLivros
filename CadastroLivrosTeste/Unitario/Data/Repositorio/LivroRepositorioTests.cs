using CadastroLivros.Data.Repositorio;
using CadastroLivros.Data;
using CadastroLivrosTeste.Unitario.Fixture;
using Microsoft.EntityFrameworkCore;

namespace CadastroLivrosTeste.Unitario.Data.Repositorio
{
    public class LivroRepositorioTests: IDisposable
    {
        private readonly LivroRepositorio _livroRepositorio;
        private readonly BancoContext _bancoContext;

        public LivroRepositorioTests()
        {
            var options = new DbContextOptionsBuilder<BancoContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _bancoContext = new BancoContext(options);
            _bancoContext.Database.EnsureCreated();
            _livroRepositorio = new LivroRepositorio(_bancoContext);

        }

        public void Dispose()
        {
            _bancoContext.Dispose();
        }

        [Fact(DisplayName = "BuscarPorNomeAsync - DeveRetornarFalseSeLivroNaoExistir")]
        [Trait("Repositorio", "Unitario")]
        public async Task BuscarPorNomeAsync_DeveRetornarFalseSeLivroNaoExistir()
        {
            // Act
            var resultado = await _livroRepositorio.BuscarPorNomeAsync("Titulo Inexistente");

            // Assert
            Assert.False(resultado);
        }


        [Fact(DisplayName = "BuscarTodosAutoresAsync - DeveRetornarTodosAutores")]
        [Trait("Repositorio", "Unitario")]
        public async Task BuscarTodosAutoresAsync_DeveRetornarTodosAutores()
        {
            // Arrange
            var autores = AutorFaker.GerarLista(3);
            await _bancoContext.Autores.AddRangeAsync(autores);
            await _bancoContext.SaveChangesAsync();

            // Act
            var resultado = await _livroRepositorio.BuscarTodosAutoresAsync();

            // Assert
            Assert.Equal(3, resultado.Count());
        }

        [Fact(DisplayName = "BuscarTodosAssuntosAsync - DeveRetornarTodosAssuntos")]
        [Trait("Repositorio", "Unitario")]
        public async Task BuscarTodosAssuntosAsync_DeveRetornarTodosAssuntos()
        {
            // Arrange
            var assuntos = AssuntoFaker.GerarLista(3);
            await _bancoContext.Assuntos.AddRangeAsync(assuntos);
            await _bancoContext.SaveChangesAsync();

            // Act
            var resultado = await _livroRepositorio.BuscarTodosAssuntosAsync();

            // Assert
            Assert.Equal(3, resultado.Count());
        }


    }
}
