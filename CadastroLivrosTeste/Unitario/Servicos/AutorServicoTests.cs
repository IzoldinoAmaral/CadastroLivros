using CadastroLivros.Interfaces.Repositorios;
using CadastroLivros.Models;
using CadastroLivros.Servicos;
using CadastroLivrosTeste.Unitario.Fixture;
using Moq;

namespace CadastroLivrosTeste.Unitario.Servicos
{
    public class AutorServicoTeste
    {
        private readonly Mock<IGenericoRepositorio<Autor>> _autorRepositorio;
        private readonly AutorServico _autorServico;

        public AutorServicoTeste()
        {
            _autorRepositorio = new Mock<IGenericoRepositorio<Autor>>();
            _autorServico = new AutorServico(_autorRepositorio.Object);
        }

        [Fact(DisplayName = "AdicionarAsync - Deve Adicionar Novo Autor")]
        [Trait("Serviço", "Autor Serviço")]
        public async Task AdicionarAsync_DeveAdicionarNovoAutor()
        {
            // Arrange
            var novoAutor = new AutorFaker().Generate();
            _autorRepositorio.Setup(repo => repo.BuscarPorNomeAsync(novoAutor.Nome)).Returns(Task.FromResult(false));

            _autorRepositorio.Setup(repo => repo.AdicionarAsync(novoAutor)).ReturnsAsync(true);
            // Act
            var resultado = await _autorServico.AdicionarAsync(novoAutor);

            // Assert
            _autorRepositorio.Verify(repo => repo.AdicionarAsync(novoAutor), Times.Once);
            Assert.True(resultado);
        }

        [Fact(DisplayName = "AdicionarAsync - Deve Lançar Exceção Se Autor Já Existir")]
        [Trait("Serviço", "Autor Serviço")]
        public async Task AdicionarAsync_DeveLancarExcecaoSeAutorJaExistir()
        {
            // Arrange
            var autorExistente = new AutorFaker().Generate();
            _autorRepositorio.Setup(repo => repo.BuscarPorNomeAsync(autorExistente.Nome)).Returns(Task.FromResult(true));

            // Act 
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _autorServico.AdicionarAsync(autorExistente));

            // Assert
            Assert.Equal("O autor já existe.", exception.Message);
        }

        [Fact(DisplayName = "AtualizarAsync - Deve Atualizar Autor Existente")]
        [Trait("Serviço", "Autor Serviço")]
        public async Task AtualizarAsync_DeveAtualizarAutorExistente()
        {
            // Arrange
            var autorExistente = new AutorFaker().Generate();
            _autorRepositorio.Setup(repo => repo.BuscarPorCodAsync(autorExistente.CodAu)).ReturnsAsync(autorExistente);

            // Act
            var resultado = await _autorServico.AtualizarAsync(autorExistente);

            // Assert
            _autorRepositorio.Verify(repo => repo.Atualizar(autorExistente), Times.Once);
            Assert.True(resultado.Sucesso);
            Assert.Equal(autorExistente, resultado.Autor);
        }

        [Fact(DisplayName = "AtualizarAsync - Deve Retornar Erro Se Autor Não Existir")]
        [Trait("Serviço", "Autor Serviço")]
        public async Task AtualizarAsync_DeveRetornarErroSeAutorNaoExistir()
        {
            // Arrange
            var autorNaoExistente = new AutorFaker().Generate();
            _autorRepositorio.Setup(repo => repo.BuscarPorCodAsync(autorNaoExistente.CodAu)).ReturnsAsync((Autor)null);

            // Act
            var resultado = await _autorServico.AtualizarAsync(autorNaoExistente);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal("Autor não encontrado.", resultado.Mensagem);
        }

        [Fact(DisplayName = "DeletarAsync - Deve Desativar Autor Existente")]
        [Trait("Serviço", "Autor Serviço")]
        public async Task DeletarAsync_DeveDesativarAutorExistente()
        {
            // Arrange
            var autorExistente = new AutorFaker().Generate();
            _autorRepositorio.Setup(repo => repo.BuscarPorCodAsync(autorExistente.CodAu)).ReturnsAsync(autorExistente);

            // Act
            var resultado = await _autorServico.DeletarAsync(autorExistente.CodAu);

            // Assert
            _autorRepositorio.Verify(repo => repo.DeletarAsync(autorExistente), Times.Once);
            Assert.True(resultado);
            Assert.False(autorExistente.Ativo);
        }

        [Fact(DisplayName = "DeletarAsync - Deve Retornar Falso Se Autor Não Existir")]
        [Trait("Serviço", "Autor Serviço")]
        public async Task DeletarAsync_DeveRetornarFalsoSeAutorNaoExistir()
        {
            // Arrange
            var autorNaoExistente = new AutorFaker().Generate();
            _autorRepositorio.Setup(repo => repo.BuscarPorCodAsync(autorNaoExistente.CodAu)).ReturnsAsync((Autor)null);

            // Act
            var resultado = await _autorServico.DeletarAsync(autorNaoExistente.CodAu);

            // Assert
            Assert.False(resultado);
        }

        [Fact(DisplayName = "BuscarTodosAsync - Deve Retornar Todos os Autores")]
        [Trait("Serviço", "Autor Serviço")]
        public async Task BuscarTodosAsync_DeveRetornarTodosOsAutores()
        {
            // Arrange
            var listaDeAutores = AutorFaker.GerarLista(5);

            _autorRepositorio.Setup(repo => repo.BuscarTodosAsync()).ReturnsAsync(listaDeAutores);

            // Act
            var resultado = await _autorServico.BuscarTodosAsync();

            // Assert
            _autorRepositorio.Verify(repo => repo.BuscarTodosAsync(), Times.Once);
            Assert.NotNull(resultado);
            Assert.Equal(5, resultado.Count());
            Assert.Equal(listaDeAutores, resultado);
        }

    }
}
