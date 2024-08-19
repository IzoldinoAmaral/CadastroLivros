using CadastroLivros.Interfaces.Repositorios;
using CadastroLivros.Models;
using CadastroLivros.Servicos;
using CadastroLivrosTeste.Unitario.Fixture;
using Moq;

namespace CadastroLivrosTeste.Unitario.Servicos
{
    public class AssuntoServicoTests
    {
        private readonly Mock<IGenericoRepositorio<Assunto>> _assuntoRepositorio;
        private readonly AssuntoServico _assuntoServico;

        public AssuntoServicoTests()
        {
            _assuntoRepositorio = new Mock<IGenericoRepositorio<Assunto>>();
            _assuntoServico = new AssuntoServico(_assuntoRepositorio.Object);
        }

        [Fact(DisplayName = "AdicionarAsync deve lançar exceção se o assunto já existir")]
        [Trait("Serviços", "Assunto Serviço")]
        public async Task AdicionarAsync_DeveLancarExcecao_SeAssuntoExistir()
        {
            // Arrange
            var assunto = AssuntoFaker.GerarAssunto();
            _assuntoRepositorio.Setup(r => r.BuscarPorNomeAsync(assunto.Descricao)).ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _assuntoServico.AdicionarAsync(assunto));
            Assert.Equal("O assunto já existe.", exception.Message);
        }

        [Fact(DisplayName = "AdicionarAsync deve adicionar assunto se ele não existir")]
        [Trait("Serviços", "Assunto Serviço")]
        public async Task AdicionarAsync_DeveAdicionarAssunto_SeNaoExistir()
        {
            // Arrange
            var assunto = AssuntoFaker.GerarAssunto();
            _assuntoRepositorio.Setup(r => r.BuscarPorNomeAsync(assunto.Descricao)).ReturnsAsync(false);
            _assuntoRepositorio.Setup(r => r.AdicionarAsync(assunto)).ReturnsAsync(true);

            // Act
            var resultado = await _assuntoServico.AdicionarAsync(assunto);

            // Assert
            Assert.True(resultado);
            _assuntoRepositorio.Verify(r => r.AdicionarAsync(assunto), Times.Once);
        }

        [Fact(DisplayName = "AtualizarAsync deve retornar false se o assunto não for encontrado")]
        [Trait("Serviços", "Assunto Serviço")]
        public async Task AtualizarAsync_DeveRetornarFalse_SeAssuntoNaoForEncontrado()
        {
            // Arrange
            var assunto = AssuntoFaker.GerarAssunto();
            _assuntoRepositorio.Setup(r => r.BuscarPorCodAsync(assunto.CodAs)).ReturnsAsync((Assunto)null);

            // Act
            var resultado = await _assuntoServico.AtualizarAsync(assunto);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal("Assunto não encontrado.", resultado.Mensagem);
        }

        [Fact(DisplayName = "AtualizarAsync deve atualizar o assunto se ele for encontrado")]
        [Trait("Serviços", "Assunto Serviço")]
        public async Task AtualizarAsync_DeveAtualizarAssunto_SeForEncontrado()
        {
            // Arrange
            var assunto = AssuntoFaker.GerarAssunto();
            _assuntoRepositorio.Setup(r => r.BuscarPorCodAsync(assunto.CodAs)).ReturnsAsync(assunto);
            _assuntoRepositorio.Setup(r => r.Atualizar(assunto)).Returns(Task.FromResult(assunto));

            // Act
            var resultado = await _assuntoServico.AtualizarAsync(assunto);

            // Assert
            Assert.True(resultado.Sucesso);
            Assert.Equal(assunto, resultado.Assunto);
            _assuntoRepositorio.Verify(r => r.Atualizar(assunto), Times.Once);
        }

        [Fact(DisplayName = "BuscarPorCodAsync deve retornar o assunto correto")]
        [Trait("Serviços", "Assunto Serviço")]
        public async Task BuscarPorCodAsync_DeveRetornarAssuntoCorreto()
        {
            // Arrange
            var assunto = AssuntoFaker.GerarAssunto();
            _assuntoRepositorio.Setup(r => r.BuscarPorCodAsync(assunto.CodAs)).ReturnsAsync(assunto);

            // Act
            var resultado = await _assuntoServico.BuscarPorCodAsync(assunto.CodAs);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(assunto.CodAs, resultado.CodAs);
            _assuntoRepositorio.Verify(r => r.BuscarPorCodAsync(assunto.CodAs), Times.Once);
        }

        [Fact(DisplayName = "DeletarAsync deve retornar false se o assunto não for encontrado")]
        [Trait("Serviços", "Assunto Serviço")]
        public async Task DeletarAsync_DeveRetornarFalse_SeAssuntoNaoForEncontrado()
        {
            // Arrange
            var codAs = 10; 
            _assuntoRepositorio.Setup(r => r.BuscarPorCodAsync(codAs)).ReturnsAsync((Assunto)null);

            // Act
            var resultado = await _assuntoServico.DeletarAsync(codAs);

            // Assert
            Assert.False(resultado);
            _assuntoRepositorio.Verify(r => r.BuscarPorCodAsync(codAs), Times.Once);
            _assuntoRepositorio.Verify(r => r.DeletarAsync(It.IsAny<Assunto>()), Times.Never);
        }

        [Fact(DisplayName = "DeletarAsync deve desativar o assunto se for encontrado")]
        [Trait("Serviços", "Assunto Serviço")]
        public async Task DeletarAsync_DeveDesativarAssunto_SeForEncontrado()
        {
            // Arrange
            var assunto = AssuntoFaker.GerarAssunto();
            _assuntoRepositorio.Setup(r => r.BuscarPorCodAsync(assunto.CodAs)).ReturnsAsync(assunto);
            _assuntoRepositorio.Setup(r => r.DeletarAsync(assunto)).Returns(Task.FromResult(true));

            // Act
            var resultado = await _assuntoServico.DeletarAsync(assunto.CodAs);

            // Assert
            Assert.True(resultado);
            Assert.False(assunto.Ativo); 
            _assuntoRepositorio.Verify(r => r.DeletarAsync(assunto), Times.Once);
        }

        [Fact(DisplayName = "BuscarTodosAsync deve retornar todos os assuntos")]
        [Trait("Categoria", "Serviços")]
        public async Task BuscarTodosAsync_DeveRetornarTodosAssuntos()
        {
            // Arrange
            var listaDeAssuntos = AssuntoFaker.GerarLista(5);
            _assuntoRepositorio.Setup(r => r.BuscarTodosAsync()).ReturnsAsync(listaDeAssuntos);

            // Act
            var resultado = await _assuntoServico.BuscarTodosAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(5, resultado.Count());
            _assuntoRepositorio.Verify(r => r.BuscarTodosAsync(), Times.Once);
        }
    }
}
