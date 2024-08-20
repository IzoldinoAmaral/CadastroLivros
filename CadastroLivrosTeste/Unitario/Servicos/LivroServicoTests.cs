using CadastroLivros.Extensions;
using CadastroLivros.Interfaces.Repositorios;
using CadastroLivros.Interfaces.Servicos;
using CadastroLivros.Models;
using CadastroLivros.Servicos;
using CadastroLivrosTeste.Unitario.Fixture;
using Moq;

namespace CadastroLivrosTeste.Unitario.Servicos
{
    public class LivroServicoTests
    {
        private readonly Mock<ILivroRepositorio> _livroRepositorio;
        private readonly Mock<IFormaCompraServico> _formaCompraServico;
        private readonly LivroServico _livroServico;

        public LivroServicoTests()
        {
            _livroRepositorio = new Mock<ILivroRepositorio>();
            _formaCompraServico = new Mock<IFormaCompraServico>();
            _livroServico = new LivroServico(_livroRepositorio.Object, _formaCompraServico.Object);
        }

        [Fact(DisplayName = "BuscarTodosAssuntosAsync deve retornar todos os assuntos")]
        [Trait("Categoria", "BuscarTodosAssuntos")]
        public async Task BuscarTodosAssuntosAsync_DeveRetornarTodosAssuntos()
        {
            // Arrange
            var listaDeAssuntos = AssuntoFaker.GerarLista(3);
            _livroRepositorio.Setup(x => x.BuscarTodosAssuntosAsync()).ReturnsAsync(listaDeAssuntos);

            // Act
            var resultado = await _livroServico.BuscarTodosAssuntosAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(3, resultado.Count());
            _livroRepositorio.Verify(x => x.BuscarTodosAssuntosAsync(), Times.Once);
        }

        [Fact(DisplayName = "BuscarTodosAssuntosAsync deve retornar lista vazia se nenhum assunto for encontrado")]
        [Trait("Servico", "BuscarTodosAssuntos")]
        public async Task BuscarTodosAssuntosAsync_DeveRetornarListaVazia_SeNenhumAssuntoForEncontrado()
        {
            // Arrange
            _livroRepositorio.Setup(x => x.BuscarTodosAssuntosAsync()).ReturnsAsync(new List<Assunto>());

            // Act
            var resultado = await _livroServico.BuscarTodosAssuntosAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Empty(resultado);
            _livroRepositorio.Verify(x => x.BuscarTodosAssuntosAsync(), Times.Once);
        }

        [Fact(DisplayName = "BuscarTodosAsync deve retornar todos os livros")]
        [Trait("Servico", "Buscar Todos")]
        public async Task BuscarTodosAsync_DeveRetornarTodosLivros()
        {
            // Arrange
            var listaDeLivros = LivroFaker.Gerar(3);
            _livroRepositorio.Setup(x => x.BuscarTodosAsync()).ReturnsAsync(listaDeLivros);

            // Act
            var resultado = await _livroServico.BuscarTodosAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(3, resultado.Count());
            _livroRepositorio.Verify(x => x.BuscarTodosAsync(), Times.Once);
        }

        [Fact(DisplayName = "BuscarTodosAsync deve retornar lista vazia se nenhum livro for encontrado")]
        [Trait("Servico", "BuscarTodos")]
        public async Task BuscarTodosAsync_DeveRetornarListaVazia_SeNenhumLivroForEncontrado()
        {
            // Arrange
            _livroRepositorio.Setup(x => x.BuscarTodosAsync()).ReturnsAsync(new List<Livro>());

            // Act
            var resultado = await _livroServico.BuscarTodosAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Empty(resultado);
            _livroRepositorio.Verify(x => x.BuscarTodosAsync(), Times.Once);
        }

        [Fact(DisplayName = "BuscarTodosAutoresAsync deve retornar todos os autores")]
        [Trait("Servico", "BuscarTodosAutores")]
        public async Task BuscarTodosAutoresAsync_DeveRetornarTodosAutores()
        {
            // Arrange
            var listaDeAutores = AutorFaker.GerarLista(3);
            _livroRepositorio.Setup(x => x.BuscarTodosAutoresAsync()).ReturnsAsync(listaDeAutores);

            // Act
            var resultado = await _livroServico.BuscarTodosAutoresAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(3, resultado.Count());
            _livroRepositorio.Verify(x => x.BuscarTodosAutoresAsync(), Times.Once);
        }

        [Fact(DisplayName = "BuscarTodosAutoresAsync deve retornar lista vazia se nenhum autor for encontrado")]
        [Trait("Servico", "BuscarTodosAutores")]
        public async Task BuscarTodosAutoresAsync_DeveRetornarListaVazia_SeNenhumAutorForEncontrado()
        {
            // Arrange
            _livroRepositorio.Setup(x => x.BuscarTodosAutoresAsync()).ReturnsAsync(new List<Autor>());

            // Act
            var resultado = await _livroServico.BuscarTodosAutoresAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Empty(resultado);
            _livroRepositorio.Verify(x => x.BuscarTodosAutoresAsync(), Times.Once);
        }

        [Fact(DisplayName = "AdicionarAsync deve adicionar livro quando não existir livro com o mesmo título")]
        [Trait("Servico", "Adicionar")]
        public async Task AdicionarAsync_DeveAdicionarLivro_QuandoNaoExistirLivroComMesmoTitulo()
        {
            // Arrange
            var novoLivro = LivroFaker.GerarComTodosCamposPreenchidos();
            _livroRepositorio.Setup(x => x.BuscarPorNomeAsync(It.IsAny<string>())).Returns(Task.FromResult(false));

            _livroRepositorio.Setup(x => x.AdicionarAsync(It.IsAny<Livro>())).ReturnsAsync(true);

            // Act
            var resultado = await _livroServico.AdicionarAsync(novoLivro);

            // Assert
            Assert.True(resultado);
            _livroRepositorio.Verify(x => x.AdicionarAsync(novoLivro), Times.Once);
        }

        [Fact(DisplayName = "AdicionarAsync deve lançar exceção quando já existir livro com o mesmo título")]
        [Trait("Servico", "Adicionar")]
        public async Task AdicionarAsync_DeveLancarExcecao_QuandoJaExistirLivroComMesmoTitulo()
        {
            // Arrange
            var livroExistente = LivroFaker.GerarComTodosCamposPreenchidos();
            _livroRepositorio.Setup(x => x.BuscarPorNomeAsync(It.IsAny<string>())).ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _livroServico.AdicionarAsync(livroExistente));
            Assert.Equal("O livro já existe.", exception.Message);
            _livroRepositorio.Verify(x => x.AdicionarAsync(It.IsAny<Livro>()), Times.Never);
        }

        [Fact(DisplayName = "AtualizarAsync deve atualizar o livro quando ele existir")]
        [Trait("Servico", "Atualizar")]
        public async Task AtualizarAsync_DeveAtualizarLivro_QuandoEleExistir()
        {
            // Arrange
            var livroExistente = LivroFaker.GerarComTodosCamposPreenchidos();
            var livroAtualizado = LivroFaker.GerarComTodosCamposPreenchidos();
            livroAtualizado.Codl = livroExistente.Codl;

            _livroRepositorio.Setup(x => x.BuscarPorCodAsync(It.IsAny<int>())).ReturnsAsync(livroExistente);

            _livroRepositorio.Setup(x => x.Atualizar(It.IsAny<Livro>())).Verifiable();

            // Act
            var resultado = await _livroServico.AtualizarAsync(livroAtualizado);

            // Assert
            Assert.True(resultado.Sucesso);
            Assert.Equal(livroAtualizado.Titulo, resultado.Livro.Titulo);
            _livroRepositorio.Verify(x => x.Atualizar(It.IsAny<Livro>()), Times.Once);
        }

        [Fact(DisplayName = "AtualizarAsync deve retornar resultado com falha quando o livro não for encontrado")]
        [Trait("Servico", "Atualizar")]
        public async Task AtualizarAsync_DeveRetornarFalha_QuandoLivroNaoForEncontrado()
        {
            // Arrange
            var livroAtualizado = LivroFaker.GerarComTodosCamposPreenchidos();
            _livroRepositorio.Setup(x => x.BuscarPorCodAsync(It.IsAny<int>())).ReturnsAsync((Livro)null);

            // Act
            var resultado = await _livroServico.AtualizarAsync(livroAtualizado);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal("Livro não encontrado.", resultado.Mensagem);
            _livroRepositorio.Verify(x => x.Atualizar(It.IsAny<Livro>()), Times.Never);
        }

        [Fact(DisplayName = "BuscarPorCodAsync deve retornar o livro quando encontrado")]
        [Trait("Servico", "BuscarPorCod")]
        public async Task BuscarPorCodAsync_DeveRetornarLivro_QuandoEncontrado()
        {
            // Arrange
            var livroExistente = LivroFaker.GerarComTodosCamposPreenchidos();
            _livroRepositorio.Setup(x => x.BuscarPorCodAsync(It.IsAny<int>())).ReturnsAsync(livroExistente);

            // Act
            var resultado = await _livroServico.BuscarPorCodAsync(livroExistente.Codl);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(livroExistente.Codl, resultado.Codl);
        }

        [Fact(DisplayName = "BuscarPorCodAsync deve retornar null quando o livro não for encontrado")]
        [Trait("Servico", "BuscarPorCod")]
        public async Task BuscarPorCodAsync_DeveRetornarNull_QuandoLivroNaoForEncontrado()
        {
            // Arrange
            _livroRepositorio.Setup(x => x.BuscarPorCodAsync(It.IsAny<int>())).ReturnsAsync((Livro)null);

            // Act
            var resultado = await _livroServico.BuscarPorCodAsync(1);

            // Assert
            Assert.Null(resultado);
        }

        [Fact(DisplayName = "DeletarAsync deve deletar o livro quando ele for encontrado")]
        [Trait("Servico", "Deletar")]
        public async Task DeletarAsync_DeveDeletarLivro_QuandoEleForEncontrado()
        {
            // Arrange
            var livroExistente = LivroFaker.GerarComTodosCamposPreenchidos();
            _livroRepositorio.Setup(x => x.BuscarPorCodAsync(It.IsAny<int>())).ReturnsAsync(livroExistente);

            _livroRepositorio.Setup(x => x.DeletarAsync(It.IsAny<Livro>())).ReturnsAsync(true).Verifiable();

            // Act
            var resultado = await _livroServico.DeletarAsync(livroExistente.Codl);

            // Assert
            Assert.True(resultado);
            _livroRepositorio.Verify(x => x.DeletarAsync(It.IsAny<Livro>()), Times.Once);
        }

        [Fact(DisplayName = "DeletarAsync deve retornar false quando o livro não for encontrado")]
        [Trait("Servico", "Deletar")]
        public async Task DeletarAsync_DeveRetornarFalse_QuandoLivroNaoForEncontrado()
        {
            // Arrange
            _livroRepositorio.Setup(x => x.BuscarPorCodAsync(It.IsAny<int>())).ReturnsAsync((Livro)null);

            // Act
            var resultado = await _livroServico.DeletarAsync(1);

            // Assert
            Assert.False(resultado);
            _livroRepositorio.Verify(x => x.DeletarAsync(It.IsAny<Livro>()), Times.Never);
        }

        [Fact(DisplayName = "ListarDetalhesAsync deve retornar os detalhes corretos do livro")]
        [Trait("Servico", "ListarDetalhes")]
        public async Task ListarDetalhesAsync_DeveRetornarDetalhesCorretosDoLivro()
        {
            // Arrange
            var livroExistente = LivroFaker.GerarComTodosCamposPreenchidos();
            var formasDeCompra = new List<FormaCompra>
            {
                new FormaCompra { Descricao = "E-book", Desconto = 0.1m },
                new FormaCompra { Descricao = "Impresso", Desconto = 0.2m }
            };

            _livroRepositorio.Setup(x => x.BuscarPorCodAsync(It.IsAny<int>())).ReturnsAsync(livroExistente);

            _formaCompraServico.Setup(x => x.BuscarTodosAsync()).ReturnsAsync(formasDeCompra);

            // Act
            var detalhes = await _livroServico.ListarDetalhesAsync(livroExistente.Codl);

            // Assert
            Assert.NotNull(detalhes);
            Assert.Equal(livroExistente.Titulo, detalhes.NomeLivro);
            Assert.Equal(2, detalhes.Precos.Count);
            Assert.Equal("E-book", detalhes.Precos[0].FormaCompra);
            Assert.Equal(livroExistente.PrecoBase.ComDesconto(0.1m), detalhes.Precos[0].ValorFinal);
        }
    }
}
