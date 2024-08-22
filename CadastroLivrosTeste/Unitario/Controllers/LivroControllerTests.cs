using CadastroLivros.Controllers;
using CadastroLivros.DTOs;
using CadastroLivros.Interfaces.Servicos;
using CadastroLivros.Models;
using CadastroLivrosTeste.Unitario.DTOs;
using CadastroLivrosTeste.Unitario.Fixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System.Text;

namespace CadastroLivrosTeste.Unitario.Controllers
{
    public class LivroControllerTests
    {
        private LivroController _livroControllerTest {  get; set; }
        private readonly Mock<ILivroServico> _livroServico = new();
        private readonly Mock<IAutorServico> _autorServico = new();
        private readonly Mock<IAssuntoServico> _assuntoServico = new();
        private readonly Mock<ILivroRelatorioServico> _livroRelatorioServico = new();

        public LivroControllerTests()
        {
            _livroControllerTest = new LivroController(_livroServico.Object, _autorServico.Object, _assuntoServico.Object, _livroRelatorioServico.Object);
        }

        #region Teste: Pagina Inicial Livro
        [Fact(DisplayName = "Chamar Index da pagina de Livro")]
        [Trait("Controller Livro"," Index")]
        public async Task Criar_DeveChamarIndex()
        {
            //Arrange
            var fakerLivros = LivroFaker.Gerar(3);

            _livroServico.Setup(lf => lf.BuscarTodosAsync()).ReturnsAsync(fakerLivros);

            //Act
            var result = await _livroControllerTest.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Livro>>(viewResult.ViewData.Model);
            Assert.Equal(3, ((List<Livro>)model).Count);


        }

        #endregion Teste: Pagina Inicial Livro

        #region Teste: Listar Detalhes
        [Fact(DisplayName = "Listar Detalhes do livro")]
        [Trait("Controller Livro", " ListarDetalhes")]
        public async Task ListarDetalhes_FormaCompra()
        {
            //Arrange
            var fakerDetalhes = new DetalhesLivroViewModelFaker();
            var dadosfakerDetalhes = fakerDetalhes.Generate();

            
            _livroServico.Setup(lf => lf.ListarDetalhesAsync(It.IsAny<int>())).ReturnsAsync(dadosfakerDetalhes);
            
            //Act
            var result = await _livroControllerTest.ListarDetalhes(1);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<DetalhesLivroViewModel>(viewResult.ViewData.Model);
            Assert.Equal(dadosfakerDetalhes.NomeLivro, model.NomeLivro);
            Assert.Equal(dadosfakerDetalhes.Precos, model.Precos);            

        }

        #endregion Teste: Listar Detalhes

        #region Teste: Criar Livro
        [Fact(DisplayName = "Criar livro")]
        [Trait("Controller Livro", "Criar")]
        public async Task Criar_DeveCriarUmNovoLivro()
        {
            //Arrange
            var fakerAutor = new AutorFaker();
            var dadosFakerAutor = fakerAutor.Generate(2);

            var fakerAssunto = new AssuntoFaker();
            var dadosFakerAssunto = fakerAssunto.Generate(2);

            _livroServico.Setup(s => s.BuscarTodosAutoresAsync()).ReturnsAsync(dadosFakerAutor);

            _livroServico.Setup(s => s.BuscarTodosAssuntosAsync()).ReturnsAsync(dadosFakerAssunto);            

            //Act            
            var result = await _livroControllerTest.Criar();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var viewData = viewResult.ViewData;

            Assert.NotNull(viewData["Autores"]);
            Assert.NotNull(viewData["Assuntos"]);

            var autoresSelectList = Assert.IsAssignableFrom<SelectList>(viewData["Autores"]);
            var assuntosSelectList = Assert.IsAssignableFrom<SelectList>(viewData["Assuntos"]);

            Assert.Equal(dadosFakerAutor.Select(a => a.Nome), autoresSelectList.Items.Cast<Autor>().Select(a => a.Nome));
            Assert.Equal(dadosFakerAssunto.Select(a => a.Descricao), assuntosSelectList.Items.Cast<Assunto>().Select(a => a.Descricao));

        }

        #endregion Teste: Criar Livro

        #region Teste: Editar Livro
        [Fact(DisplayName = "Editar livro")]
        [Trait("Controller Livro", "Editar")]
        public async Task Editar_DeveChamarViewComDadosParaEditar()
        {
            //Arrange
            var livroId = 1;

            var fakerAutor = new AutorFaker();
            var dadosFakerAutor = fakerAutor.Generate(2);

            var fakerAssunto = new AssuntoFaker();
            var dadosFakerAssunto = fakerAssunto.Generate(2);

            var fakerLivros = await LivroFaker.GerarComTodosCamposPreenchidosAsync();

            _livroServico.Setup(s => s.BuscarPorCodAsync(livroId)).ReturnsAsync(fakerLivros);
            _autorServico.Setup(s => s.BuscarTodosAsync()).ReturnsAsync(dadosFakerAutor);
            _assuntoServico.Setup(s => s.BuscarTodosAsync()).ReturnsAsync(dadosFakerAssunto);

            // Act
            var result = await _livroControllerTest.Editar(livroId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Livro>(viewResult.Model);
            var viewData = viewResult.ViewData;

            Assert.Equal(fakerLivros, model);
            Assert.NotNull(viewData["Autores"]);
            Assert.NotNull(viewData["Assuntos"]);

            var autoresSelectList = Assert.IsAssignableFrom<SelectList>(viewData["Autores"]);
            var assuntosSelectList = Assert.IsAssignableFrom<SelectList>(viewData["Assuntos"]);

            Assert.Equal(dadosFakerAutor.Select(a => a.Nome), autoresSelectList.Items.Cast<Autor>().Select(a => a.Nome));
            Assert.Equal(dadosFakerAssunto.Select(a => a.Descricao), assuntosSelectList.Items.Cast<Assunto>().Select(a => a.Descricao));
        }
        #endregion Teste: Editar Livro

        #region Teste: Atualizar Livro
        [Fact(DisplayName = "Atualizar ModelState Valido")]
        [Trait("Controller Livro", "Atualizar")]
        public async Task Atualizar_DeveRedirecionarParaIndex_QuandoModelStateEhValido()
        {
            // Arrange
            var fakeLivro = LivroFaker.GerarComTodosCamposPreenchidos();
            var fakeResultado = await ResultadoFaker.GerarComLivroAsync(true);

            var tempData = new Mock<ITempDataDictionary>();
            _livroControllerTest.TempData = tempData.Object;

            _livroServico.Setup(s => s.BuscarPorCodAsync(fakeLivro.Codl)).ReturnsAsync(fakeLivro);

            _livroServico.Setup(s => s.AtualizarAsync(fakeLivro)).ReturnsAsync(fakeResultado);

            // Act
            var result = await _livroControllerTest.Atualizar(fakeLivro);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            tempData.VerifySet(t => t["MensagemSucesso"] = "Livro atualizado com sucesso", Times.Once);
        }

        [Fact(DisplayName = "Atualizar ModelState Invalido")]
        [Trait("Controller Livro", "Atualizar")]
        public async Task Atualizar_DeveRetornarViewEditarComLivro_QuandoModelStateEhInvalido()
        {
            // Arrange
            var fakeLivro = LivroFaker.GerarComTodosCamposPreenchidos();
            _livroControllerTest.ModelState.AddModelError("Erro", "Erro de validação");

            // Act
            var result = await _livroControllerTest.Atualizar(fakeLivro);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Editar", viewResult.ViewName);
            Assert.Equal(fakeLivro, viewResult.Model);
            _livroServico.Verify(s => s.AtualizarAsync(It.IsAny<Livro>()), Times.Never);
        }

        [Fact(DisplayName = "Atualizar com Erro")]
        [Trait("Controller Livro", "Atualizar")]
        public async Task Atualizar_DeveRedirecionarParaIndexComMensagemErro_QuandoExcecaoEhLancada()
        {
            // Arrange
            var fakeLivro = LivroFaker.GerarComTodosCamposPreenchidos();
            var mensagemErro = "Erro ao atualizar";

            var tempData = new Mock<ITempDataDictionary>();
            _livroControllerTest.TempData = tempData.Object;

            _livroServico.Setup(s => s.AtualizarAsync(fakeLivro)).ThrowsAsync(new Exception(mensagemErro));

            // Act
            var result = await _livroControllerTest.Atualizar(fakeLivro);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            tempData.VerifySet(t => t["MensagemErro"] = It.Is<string>(msg => msg.Contains(mensagemErro)), Times.Once);
            _livroServico.Verify(s => s.AtualizarAsync(fakeLivro), Times.Once);
        }

        #endregion Teste: Atualizar Livro

        #region Teste: Confirmar exclusão Livro
        [Fact(DisplayName = "Confimar exclusão Com Sucesso")]
        [Trait("Controller Livro", "Deletar")]
        public async Task ConfirmarDelecao_DeveRedirecionarParaIndexComMensagemSucesso_QuandoLivroDeletadoComSucesso()
        {
            // Arrange
            int livroId = 1; 
            _livroServico.Setup(s => s.DeletarAsync(livroId)).ReturnsAsync(true);

            var tempData = new Mock<ITempDataDictionary>();
            _livroControllerTest.TempData = tempData.Object;

            // Act
            var result = await _livroControllerTest.ConfirmarDelecao(livroId);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);

            tempData.VerifySet(t => t["MensagemSucesso"] = "Livro deletado com sucesso", Times.Once);
            _livroServico.Verify(s => s.DeletarAsync(livroId), Times.Once);
        }

        [Fact(DisplayName = "Confimar exclusão com Erro")]
        [Trait("Controller Livro", "Deletar")]
        public async Task ConfirmarDelecao_DeveRedirecionarParaIndexComMensagemErro_QuandoExcecaoForLancada()
        {
            // Arrange
            int livroId = 1; 
            var mensagemErro = "Erro ao deletar livro";

            _livroServico.Setup(s => s.DeletarAsync(livroId)).ThrowsAsync(new Exception(mensagemErro));

            var tempData = new Mock<ITempDataDictionary>();
            _livroControllerTest.TempData = tempData.Object;

            // Act
            var result = await _livroControllerTest.ConfirmarDelecao(livroId);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);

            tempData.VerifySet(t => t["MensagemErro"] = It.Is<string>(msg => msg.Contains(mensagemErro)), Times.Once);
            _livroServico.Verify(s => s.DeletarAsync(livroId), Times.Once);
        }

        #endregion Teste: Confirmar exclusão Livro

        #region Teste: Deletar Livro
        [Fact(DisplayName = "Deletar quando livro existe")]
        [Trait("Controller Livro", "Deletar")]
        public async Task Deletar_DeveRetornarViewComLivro_QuandoLivroExiste()
        {
            // Arrange
            int livroId = 1; // ID do livro a ser deletado
            var fakeLivro = LivroFaker.GerarComTodosCamposPreenchidos();

            _livroServico.Setup(s => s.BuscarPorCodAsync(livroId)).ReturnsAsync(fakeLivro);

            // Act
            var result = await _livroControllerTest.Deletar(livroId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(fakeLivro, viewResult.Model);
            _livroServico.Verify(s => s.BuscarPorCodAsync(livroId), Times.Once);
        }

        [Fact(DisplayName = "Deletar quando livro não existe")]
        [Trait("Controller Livro", "Deletar")]
        public async Task Deletar_DeveRedirecionarParaIndex_QuandoLivroNaoExiste()
        {
            // Arrange
            int livroId = 1; // ID do livro a ser deletado
            _livroServico.Setup(s => s.BuscarPorCodAsync(livroId)).ReturnsAsync((Livro)null);

            var mockTempData = new Mock<ITempDataDictionary>();
            _livroControllerTest.TempData = mockTempData.Object;

            // Act
            var result = await _livroControllerTest.Deletar(livroId);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mockTempData.VerifySet(t => t["MensagemErro"] = "Livro não encontrado.", Times.Once);
            _livroServico.Verify(s => s.BuscarPorCodAsync(livroId), Times.Once);
        }

        #endregion Teste: Deletar Livro

        #region Teste: Criar Http Post Livro
        [Fact(DisplayName = "Criar htttpPost livro ModelState Valido")]
        [Trait("Controller Livro", "Criar(POST)")]
        public async Task Criar_DeveRedirecionarParaIndex_QuandoModelStateEhValido()
        {
            // Arrange
            var fakeLivro = LivroFaker.GerarComTodosCamposPreenchidos();

            var mockTempData = new Mock<ITempDataDictionary>();
            _livroControllerTest.TempData = mockTempData.Object;

            _livroServico.Setup(s => s.AdicionarAsync(fakeLivro)).ReturnsAsync(true);

            // Act
            var result = await _livroControllerTest.Criar(fakeLivro);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);

            mockTempData.VerifySet(t => t["MensagemSucesso"] = "Livro Cadastrado com sucesso", Times.Once);
            
            _livroServico.Verify(s => s.AdicionarAsync(fakeLivro), Times.Once);
        }
        [Fact(DisplayName = "Criar htttpPost livro ModelState InValido")]
        [Trait("Controller Livro", "Criar(POST)")]
        public async Task Criar_DeveRetornarViewComErros_QuandoModelStateEhInvalido()
        {
            // Arrange
            var fakeLivro = LivroFaker.GerarComTodosCamposPreenchidos();

            var mockTempData = new Mock<ITempDataDictionary>();
            _livroControllerTest.TempData = mockTempData.Object;

            _livroControllerTest.ModelState.AddModelError("Titulo", "Titulo é obrigatório");

            var fakeAutores = AutorFaker.GerarLista(5);
            var fakeAssuntos = AssuntoFaker.GerarLista(5);

            _livroServico.Setup(s => s.BuscarTodosAutoresAsync()).ReturnsAsync(fakeAutores);
            _livroServico.Setup(s => s.BuscarTodosAssuntosAsync()).ReturnsAsync(fakeAssuntos);

            // Act
            var result = await _livroControllerTest.Criar(fakeLivro);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(fakeLivro, viewResult.Model);
            
            Assert.IsType<SelectList>(_livroControllerTest.ViewBag.Autores);
            Assert.IsType<SelectList>(_livroControllerTest.ViewBag.Assuntos);

             Assert.Equal(fakeAutores.Count(), ((SelectList)_livroControllerTest.ViewBag.Autores).Count());
            Assert.Equal(fakeAssuntos.Count(), ((SelectList)_livroControllerTest.ViewBag.Assuntos).Count());
        }

        [Fact(DisplayName = "Criar httpPost livro Exception")]
        [Trait("Controller Livro", "Criar(POST)")]
        public async Task Criar_DeveRedirecionarParaIndexComMensagemErro_QuandoExcecaoEhLancada()
        {
            // Arrange
            var fakeLivro = LivroFaker.GerarComTodosCamposPreenchidos();
            var mensagemErro = "Erro ao cadastrar livro";

            var mockTempData = new Mock<ITempDataDictionary>();
            _livroControllerTest.TempData = mockTempData.Object;

            _livroServico.Setup(s => s.AdicionarAsync(fakeLivro)).ThrowsAsync(new Exception(mensagemErro));

            // Act
            var result = await _livroControllerTest.Criar(fakeLivro);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
         
            mockTempData.VerifySet(t => t["MensagemErro"] = It.Is<string>(msg => msg.Contains(mensagemErro)), Times.Once);

            _livroServico.Verify(s => s.AdicionarAsync(fakeLivro), Times.Once);
        }

        #endregion Teste: Criar Http Post Livro

        #region Teste: Gerar relatorio
        [Fact(DisplayName = "Gerar Relatorio com dados")]
        [Trait("Controller Livro", "Gerar Relatorio com pdf")]
        public async Task GerarRelatorio_DeveRetornarArquivoPdf()
        {
            // Arrange

            var fakerRelatorio = new LivroRelatorioDtoFaker();
            var dadosRelatorio = fakerRelatorio.Generate(1);
            //var dadosRelatorio = new List<LivroRelatorioDto>
            //{
            //    new LivroRelatorioDto { NomeAutor = "Autor Teste", TituloLivro = "Livro Teste" }
            //};

            _livroRelatorioServico.Setup(s => s.ObterDadosRelatorioAsync())
                                      .ReturnsAsync(dadosRelatorio);

            _livroRelatorioServico.Setup(s => s.GerarRelatorioAsync(dadosRelatorio, It.IsAny<Stream>()))
                                      .Callback((List<LivroRelatorioDto> relatorio, Stream stream) =>
                                      {
                                          // Simula a escrita de dados no stream
                                          byte[] fakeData = Encoding.UTF8.GetBytes("Fake PDF Data");
                                          stream.Write(fakeData, 0, fakeData.Length);
                                      })
                                      .Returns(Task.CompletedTask);

            // Act
            var result = await _livroControllerTest.GerarRelatorio();

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("application/pdf", fileResult.ContentType);
            Assert.Equal("RelatorioLivros.pdf", fileResult.FileDownloadName);
            Assert.NotEmpty(fileResult.FileContents);

            _livroRelatorioServico.Verify(s => s.ObterDadosRelatorioAsync(), Times.Once);
            _livroRelatorioServico.Verify(s => s.GerarRelatorioAsync(dadosRelatorio, It.IsAny<Stream>()), Times.Once);
        }

        [Fact(DisplayName = "Gerar Relatorio sem dados")]
        [Trait("Controller Livro", "Gerar Relatorio sem dados")]
        public async Task GerarRelatorio_DeveRedirecionarParaIndexComMensagem_QuandoDadosRelatorioEstiverVazio()
        {
            // Arrange
            _livroRelatorioServico.Setup(s => s.ObterDadosRelatorioAsync())
                                  .ReturnsAsync(new List<LivroRelatorioDto>());

            _livroControllerTest.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            // Act
            var result = await _livroControllerTest.GerarRelatorio();

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Livro", redirectResult.ControllerName);
            Assert.Equal("Não existem livros com autores cadastrados para o relatório.", _livroControllerTest.TempData["Mensagem"]);
        }

        #endregion Teste: Gerar relatorio
    }

}

