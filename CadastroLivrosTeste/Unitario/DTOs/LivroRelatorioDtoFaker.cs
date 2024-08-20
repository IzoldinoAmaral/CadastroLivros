using Bogus;
using CadastroLivros.DTOs;

namespace CadastroLivrosTeste.Unitario.DTOs
{
    public class LivroRelatorioDtoFaker : Faker<LivroRelatorioDto>
    {
        public LivroRelatorioDtoFaker()
        {
            RuleFor(l => l.NomeAutor, f => f.Name.FullName());
            RuleFor(l => l.TituloLivro, f => f.Lorem.Sentence(3));
            RuleFor(l => l.Editora, f => f.Company.CompanyName());
            RuleFor(l => l.PrecoBase, f => f.Finance.Amount(10, 500));
            RuleFor(l => l.Edicao, f => f.Random.Int(1, 10));
            RuleFor(l => l.AnoPublicacao, f => f.Date.Past(20).Year.ToString());
            RuleFor(l => l.Assuntos, f => string.Join(", ", f.Lorem.Words(3)));
        }
    }
}
