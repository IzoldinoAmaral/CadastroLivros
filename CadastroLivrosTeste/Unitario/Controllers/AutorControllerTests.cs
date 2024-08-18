using CadastroLivros.Controllers;
using CadastroLivros.Interfaces.Servicos;
using CadastroLivros.Models;
using CadastroLivros.Types;
using CadastroLivrosTeste.Unitario.Fixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;

namespace CadastroLivrosTeste.Unitario.Controllers
{
    public class AutorControllerTests
    {
        private AutorController _autorControllerTest { get; set; }
        private readonly Mock<IAutorServico> _autorServico = new();

        public AutorControllerTests()
        {
            _autorControllerTest = new AutorController(_autorServico.Object);
        }

        [Fact(DisplayName = "Index - Deve Retornar View com Lista de Autores")]
        [Trait("Controller Autor", " Index")]
        public async Task Index_DeveRetornarViewComListaDeAutores()
        {
            // Arrange
            var autores = AutorFaker.GerarLista(3);
            _autorServico.Setup(s => s.BuscarTodosAsync()).ReturnsAsync(autores);

            // Act
            var result = await _autorControllerTest.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Autor>>(viewResult.Model);
            Assert.Equal(3, model.Count());
        }

        [Fact(DisplayName = "Criar - Deve Retornar View")]
        [Trait("Controller Autor", " Criar")]
        public void Criar_DeveRetornarView()
        {
            // Act
            var result = _autorControllerTest.Criar();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact(DisplayName = "Editar - Deve Retornar View com Autor")]
        [Trait("Controller Autor", " Editar")]
        public async Task Editar_DeveRetornarViewComAutor()
        {
            // Arrange
            var autor = new AutorFaker().Generate();
            _autorServico.Setup(s => s.BuscarPorCodAsync(autor.CodAu)).ReturnsAsync(autor);

            // Act
            var result = await _autorControllerTest.Editar(autor.CodAu);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Autor>(viewResult.Model);
            Assert.Equal(autor.CodAu, model.CodAu);
        }

        [Fact(DisplayName = "Atualizar - Sucesso Deve Redirecionar Para Index")]
        [Trait("Controller Autor", " Atualizar")]
        public async Task Atualizar_Sucesso_DeveRedirecionarParaIndex()
        {
            // Arrange
            var autor = new AutorFaker().Generate();
            var resultado = new Resultado { Sucesso = true, Mensagem = "Autor atualizado com sucesso" };

            _autorServico
                .Setup(s => s.AtualizarAsync(autor))
                .ReturnsAsync(resultado);

            var tempData = new Mock<ITempDataDictionary>();
            _autorControllerTest.TempData = tempData.Object;

            // Act
            var result = await _autorControllerTest.Atualizar(autor);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            tempData.VerifySet(t => t["MensagemSucesso"] = "Autor atualizado com sucesso", Times.Once);
        }

        [Fact(DisplayName = "Atualizar - Falha Deve Retornar View Editar com Autor")]
        [Trait("Controller Livro", " Atualizar")]
        public async Task Atualizar_Falha_DeveRetornarViewEditarComAutor()
        {
            // Arrange
            var autor = new AutorFaker().Generate();
            _autorControllerTest.ModelState.AddModelError("Erro", "Erro de Validação");

            // Act
            var result = await _autorControllerTest.Atualizar(autor);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Autor>(viewResult.Model);
            Assert.Equal(autor, model);
        }

        [Fact(DisplayName = "ConfirmarDelecao - Sucesso Deve Redirecionar Para Index")]
        [Trait("Controller Autor", " Deletar")]
        public async Task ConfirmarDelecao_Sucesso_DeveRedirecionarParaIndex()
        {
            // Arrange
            var autorId = 1;
            _autorServico.Setup(s => s.DeletarAsync(autorId)).ReturnsAsync(true);

            var tempData = new Mock<ITempDataDictionary>();
            _autorControllerTest.TempData = tempData.Object;

            // Act
            var result = await _autorControllerTest.ConfirmarDelecao(autorId);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            tempData.VerifySet(t => t["MensagemSucesso"] = "Autor deletado com sucesso", Times.Once);
        }

        [Fact(DisplayName = "Deletar - Deve Retornar View com Autor")]
        [Trait("Controller Autor", "Deletar")]
        public async Task Deletar_DeveRetornarViewComAutor()
        {
            // Arrange
            var autor = new AutorFaker().Generate();
            _autorServico.Setup(s => s.BuscarPorCodAsync(autor.CodAu)).ReturnsAsync(autor);

            // Act
            var result = await _autorControllerTest.Deletar(autor.CodAu);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Autor>(viewResult.Model);
            Assert.Equal(autor.CodAu, model.CodAu);
        }

        [Fact(DisplayName = "Criar - Sucesso Deve Redirecionar Para Index")]
        [Trait("Controller Autor", " Criar")]
        public async Task Criar_Sucesso_DeveRedirecionarParaIndex()
        {
            // Arrange
            var autor = new AutorFaker().Generate();
            var resultado = new Resultado { Sucesso = true, Mensagem = "Autor cadastrado com sucesso" };

            _autorServico
                .Setup(s => s.AdicionarAsync(autor))
                .ReturnsAsync(resultado.Sucesso);

            // Configurando TempData
            var tempData = new Mock<ITempDataDictionary>();
            _autorControllerTest.TempData = tempData.Object;

            // Act
            var result = await _autorControllerTest.Criar(autor);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            tempData.VerifySet(t => t["MensagemSucesso"] = "Autor Cadastrado com sucesso", Times.Once);

        }

        [Fact(DisplayName = "Atualizar - Erro Deve Redirecionar Para Index Com Mensagem de Erro")]
        [Trait("Controller Autor", " Atualizar")]
        public async Task Atualizar_Erro_DeveRedirecionarParaIndex_ComMensagemErro()
        {
            // Arrange
            var autor = new AutorFaker().Generate();
            var exceptionMessage = "Erro ao atualizar autor";

            _autorServico
                .Setup(s => s.AtualizarAsync(autor))
                .ThrowsAsync(new Exception(exceptionMessage)); // Simula uma exceção

            // Configurando TempData
            var tempData = new Mock<ITempDataDictionary>();
            _autorControllerTest.TempData = tempData.Object;

            // Act
            var result = await _autorControllerTest.Atualizar(autor);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            tempData.VerifySet(t => t["MensagemErro"] = $"Erro ao atualizar autor, detalhe do erro: {exceptionMessage}", Times.Once);
        }

        [Fact(DisplayName = "ConfirmarDelecao - Erro Deve Redirecionar Para Index Com Mensagem de Erro")]
        [Trait("Controller Autor", " ConfirmarDeleçao")]
        public async Task ConfirmarDelecao_Erro_DeveRedirecionarParaIndex_ComMensagemErro()
        {
            // Arrange
            var autorId = 1;
            var exceptionMessage = "Erro ao deletar autor";

            _autorServico
                .Setup(s => s.DeletarAsync(autorId))
                .ThrowsAsync(new Exception(exceptionMessage)); // Simula uma exceção

            // Configurando TempData
            var tempData = new Mock<ITempDataDictionary>();
            _autorControllerTest.TempData = tempData.Object;

            // Act
            var result = await _autorControllerTest.ConfirmarDelecao(autorId);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            tempData.VerifySet(t => t["MensagemErro"] = $"Erro ao deletar autor, detalhe do erro: {exceptionMessage}", Times.Once);
        }

        [Fact(DisplayName = "Criar - Erro Deve Redirecionar Para Index Com Mensagem de Erro")]
        [Trait("Controller Autor", " Criar")]
        public async Task Criar_Erro_DeveRedirecionarParaIndex_ComMensagemErro()
        {
            // Arrange
            var autor = new AutorFaker().Generate();
            var exceptionMessage = "Erro ao cadastrar autor";

            _autorServico
                .Setup(s => s.AdicionarAsync(autor))
                .ThrowsAsync(new Exception(exceptionMessage)); // Simula uma exceção

            // Configurando TempData
            var tempData = new Mock<ITempDataDictionary>();
            _autorControllerTest.TempData = tempData.Object;

            // Act
            var result = await _autorControllerTest.Criar(autor);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            tempData.VerifySet(t => t["MensagemErro"] = $"Erro ao cadastrar autor, detalhe do erro: {exceptionMessage}", Times.Once);
        }


    }
}
