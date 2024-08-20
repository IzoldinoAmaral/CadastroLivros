using CadastroLivros.Data.Repositorio;
using CadastroLivros.Data;
using Microsoft.EntityFrameworkCore;
using CadastroLivrosTeste.Unitario.Fixture;

namespace CadastroLivrosTeste.Unitario.Data.Repositorio
{
    public class FormaCompraRepositorioTests : IDisposable
    {
        private readonly FormaCompraRepositorio _formaCompraRepositorio;
        private readonly BancoContext _bancoContext;
        private readonly FormaCompraFaker _formaCompraFaker;

        public FormaCompraRepositorioTests()
        {
            var options = new DbContextOptionsBuilder<BancoContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _bancoContext = new BancoContext(options);
            _bancoContext.Database.EnsureCreated();
            _formaCompraRepositorio = new FormaCompraRepositorio(_bancoContext);

            _formaCompraFaker = new FormaCompraFaker();
        }

        public void Dispose()
        {
            _bancoContext.Dispose();
        }

        [Fact(DisplayName = "AdicionarAsync_DeveAdicionarFormaCompra")]
        [Trait("Repositório", "Forma Compra Repositorio")]
        public async Task AdicionarAsync_DeveAdicionarFormaCompra()
        {
            // Arrange
            var formaCompra = _formaCompraFaker.Generate();

            // Act
            var result = await _formaCompraRepositorio.AdicionarAsync(formaCompra);
            var formaCompraNoBanco = await _bancoContext.FormaCompras.FindAsync(formaCompra.CodCom);

            // Assert
            Assert.True(result);
            Assert.NotNull(formaCompraNoBanco);
            Assert.Equal(formaCompra.CodCom, formaCompraNoBanco.CodCom);
        }

        [Fact(DisplayName = "Atualizar_DeveAtualizarFormaCompra")]
        [Trait("Repositório", "FormaCompraRepositorio")]
        public async Task Atualizar_DeveAtualizarFormaCompra()
        {
            // Arrange
            var formaCompra = _formaCompraFaker.Generate();
            await _formaCompraRepositorio.AdicionarAsync(formaCompra);

            formaCompra.Descricao = "Novo Descrição";

            // Act
            var updatedFormaCompra = await _formaCompraRepositorio.Atualizar(formaCompra);
            var formaCompraNoBanco = await _bancoContext.FormaCompras.FindAsync(formaCompra.CodCom);

            // Assert
            Assert.Equal(formaCompra, updatedFormaCompra);
            Assert.Equal("Novo Descrição", formaCompraNoBanco.Descricao);
        }

        [Fact(DisplayName = "BuscarPorNomeAsync_DeveRetornarTrueSeDescricaoExistir")]
        [Trait("Repositório", "FormaCompraRepositorio")]
        public async Task BuscarPorNomeAsync_DeveRetornarTrueSeDescricaoExistir()
        {
            // Arrange
            var formaCompra = _formaCompraFaker.Generate();
            await _formaCompraRepositorio.AdicionarAsync(formaCompra);

            // Act
            var resultado = await _formaCompraRepositorio.BuscarPorNomeAsync(formaCompra.Descricao);

            // Assert
            Assert.True(resultado);
        }

        [Fact(DisplayName = "BuscarPorCodAsync_DeveRetornarFormaCompraSeCodExistir")]
        [Trait("Repositório", "FormaCompraRepositorio")]
        public async Task BuscarPorCodAsync_DeveRetornarFormaCompraSeCodExistir()
        {
            // Arrange
            var formaCompra = _formaCompraFaker.Generate();
            await _formaCompraRepositorio.AdicionarAsync(formaCompra);

            // Act
            var resultado = await _formaCompraRepositorio.BuscarPorCodAsync(formaCompra.CodCom);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(formaCompra.CodCom, resultado.CodCom);
        }

        [Fact(DisplayName = "BuscarTodosAsync_DeveRetornarTodasFormaCompras")]
        [Trait("Repositório", "FormaCompraRepositorio")]
        public async Task BuscarTodosAsync_DeveRetornarTodasFormaCompras()
        {
            // Arrange
            var formaCompra1 = _formaCompraFaker.Generate();
            var formaCompra2 = _formaCompraFaker.Generate();
            await _formaCompraRepositorio.AdicionarAsync(formaCompra1);
            await _formaCompraRepositorio.AdicionarAsync(formaCompra2);

            // Act
            var resultado = await _formaCompraRepositorio.BuscarTodosAsync();

            // Assert
            Assert.Contains(formaCompra1, resultado);
            Assert.Contains(formaCompra2, resultado);
        }

        [Fact(DisplayName = "DeletarAsync_DeveRemoverFormaCompra")]
        [Trait("Repositório", "FormaCompraRepositorio")]
        public async Task DeletarAsync_DeveRemoverFormaCompra()
        {
            // Arrange
            var formaCompra = _formaCompraFaker.Generate();
            await _formaCompraRepositorio.AdicionarAsync(formaCompra);

            // Act
            var result = await _formaCompraRepositorio.DeletarAsync(formaCompra);
            var formaCompraNoBanco = await _bancoContext.FormaCompras.FindAsync(formaCompra.CodCom);

            // Assert
            Assert.True(result);
            Assert.Null(formaCompraNoBanco);
        }

    }
}
