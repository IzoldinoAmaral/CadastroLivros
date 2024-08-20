using CadastroLivros.Interfaces.Repositorios;
using CadastroLivros.Models;
using CadastroLivros.Servicos;
using CadastroLivrosTeste.Unitario.Fixture;
using Moq;

namespace CadastroLivrosTeste.Unitario.Servicos
{
    public class FormaCompraServicoTests
    {
        private readonly Mock<IGenericoRepositorio<FormaCompra>> _formaCompraRepositorio;
        private readonly FormaCompraServico _formaCompraServico;
        private readonly FormaCompraFaker _formaCompraFaker;

        public FormaCompraServicoTests()
        {            
            _formaCompraRepositorio = new Mock<IGenericoRepositorio<FormaCompra>>();
            _formaCompraServico = new FormaCompraServico(_formaCompraRepositorio.Object);

            _formaCompraFaker = new FormaCompraFaker();
        }

        [Fact(DisplayName = "AdicionarAsync_DeveAdicionarFormaCompraQuandoNaoExistente")]
        [Trait("Servico", "Servico FormaCompra ")]
        public async Task AdicionarAsync_DeveAdicionarFormaCompraQuandoNaoExistente()
        {
            // Arrange
            var novaFormaCompra = _formaCompraFaker.Generate();

            _formaCompraRepositorio.Setup(repo => repo.BuscarPorNomeAsync(novaFormaCompra.Descricao)).ReturnsAsync(false);

            _formaCompraRepositorio.Setup(repo => repo.AdicionarAsync(novaFormaCompra)).ReturnsAsync(true);

            // Act
            var resultado = await _formaCompraServico.AdicionarAsync(novaFormaCompra);

            // Assert
            Assert.True(resultado);
            _formaCompraRepositorio.Verify(repo => repo.AdicionarAsync(novaFormaCompra), Times.Once);
        }

        [Fact(DisplayName = "AdicionarAsync_DeveLancarExcecaoQuandoFormaCompraJaExistir")]
        [Trait("Servico", "Servico FormaCompra")]
        public async Task AdicionarAsync_DeveLancarExcecaoQuandoFormaCompraJaExistir()
        {
            // Arrange
            var formaCompraExistente = _formaCompraFaker.Generate();

            _formaCompraRepositorio.Setup(repo => repo.BuscarPorNomeAsync(formaCompraExistente.Descricao)).ReturnsAsync(true);

            // Act & Assert
            var excecao = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _formaCompraServico.AdicionarAsync(formaCompraExistente)
            );

            Assert.Equal("A forma de Compra já existe.", excecao.Message);
            _formaCompraRepositorio.Verify(repo => repo.AdicionarAsync(It.IsAny<FormaCompra>()), Times.Never);
        }

        [Fact(DisplayName = "AtualizarAsync_DeveRetornarSucessoQuandoFormaCompraExistir")]
        [Trait("Servico", "Servico FormaCompra")]
        public async Task AtualizarAsync_DeveRetornarSucessoQuandoFormaCompraExistir()
        {
            // Arrange
            var formaCompraExistente = _formaCompraFaker.Generate();

            _formaCompraRepositorio.Setup(repo => repo.BuscarPorCodAsync(formaCompraExistente.CodCom)).ReturnsAsync(formaCompraExistente);

            _formaCompraRepositorio.Setup(repo => repo.Atualizar(It.IsAny<FormaCompra>())).ReturnsAsync(formaCompraExistente);

            // Act
            var resultado = await _formaCompraServico.AtualizarAsync(formaCompraExistente);

            // Assert
            Assert.True(resultado.Sucesso);
            Assert.Equal(formaCompraExistente, resultado.FormaCompra);
            _formaCompraRepositorio.Verify(repo => repo.Atualizar(formaCompraExistente), Times.Once);
        }

        [Fact(DisplayName = "AtualizarAsync_DeveRetornarErroQuandoFormaCompraNaoExistir")]
        [Trait("Servico", "Servico FormaCompra")]
        public async Task AtualizarAsync_DeveRetornarErroQuandoFormaCompraNaoExistir()
        {
            // Arrange
            var formaCompraNaoExistente = _formaCompraFaker.Generate();

            _formaCompraRepositorio.Setup(repo => repo.BuscarPorCodAsync(formaCompraNaoExistente.CodCom)).ReturnsAsync((FormaCompra)null);

            // Act
            var resultado = await _formaCompraServico.AtualizarAsync(formaCompraNaoExistente);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal("Forma de Compra não encontrado.", resultado.Mensagem);
            _formaCompraRepositorio.Verify(repo => repo.Atualizar(It.IsAny<FormaCompra>()), Times.Never);
        }

        [Fact(DisplayName = "BuscarPorCodAsync_DeveRetornarFormaCompraQuandoExistir")]
        [Trait("Servico", "Servico FormaCompra")]
        public async Task BuscarPorCodAsync_DeveRetornarFormaCompraQuandoExistir()
        {
            // Arrange
            var formaCompra = _formaCompraFaker.Generate();

            _formaCompraRepositorio.Setup(repo => repo.BuscarPorCodAsync(formaCompra.CodCom)).ReturnsAsync(formaCompra);

            // Act
            var resultado = await _formaCompraServico.BuscarPorCodAsync(formaCompra.CodCom);

            // Assert
            Assert.Equal(formaCompra, resultado);
            _formaCompraRepositorio.Verify(repo => repo.BuscarPorCodAsync(formaCompra.CodCom), Times.Once);
        }

        [Fact(DisplayName = "BuscarPorCodAsync_DeveRetornarNuloQuandoNaoExistir")]
        [Trait("Servico", "Servico FormaCompra")]
        public async Task BuscarPorCodAsync_DeveRetornarNuloQuandoNaoExistir()
        {
            // Arrange
            var codInexistente = 999;

            _formaCompraRepositorio.Setup(repo => repo.BuscarPorCodAsync(codInexistente)).ReturnsAsync((FormaCompra)null);

            // Act
            var resultado = await _formaCompraServico.BuscarPorCodAsync(codInexistente);

            // Assert
            Assert.Null(resultado);
            _formaCompraRepositorio.Verify(repo => repo.BuscarPorCodAsync(codInexistente), Times.Once);
        }

        [Fact(DisplayName = "BuscarPorNomeAsync_DeveRetornarSucessoQuandoExistir")]
        [Trait("Servico", "Servico FormaCompra")]
        public async Task BuscarPorNomeAsync_DeveRetornarSucessoQuandoExistir()
        {
            // Arrange
            var formaCompra = _formaCompraFaker.Generate();

            _formaCompraRepositorio.Setup(repo => repo.BuscarPorNomeAsync(formaCompra.Descricao)).ReturnsAsync(true);

            // Act
            var resultado = await _formaCompraServico.BuscarPorNomeAsync(formaCompra.Descricao);

            // Assert
            Assert.True(resultado.Sucesso);
            _formaCompraRepositorio.Verify(repo => repo.BuscarPorNomeAsync(formaCompra.Descricao), Times.Once);
        }

        [Fact(DisplayName = "BuscarPorNomeAsync_DeveRetornarErroQuandoNaoExistir")]
        [Trait("Servico", "Servico FormaCompra")]
        public async Task BuscarPorNomeAsync_DeveRetornarErroQuandoNaoExistir()
        {
            // Arrange
            var nomeInexistente = "FormaCompraInexistente";

            _formaCompraRepositorio.Setup(repo => repo.BuscarPorNomeAsync(nomeInexistente)).ReturnsAsync(false);

            // Act
            var resultado = await _formaCompraServico.BuscarPorNomeAsync(nomeInexistente);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal("Forma de Compra não encontrado.", resultado.Mensagem);
            _formaCompraRepositorio.Verify(repo => repo.BuscarPorNomeAsync(nomeInexistente), Times.Once);
        }

        [Fact(DisplayName = "BuscarTodosAsync_DeveRetornarTodasAsFormasDeCompra")]
        [Trait("Servico", "Servico FormaCompra")]
        public async Task BuscarTodosAsync_DeveRetornarTodasAsFormasDeCompra()
        {
            // Arrange
            var formasCompra = _formaCompraFaker.Generate(3);

            _formaCompraRepositorio.Setup(repo => repo.BuscarTodosAsync()).ReturnsAsync(formasCompra);

            // Act
            var resultado = await _formaCompraServico.BuscarTodosAsync();

            // Assert
            Assert.Equal(formasCompra, resultado);
            _formaCompraRepositorio.Verify(repo => repo.BuscarTodosAsync(), Times.Once);
        }

        [Fact(DisplayName = "DeletarAsync_DeveDeletarFormaCompraQuandoExistir")]
        [Trait("Servico", "Servico FormaCompra")]
        public async Task DeletarAsync_DeveDeletarFormaCompraQuandoExistir()
        {
            // Arrange
            var formaCompra = _formaCompraFaker.Generate();

            _formaCompraRepositorio.Setup(repo => repo.BuscarPorCodAsync(formaCompra.CodCom)).ReturnsAsync(formaCompra);

            _formaCompraRepositorio.Setup(repo => repo.DeletarAsync(formaCompra)).ReturnsAsync(true);

            // Act
            var resultado = await _formaCompraServico.DeletarAsync(formaCompra.CodCom);

            // Assert
            Assert.True(resultado);
            _formaCompraRepositorio.Verify(repo => repo.DeletarAsync(formaCompra), Times.Once);
        }

        [Fact(DisplayName = "DeletarAsync_DeveRetornarFalsoQuandoFormaCompraNaoExistir")]
        [Trait("Servico", "Servico FormaCompra")]
        public async Task DeletarAsync_DeveRetornarFalsoQuandoFormaCompraNaoExistir()
        {
            // Arrange
            var codInexistente = 999;

            _formaCompraRepositorio.Setup(repo => repo.BuscarPorCodAsync(codInexistente)).ReturnsAsync((FormaCompra)null);

            // Act
            var resultado = await _formaCompraServico.DeletarAsync(codInexistente);

            // Assert
            Assert.False(resultado);
            _formaCompraRepositorio.Verify(repo => repo.DeletarAsync(It.IsAny<FormaCompra>()), Times.Never);
        }
    }
}
