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

        [Fact(DisplayName = "ObterPorNomeAsync - Deve Retornar Autor Pelo Nome")]
        [Trait("Serviço", "Obter por nome")]
        public async Task ObterPorNomeAsync_DeveRetornarAutorPeloNome()
        {
            // Arrange
            var nomeAutor = "Autor Teste";
            var autorEsperado = new AutorFaker().Generate();
            autorEsperado.Nome = nomeAutor;

            _autorRepositorio
                .Setup(repo => repo.ObterPorNomeAsync(nomeAutor))
                .ReturnsAsync(autorEsperado);

            // Act
            var resultado = await _autorServico.ObterPorNomeAsync(nomeAutor);

            // Assert
            _autorRepositorio.Verify(repo => repo.ObterPorNomeAsync(nomeAutor), Times.Once);
            Assert.NotNull(resultado);
            Assert.Equal(nomeAutor, resultado.Nome);
        }

        [Fact(DisplayName = "AdicionarAsync - Deve Ativar Autor Se Já Existir")]
        [Trait("Categoria", "Serviço - Autor")]
        public async Task AdicionarAsync_DeveAtivarAutorSeJaExistir()
        {
            // Arrange
            var autorExistente = new AutorFaker().Generate();

            _autorRepositorio.Setup(repo => repo.BuscarPorNomeAsync(autorExistente.Nome)).ReturnsAsync(true);

            _autorRepositorio.Setup(repo => repo.ObterPorNomeAsync(autorExistente.Nome)).ReturnsAsync(autorExistente);

            _autorRepositorio.Setup(repo => repo.Atualizar(autorExistente)).ReturnsAsync(autorExistente);

            // Act
            var resultado = await _autorServico.AdicionarAsync(autorExistente);

            // Assert
            _autorRepositorio.Verify(repo => repo.BuscarPorNomeAsync(autorExistente.Nome), Times.Once);
            _autorRepositorio.Verify(repo => repo.ObterPorNomeAsync(autorExistente.Nome), Times.Once);
            _autorRepositorio.Verify(repo => repo.Atualizar(It.Is<Autor>(a => a.Ativo == true)), Times.Once);
            _autorRepositorio.Verify(repo => repo.AdicionarAsync(It.IsAny<Autor>()), Times.Never);
            Assert.True(resultado);
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
