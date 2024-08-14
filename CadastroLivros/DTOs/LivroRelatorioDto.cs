namespace CadastroLivros.DTOs
{
    public class LivroRelatorioDto
    {
        public string NomeAutor { get; set; }
        public string TituloLivro { get; set; }
        public string Editora { get; set; }
        public decimal PrecoBase { get; set; }
        public int Edicao { get; set; }
        public string AnoPublicacao { get; set; }
        public string Assuntos { get; set; }
    }
}
