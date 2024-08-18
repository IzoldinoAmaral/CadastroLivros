using CadastroLivros.Data;
using CadastroLivros.Data.Repositorio;
using Moq;

namespace CadastroLivrosTeste.Unitario.Data.Repositorio
{
    public class AssuntoRepositorioTests
    {
        private readonly Mock<BancoContext> _bancoContext;
        private readonly AssuntoRepositorio _assuntoRepositorio;

        public AssuntoRepositorioTests()
        {
            _bancoContext = new Mock<BancoContext>();
            _assuntoRepositorio = new AssuntoRepositorio(_bancoContext.Object);
        }


    }
}
