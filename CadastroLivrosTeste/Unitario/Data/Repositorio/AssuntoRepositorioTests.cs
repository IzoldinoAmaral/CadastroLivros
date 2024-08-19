using CadastroLivros.Data;
using CadastroLivros.Data.Repositorio;
using CadastroLivros.Models;
using CadastroLivrosTeste.Unitario.Fixture;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CadastroLivrosTeste.Unitario.Data.Repositorio
{
    public class AssuntoRepositorioTests 
    {
        private readonly AssuntoRepositorio _assuntoRepositorio;
        private readonly BancoContext _bancoContext;

        public AssuntoRepositorioTests()
        {
            var options = new DbContextOptionsBuilder<BancoContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _bancoContext = new BancoContext(options);
            _bancoContext.Database.EnsureCreated();
            _assuntoRepositorio = new AssuntoRepositorio(_bancoContext);
        }


        [Fact(DisplayName = "AdicionarAsync_DeveRetornarVerdadeiroQuandoAdicionadoComSucesso")]
        [Trait("Repositorio", "AssuntoRepositorio")]
        public async Task AdicionarAsync_DeveRetornarVerdadeiroQuandoAdicionadoComSucesso()
        {
            // Arrange
            var assunto = AssuntoFaker.GerarAssunto();

            // Act
            var resultado = await _assuntoRepositorio.AdicionarAsync(assunto);

            // Assert
            Assert.True(resultado);
            Assert.Contains(assunto, _bancoContext.Assuntos);


        }

        [Fact(DisplayName = "Atualizar_DeveRetornarAssuntoAtualizado")]
        [Trait("Repositorio", "AssuntoRepositorio")]
        public async Task Atualizar_DeveRetornarAssuntoAtualizado()
        {
            // Arrange
            var assunto = AssuntoFaker.GerarAssunto();
            _bancoContext.Assuntos.Add(assunto);
            await _bancoContext.SaveChangesAsync();

            var novoDescricao = "Nova Descrição";
            assunto.Descricao = novoDescricao;

            // Act
            var resultado = await _assuntoRepositorio.Atualizar(assunto);

            // Assert
            Assert.Equal(novoDescricao, resultado.Descricao);
            Assert.Contains(assunto, _bancoContext.Assuntos);
            

        }

        [Fact(DisplayName = "BuscarPorNomeAsync_DeveRetornarVerdadeiroSeNomeExiste")]
        [Trait("Repositorio", "AssuntoRepositorio")]
        public async Task BuscarPorNomeAsync_DeveRetornarVerdadeiroSeNomeExiste()
        {
            // Arrange
            var assuntoTeste = new Assunto
            {
                CodAs = 1,
                Descricao = "Teste",
                Ativo = true
            };


            _bancoContext.Assuntos.Add(assuntoTeste);
            await _bancoContext.SaveChangesAsync();

            // Act
            var resultado = await _assuntoRepositorio.BuscarPorNomeAsync("Teste");

            // Assert
            Assert.True(resultado);

        }

        [Fact(DisplayName = "BuscarPorCodAsync_DeveRetornarAssuntoSeEncontrado")]
        [Trait("Repositorio", "AssuntoRepositorio")]
        public async Task BuscarPorCodAsync_DeveRetornarAssuntoSeEncontrado()
        {
            // Arrange
            var assunto = AssuntoFaker.GerarAssunto();
            assunto.Ativo = true;
            _bancoContext.Assuntos.Add(assunto);
            await _bancoContext.SaveChangesAsync();

            // Act
            var resultado = await _assuntoRepositorio.BuscarPorCodAsync(assunto.CodAs);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(assunto.CodAs, resultado.CodAs);

        }

        [Fact(DisplayName = "BuscarTodosAsync_DeveRetornarTodosAssuntosAtivos")]
        [Trait("Repositorio", "AssuntoRepositorio")]
        public async Task BuscarTodosAsync_DeveRetornarTodosAssuntosAtivos()
        {
            // Arrange
            var assuntos = AssuntoFaker.GerarLista(2);
            
            foreach (var assunto in assuntos)
            {
                assunto.Ativo = true;
                _bancoContext.Assuntos.Add(assunto);
                
            }

            await _bancoContext.SaveChangesAsync();

            // Act
            var resultado = await _assuntoRepositorio.BuscarTodosAsync();

            // Assert
            Assert.Equal(2, resultado.Count());

        }

        [Fact(DisplayName = "DeletarAsync_DeveDesativarAssunto")]
        [Trait("Repositório", "AssuntoRepositorio")]
        public async Task DeletarAsync_DeveDesativarAssunto()
        {
            // Arrange
            var assunto = AssuntoFaker.GerarAssunto();
            _bancoContext.Assuntos.Add(assunto);
            await _bancoContext.SaveChangesAsync();

            assunto.Ativo = false;

            // Act
            var resultado = await _assuntoRepositorio.DeletarAsync(assunto);

            // Assert
            Assert.False(assunto.Ativo);
            Assert.True(resultado);


        }



    }

}
