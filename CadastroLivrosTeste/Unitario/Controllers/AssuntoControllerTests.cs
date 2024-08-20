using CadastroLivros.Controllers;
using CadastroLivros.Interfaces.Servicos;
using CadastroLivros.Models;
using CadastroLivros.Types;
using CadastroLivrosTeste.Unitario.Fixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;

namespace CadastroLivrosTeste.Unitario.Controllers
{


    public class AssuntoControllerTests
    {
        private AssuntoController _assuntoControllerTest { get; set; }
        private readonly Mock<IAssuntoServico> _assuntoServico = new();

        public AssuntoControllerTests()
        {
            _assuntoControllerTest = new AssuntoController(_assuntoServico.Object);

        }

        [Fact(DisplayName = "Index deve retornar a view com a lista de assuntos")]
        [Trait("Controller Assunto", "Index")]
        public async Task Index_DeveRetornarViewComListaDeAssuntos()
        {
            // Arrange
            var fakerAssunto = AssuntoFaker.GerarLista(3);
            _assuntoServico.Setup(s => s.BuscarTodosAsync()).ReturnsAsync(fakerAssunto);

            // Act
            var result = await _assuntoControllerTest.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Assunto>>(viewResult.ViewData.Model);
            Assert.Equal(3, model.Count());
        }

        [Fact(DisplayName = "Criar deve retornar a view de criação")]
        [Trait("Controller Assunto", "Criar")]
        public void Criar_DeveRetornarViewDeCriacao()
        {
            // Act
            var result = _assuntoControllerTest.Criar();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewData.Model); 
        }

        [Fact(DisplayName = "Editar deve retornar a view com o assunto correto")]
        [Trait("Controller Assunto", "Editar")]
        public async Task Editar_DeveRetornarViewComAssuntoCorreto()
        {
            // Arrange
            var fakerAssunto = AssuntoFaker.GerarAssunto();

            _assuntoServico.Setup(s => s.BuscarPorCodAsync(fakerAssunto.CodAs)).ReturnsAsync(fakerAssunto);

            // Act
            var result = await _assuntoControllerTest.Editar(fakerAssunto.CodAs);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Assunto>(viewResult.ViewData.Model);
            Assert.Equal(fakerAssunto.CodAs, model.CodAs);
        }

        [Fact(DisplayName = "Atualizar deve redirecionar para Index com mensagem de sucesso quando o modelo é válido")]
        [Trait("Controller Assunto", "Atualizar")]
        public async Task Atualizar_DeveRedirecionarParaIndexComMensagemDeSucesso_QuandoModeloValido()
        {
            // Arrange
            var assunto = AssuntoFaker.GerarAssunto();

            var resultado = new Resultado { Sucesso = true, Mensagem = "Assunto atualizado com sucesso" };

            _assuntoServico.Setup(s => s.AtualizarAsync(assunto)).ReturnsAsync(resultado);

            // Configuração do TempData como um dicionário simples
            _assuntoControllerTest.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            // Act
            var result = await _assuntoControllerTest.Atualizar(assunto);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Assunto atualizado com sucesso", _assuntoControllerTest.TempData["MensagemSucesso"]);
        }

        [Fact(DisplayName = "Atualizar deve retornar a view Editar com o modelo quando o modelo é inválido")]
        [Trait("Controller Assunto", "Atualizar")]
        public async Task Atualizar_DeveRetornarViewEditarComModelo_QuandoModeloInvalido()
        {
            // Arrange
            var assunto = AssuntoFaker.GerarAssunto();
            _assuntoControllerTest.ModelState.AddModelError("Descricao", "O campo Descrição é obrigatório.");

            // Act
            var result = await _assuntoControllerTest.Atualizar(assunto);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Editar", viewResult.ViewName);
            Assert.Equal(assunto, viewResult.Model);
        }

        [Fact(DisplayName = "Atualizar deve redirecionar para Index com mensagem de erro quando ocorre uma exceção")]
        [Trait("Controller Assunto", "Atualizar")]
        public async Task Atualizar_DeveRedirecionarParaIndexComMensagemDeErro_QuandoOcorrerExcecao()
        {
            // Arrange
            var assunto = AssuntoFaker.GerarAssunto();
            var exceptionMessage = "Erro ao atualizar o banco de dados";

            var tempData = new Mock<ITempDataDictionary>();
            _assuntoControllerTest.TempData = tempData.Object;

            _assuntoServico.Setup(s => s.AtualizarAsync(assunto)).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _assuntoControllerTest.Atualizar(assunto);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            tempData.VerifySet(tempData => tempData["MensagemErro"] = It.Is<string>(msg => msg.StartsWith("Erro ao atualizar assunto, detalhe do erro:")));            
        }

        [Fact(DisplayName = "ConfirmarDelecao deve redirecionar para Index com mensagem de sucesso quando a exclusão é bem-sucedida")]
        [Trait("Controller Assunto", "ConfirmarDelecao")]
        public async Task ConfirmarDelecao_DeveRedirecionarParaIndexComMensagemDeSucesso_QuandoExclusaoBemSucedida()
        {
            // Arrange
            int id = 1;

            var tempData = new Mock<ITempDataDictionary>();
            _assuntoControllerTest.TempData = tempData.Object;

            _assuntoServico.Setup(s => s.DeletarAsync(id)).ReturnsAsync(true);

            // Act
            var result = await _assuntoControllerTest.ConfirmarDelecao(id);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            tempData.VerifySet(tempData => tempData["MensagemSucesso"] = "Assunto deletado com sucesso", Times.Once);
        }

        [Fact(DisplayName = "ConfirmarDelecao deve redirecionar para Index com mensagem de erro quando ocorre uma exceção")]
        [Trait("Controller Assunto", "ConfirmarDelecao")]
        public async Task ConfirmarDelecao_DeveRedirecionarParaIndexComMensagemDeErro_QuandoOcorrerExcecao()
        {
            // Arrange
            int id = 1;
            var exceptionMessage = "Erro ao deletar o banco de dados";

            var tempData = new Mock<ITempDataDictionary>();
            _assuntoControllerTest.TempData = tempData.Object;

            _assuntoServico.Setup(s => s.DeletarAsync(id)).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _assuntoControllerTest.ConfirmarDelecao(id);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            tempData.VerifySet(tempData => tempData["MensagemErro"] = It.Is<string>(msg => msg.StartsWith("Erro ao deletar assunto, detalhe do erro:")), Times.Once);
        }

        [Fact(DisplayName = "Deletar deve retornar a view com o assunto correto")]
        [Trait("Controller Assunto", "Deletar")]
        public async Task Deletar_DeveRetornarViewComAssuntoCorreto()
        {
            // Arrange
            int id = 1;
            var assuntoEsperado = AssuntoFaker.GerarAssunto();
            assuntoEsperado.CodAs = id;

            _assuntoServico.Setup(s => s.BuscarPorCodAsync(id)).ReturnsAsync(assuntoEsperado);

            // Act
            var result = await _assuntoControllerTest.Deletar(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Assunto>(viewResult.Model);
            Assert.Equal(assuntoEsperado.CodAs, model.CodAs);
            Assert.Equal(assuntoEsperado.Descricao, model.Descricao);
        }

        [Fact(DisplayName = "Criar deve redirecionar para Index com mensagem de sucesso quando o modelo é válido")]
        [Trait("Controller Assunto", "Criar")]
        public async Task Criar_DeveRedirecionarParaIndexComMensagemDeSucesso_QuandoModeloValido()
        {
            // Arrange
            var assunto = AssuntoFaker.GerarAssunto();            
            _assuntoServico.Setup(s => s.AdicionarAsync(assunto)).ReturnsAsync(true);

            // Configuração do TempData como mock
            var tempData = new Mock<ITempDataDictionary>();
            _assuntoControllerTest.TempData = tempData.Object;

            // Act
            var result = await _assuntoControllerTest.Criar(assunto);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            tempData.VerifySet(tempData => tempData["MensagemSucesso"] = "Assunto Cadastrado com sucesso", Times.Once);
            _assuntoServico.Verify(s => s.AdicionarAsync(assunto), Times.Once);
        }

        [Fact(DisplayName = "Criar deve retornar a view com o modelo quando o modelo é inválido")]
        [Trait("Controller Assunto", "Criar")]
        public async Task Criar_DeveRetornarViewComModelo_QuandoModeloInvalido()
        {
            // Arrange
            var assunto = AssuntoFaker.GerarAssunto();
            _assuntoControllerTest.ModelState.AddModelError("Descricao", "Campo obrigatório");

            // Act
            var result = await _assuntoControllerTest.Criar(assunto);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Assunto>(viewResult.Model);
            Assert.Equal(assunto.CodAs, model.CodAs);
            Assert.Equal(assunto.Descricao, model.Descricao);
            _assuntoServico.Verify(s => s.AdicionarAsync(It.IsAny<Assunto>()), Times.Never);
        }

        [Fact(DisplayName = "Criar deve redirecionar para Index com mensagem de erro quando ocorre uma exceção")]
        [Trait("Controller Assunto", "Criar")]
        public async Task Criar_DeveRedirecionarParaIndexComMensagemDeErro_QuandoOcorrerExcecao()
        {
            // Arrange
            var assunto = AssuntoFaker.GerarAssunto();
            var exceptionMessage = "Erro ao cadastrar o assunto";

            var tempData = new Mock<ITempDataDictionary>();
            _assuntoControllerTest.TempData = tempData.Object;

            _assuntoServico.Setup(s => s.AdicionarAsync(assunto)).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _assuntoControllerTest.Criar(assunto);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            tempData.VerifySet(tempData => tempData["MensagemErro"] = It.Is<string>(msg => msg.StartsWith("Erro ao cadastrar assunto, detalhe do erro:")), Times.Once);
            _assuntoServico.Verify(s => s.AdicionarAsync(assunto), Times.Once);
        }




    }
}
