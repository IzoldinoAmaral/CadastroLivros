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
    public class FormaCompraControllerTests
    {
        private FormaCompraController _formaCompraControllerTest { get; set; }
        private readonly Mock<IFormaCompraServico> _formaCompraServico = new();


        public FormaCompraControllerTests()
        {
            _formaCompraControllerTest = new FormaCompraController(_formaCompraServico.Object);

        }

        [Fact(DisplayName = "Deve Retornar View com Lista de Formas de Compra")]
        [Trait("Controller FormaCompra", "Listagem de Formas de Compra")]
        public async Task Index_DeveRetornarViewComListaDeFormasDeCompra()
        {
            // Arrange
            var fakerFormaCompra = new FormaCompraFaker();
            var dadosfakerFormaCompra = fakerFormaCompra.Generate(2);

            _formaCompraServico.Setup(s => s.BuscarTodosAsync()).ReturnsAsync(dadosfakerFormaCompra);

            // Act
            var result = await _formaCompraControllerTest.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<FormaCompra>>(viewResult.Model);
            Assert.Equal(2, ((List<FormaCompra>)model).Count);
            _formaCompraServico.Verify(s => s.BuscarTodosAsync(), Times.Once);
        }

        [Fact(DisplayName = "Deve Retornar View Vazia para Criação de Forma de Compra")]
        [Trait("Controller FormaCompra", "Criação de Forma de Compra")]
        public void Criar_DeveRetornarViewVazia()
        {
            // Arrange & Act
            var result = _formaCompraControllerTest.Criar();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.Model); 
        }


        [Fact(DisplayName = "Deve Retornar View com FormaCompra para Edição")]
        [Trait("Controller FormaCompra", "Edição de Forma de Compra")]
        public async Task Editar_IdValido_DeveRetornarViewComFormaCompra()
        {
            // Arrange
            int id = 1;
            var formaCompraEsperada = new FormaCompra { CodCom = id, Descricao = "Forma de Teste", Desconto = 10 };
            _formaCompraServico.Setup(s => s.BuscarPorCodAsync(id)).ReturnsAsync(formaCompraEsperada);

            // Act
            var result = await _formaCompraControllerTest.Editar(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<FormaCompra>(viewResult.Model);
            Assert.Equal(formaCompraEsperada, model);
            _formaCompraServico.Verify(s => s.BuscarPorCodAsync(id), Times.Once);
        }

        [Fact(DisplayName = "Deve Retornar View com Modelo Nulo quando Id Não Encontrado para Edição")]
        [Trait("Controller FormaCompra", "Edição de Forma de Compra")]
        public async Task Editar_IdNaoEncontrado_DeveRetornarViewComModeloNulo()
        {
            // Arrange
            int id = 1;
            _formaCompraServico.Setup(s => s.BuscarPorCodAsync(id)).ReturnsAsync((FormaCompra)null);

            // Act
            var result = await _formaCompraControllerTest.Editar(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.Model);
            _formaCompraServico.Verify(s => s.BuscarPorCodAsync(id), Times.Once);
        }

        [Fact(DisplayName = "Atualizar ModelState Valido")]
        [Trait("Controller FormaCompra", "Atualizar")]
        public async Task Atualizar_DeveRedirecionarParaIndex_QuandoModelStateEhValido()
        {
            // Arrange

            var fakerFormaCompra = new FormaCompraFaker();
            var dadosfakerFormaCompra = fakerFormaCompra.Generate();

            var resultado = new Resultado { Sucesso = true, Mensagem = "Forma de Compra atualizado com sucesso" };

            var tempData = new Mock<ITempDataDictionary>();
            _formaCompraControllerTest.TempData = tempData.Object;

            _formaCompraServico.Setup(s => s.AtualizarAsync(dadosfakerFormaCompra)).ReturnsAsync(resultado);

            // Act
            var result = await _formaCompraControllerTest.Atualizar(dadosfakerFormaCompra);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            tempData.VerifySet(t => t["MensagemSucesso"] = "Forma de Compra atualizado com sucesso", Times.Once);
        }

        [Fact(DisplayName = "Atualizar ModelState Invalido")]
        [Trait("Controller FormaCompra", "Atualizar")]
        public async Task Atualizar_ModelStateInvalido_DeveRetornarViewComModeloOriginal()
        {
            // Arrange
            var fakerFormaCompra = new FormaCompraFaker();
            var dadosfakerFormaCompra = fakerFormaCompra.Generate();

            _formaCompraControllerTest.ModelState.AddModelError("Nome", "Erro de validação");

            // Act
            var result = await _formaCompraControllerTest.Atualizar(dadosfakerFormaCompra);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Editar", viewResult.ViewName);
            Assert.Equal(dadosfakerFormaCompra, viewResult.Model);
            _formaCompraServico.Verify(s => s.AtualizarAsync(It.IsAny<FormaCompra>()), Times.Never);
        }

        [Fact(DisplayName = "Atualizar Com Exception")]
        [Trait("Controller FormaCompra", "Atualizar")]
        public async Task Atualizar_QuandoExcecaoOcorre_DeveRedirecionarParaIndexComMensagemErro()
        {

            // Arrange
            var fakerFormaCompra = new FormaCompraFaker();
            var dadosfakerFormaCompra = fakerFormaCompra.Generate();

            var exceptionMessage = "Erro genérico";

            _formaCompraServico.Setup(s => s.AtualizarAsync(dadosfakerFormaCompra))
                .ThrowsAsync(new Exception(exceptionMessage)); 

            var tempData = new Mock<ITempDataDictionary>();
            _formaCompraControllerTest.TempData = tempData.Object;

            // Act
            var result = await _formaCompraControllerTest.Atualizar(dadosfakerFormaCompra);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            tempData.VerifySet(t => t["MensagemErro"] = $"Erro ao Atualizar Forma Compra, detalhe do erro: {exceptionMessage}", Times.Once);

        }

        [Fact(DisplayName = "Deve Redirecionar para Index com Mensagem de Sucesso quando Deleção Bem-Sucedida")]
        [Trait("Controller FormaCompra", "Deleção de Forma de Compra")]
        public async Task ConfirmarDelecao_DelecaoBemSucedida_DeveRedirecionarParaIndexComMensagemSucesso()
        {
            // Arrange
            int id = 1;
            _formaCompraServico.Setup(s => s.DeletarAsync(id)).ReturnsAsync(true);

            var tempData = new Mock<ITempDataDictionary>();
            _formaCompraControllerTest.TempData = tempData.Object;

            // Act
            var result = await _formaCompraControllerTest.ConfirmarDelecao(id);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            tempData.VerifySet(tempData => tempData["MensagemSucesso"] = "Forma de Compra deletado com sucesso");
            _formaCompraServico.Verify(s => s.DeletarAsync(id), Times.Once);
        }

        [Fact(DisplayName = "Deve Redirecionar para Index sem Mensagem de Sucesso quando Deleção Não Bem-Sucedida")]
        [Trait("Controller FormaCompra", "Deleção de Forma de Compra")]
        public async Task ConfirmarDelecao_DelecaoNaoBemSucedida_DeveRedirecionarParaIndexSemMensagemSucesso()
        {
            // Arrange
            int id = 1;
            _formaCompraServico.Setup(s => s.DeletarAsync(id)).ReturnsAsync(false);

            var tempData = new Mock<ITempDataDictionary>();
            _formaCompraControllerTest.TempData = tempData.Object;

            // Act
            var result = await _formaCompraControllerTest.ConfirmarDelecao(id);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            tempData.VerifySet(tempData => tempData["MensagemSucesso"] = It.IsAny<string>(), Times.Never);
            _formaCompraServico.Verify(s => s.DeletarAsync(id), Times.Once);
        }

        [Fact(DisplayName = "Deve Redirecionar para Index com Mensagem de Erro quando Exceção Ocorre")]
        [Trait("Controller FormaCompra", "Deleção de Forma de Compra")]
        public async Task ConfirmarDelecao_QuandoExcecaoOcorre_DeveRedirecionarParaIndexComMensagemErro()
        {
            // Arrange
            var formaCompraId = 1;
            var exceptionMessage = "Erro genérico ao deletar Forma Compra";

            _formaCompraServico.Setup(s => s.DeletarAsync(formaCompraId))
                .ThrowsAsync(new Exception(exceptionMessage)); 

            // Configurando TempData
            var tempData = new Mock<ITempDataDictionary>();
            _formaCompraControllerTest.TempData = tempData.Object;

            // Act
            var result = await _formaCompraControllerTest.ConfirmarDelecao(formaCompraId);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            tempData.VerifySet(t => t["MensagemErro"] = $"Erro ao Deletar Forma Compra, detalhe do erro: {exceptionMessage}", Times.Once);
        }


        [Fact(DisplayName = "Deve Retornar View com FormaCompra quando Id Válido")]
        [Trait("Controller FormaCompra", "Busca de Forma de Compra")]
        public async Task Deletar_IdValido_DeveRetornarViewComFormaCompra()
        {
            // Arrange
            int id = 1;
            var formaCompraEsperada = new FormaCompra { CodCom = id, Descricao = "Teste", Desconto = 10 };
            _formaCompraServico.Setup(s => s.BuscarPorCodAsync(id)).ReturnsAsync(formaCompraEsperada);

            // Act
            var result = await _formaCompraControllerTest.Deletar(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<FormaCompra>(viewResult.Model);
            Assert.Equal(formaCompraEsperada, model);
            _formaCompraServico.Verify(s => s.BuscarPorCodAsync(id), Times.Once);
        }

        [Fact(DisplayName = "Deve Retornar View com Modelo Nulo quando Id Não Encontrado")]
        [Trait("Controller FormaCompra", "Busca de Forma de Compra")]
        public async Task Deletar_IdNaoEncontrado_DeveRetornarViewComModeloNulo()
        {
            // Arrange
            int id = 1;
            _formaCompraServico.Setup(s => s.BuscarPorCodAsync(id)).ReturnsAsync((FormaCompra)null);

            // Act
            var result = await _formaCompraControllerTest.Deletar(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.Model);
            _formaCompraServico.Verify(s => s.BuscarPorCodAsync(id), Times.Once);
        }

        [Fact(DisplayName = "Deve Redirecionar para Index com Mensagem de Sucesso quando ModelState é Válido")]
        [Trait("Controller FormaCompra", "Criação de Forma de Compra")]
        public async Task Criar_ModelStateValido_DeveRedirecionarParaIndexComMensagemSucesso()
        {
            // Arrange
            var fakerFormaCompra = new FormaCompraFaker();
            var dadosfakerFormaCompra = fakerFormaCompra.Generate();
            _formaCompraControllerTest.ModelState.Clear(); 

            var tempData = new Mock<ITempDataDictionary>();
            _formaCompraControllerTest.TempData = tempData.Object;

            // Act
            var result = await _formaCompraControllerTest.Criar(dadosfakerFormaCompra);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            tempData.VerifySet(tempData => tempData["MensagemSucesso"] = "Forma de Compra Cadastrado com sucesso");
            _formaCompraServico.Verify(s => s.AdicionarAsync(dadosfakerFormaCompra), Times.Once);
        }

        [Fact(DisplayName = "Deve Retornar View com Modelo quando ModelState é Inválido")]
        [Trait("Controller FormaCompra", "Criação de Forma de Compra")]
        public async Task Criar_ModelStateInvalido_DeveRetornarViewComModelo()
        {
            // Arrange
            var fakerFormaCompra = new FormaCompraFaker();
            var dadosfakerFormaCompra = fakerFormaCompra.Generate();
            _formaCompraControllerTest.ModelState.AddModelError("Erro", "ModelState é inválido");

            // Act
            var result = await _formaCompraControllerTest.Criar(dadosfakerFormaCompra);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(dadosfakerFormaCompra, viewResult.Model);
            _formaCompraServico.Verify(s => s.AdicionarAsync(It.IsAny<FormaCompra>()), Times.Never);
        }

        [Fact(DisplayName = "Deve Redirecionar para Index com Mensagem de Erro quando Exceção Ocorre")]
        [Trait("Controller FormaCompra", "Criação de Forma de Compra")]
        public async Task Criar_QuandoExcecaoOcorre_DeveRedirecionarParaIndexComMensagemErro()
        {
            // Arrange
            var fakerFormaCompra = new FormaCompraFaker();
            var dadosfakerFormaCompra = fakerFormaCompra.Generate();

            var exceptionMessage = "Erro ao cadastrar Forma Compra";

            _formaCompraServico.Setup(s => s.AdicionarAsync(dadosfakerFormaCompra))
                .ThrowsAsync(new Exception(exceptionMessage)); // Simula uma exceção genérica

            // Configurando TempData
            var tempData = new Mock<ITempDataDictionary>();
            _formaCompraControllerTest.TempData = tempData.Object;

            // Act
            var result = await _formaCompraControllerTest.Criar(dadosfakerFormaCompra);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            // Verificação precisa
            var expectedErrorMessage = $"Erro ao Cadastrar Forma Compra, detalhe do erro: {exceptionMessage}";
            tempData.VerifySet(t => t["MensagemErro"] = expectedErrorMessage, Times.Once);
        }
    }

 }


