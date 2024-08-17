using CadastroLivros.DTOs;
using CadastroLivros.Interfaces.Repositorios;
using CadastroLivros.Interfaces.Servicos;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CadastroLivros.Servicos
{
    public class LivroRelatorioServico : ILivroRelatorioServico
    {
        private readonly ILivroRelatorioRepositorio _livroRelatorioRepositorio;        

        public LivroRelatorioServico(ILivroRelatorioRepositorio livroRelatorioRepositorio)
        {
            _livroRelatorioRepositorio = livroRelatorioRepositorio ?? throw new ArgumentNullException(nameof(livroRelatorioRepositorio));
        }
        public async Task GerarRelatorioAsync(List<LivroRelatorioDto> dadosRelatorio, Stream arquivoSaida)
        {
            iTextSharp.text.Document doc = new(PageSize.A4.Rotate());
            PdfWriter.GetInstance(doc, arquivoSaida);
            doc.Open();

            Paragraph titulo = new("Relatório de Livros por Autor")
            {
                Alignment = Element.ALIGN_CENTER
            };
            doc.Add(titulo);
            doc.Add(new Paragraph(" "));

            PdfPTable tabela = new(7);
            tabela.AddCell("Autor");
            tabela.AddCell("Livro");
            tabela.AddCell("Editora");
            tabela.AddCell("Preço Base");
            tabela.AddCell("Edição");
            tabela.AddCell("Ano Publicação");
            tabela.AddCell("Assuntos");

            foreach (var item in dadosRelatorio)
            {
                tabela.AddCell(item.NomeAutor);
                tabela.AddCell(item.TituloLivro);
                tabela.AddCell(item.Editora);
                tabela.AddCell(item.PrecoBase.ToString("F2"));
                tabela.AddCell(item.Edicao.ToString());
                tabela.AddCell(item.AnoPublicacao.ToString());
                tabela.AddCell(item.Assuntos);
            }

            doc.Add(tabela);
            doc.Close();
        }

        public async Task<List<LivroRelatorioDto>> ObterDadosRelatorioAsync()
        {
            return await _livroRelatorioRepositorio.ObterDadosRelatorioAsync();
        }
    }
}
