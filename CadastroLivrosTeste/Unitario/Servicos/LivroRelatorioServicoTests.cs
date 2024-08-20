using CadastroLivros.Interfaces.Repositorios;
using CadastroLivros.Servicos;
using CadastroLivrosTeste.Unitario.DTOs;
using iTextSharp.text.pdf;
using Moq;

namespace CadastroLivrosTeste.Unitario.Servicos
{
    public class LivroRelatorioServicoTests
    {
        private readonly Mock<ILivroRelatorioRepositorio> _livroRelatorioRepositorio;
        private readonly LivroRelatorioServico _livroRelatorioServico;
        private readonly LivroRelatorioDtoFaker _livroRelatorioDtoFaker;

        public LivroRelatorioServicoTests()
        {
            _livroRelatorioRepositorio = new Mock<ILivroRelatorioRepositorio>();
            _livroRelatorioServico = new LivroRelatorioServico(_livroRelatorioRepositorio.Object);
            _livroRelatorioDtoFaker = new LivroRelatorioDtoFaker();
        }

        [Fact(DisplayName = "Deve Gerar Relatório PDF com Dados Válidos")]
        [Trait("Servico", "Livro Relatorio")]
        public async Task GerarRelatorioAsync_DeveGerarRelatorioComDadosValidos()
        {
            // Arrange
            var dadosRelatorio = _livroRelatorioDtoFaker.Generate(5);
            using var memoryStream = new MemoryStream();

            // Act
            await _livroRelatorioServico.GerarRelatorioAsync(dadosRelatorio, memoryStream);

            // Assert
            var pdfReader = new PdfReader(memoryStream.ToArray());
            Assert.True(pdfReader.NumberOfPages > 0);
            pdfReader.Close();
        }

        [Fact(DisplayName = "Deve Obter Dados do Relatório")]
        [Trait("Servico", "Livro Relatorio")]
        public async Task ObterDadosRelatorioAsync_DeveRetornarDadosRelatorio()
        {
            // Arrange
            var dadosEsperados = _livroRelatorioDtoFaker.Generate(3);
            _livroRelatorioRepositorio
                .Setup(repo => repo.ObterDadosRelatorioAsync())
                .ReturnsAsync(dadosEsperados);

            // Act
            var resultado = await _livroRelatorioServico.ObterDadosRelatorioAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(3, resultado.Count);
            Assert.Equal(dadosEsperados, resultado);
        }
    }
}
