using CadastroLivros.Controllers;
using CadastroLivros.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Diagnostics;

namespace CadastroLivrosTeste.Unitario.Controllers
{
    public class HomeControllerTests
    {
        private HomeController _homeControllerTest { get; set; }

        public HomeControllerTests()
        {
            _homeControllerTest = new HomeController();
        }

        [Fact(DisplayName = "Chamar Index da pagina Home")]
        [Trait("Controller Home", " Index")]
        public void Index_DeveRetornarViewResult()
        {
            // Act
            var resultado = _homeControllerTest.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(resultado);
            Assert.Null(viewResult.ViewName); 
        }

        [Fact(DisplayName = "Chamar Privacy da pagina Home")]
        [Trait("Controller Home", " Privacy")]
        public void Privacy_DeveRetornarViewResult()
        {
            // Act
            var resultado = _homeControllerTest.Privacy();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(resultado);
            Assert.Null(viewResult.ViewName); 
        }

        [Fact(DisplayName = "Erro na View")]
        [Trait("Controller Home", " ErroViewModel")]
        public void Error_DeveRetornarViewResult_ComErrorViewModel()
        {
            // Arrange
            Activity.Current = null;

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(c => c.TraceIdentifier).Returns("123456");

            var controller = new HomeController
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            // Act
            var resultado = controller.Error();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(resultado);
            var modelo = Assert.IsType<ErrorViewModel>(viewResult.Model);
            Assert.NotNull(modelo.RequestId);
            Assert.Equal("123456", modelo.RequestId);
        }
    }
}
