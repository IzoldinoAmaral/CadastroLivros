using Bogus;
using CadastroLivros.DTOs;

namespace CadastroLivrosTeste.Unitario.Fixture
{
    public class LivroRelatorioDtoFaker : Faker<LivroRelatorioDto>
    {
        public LivroRelatorioDtoFaker()
        {
            RuleFor(x => x.NomeAutor, lrdf => lrdf.Random.String2(1, 20));
            RuleFor(x => x.TituloLivro, lrdf => lrdf.Random.String2(1, 20));
            RuleFor(x => x.Editora, lrdf => lrdf.Random.String2(1, 20));
            RuleFor(x => x.PrecoBase, lrdf => lrdf.Random.Decimal(1, 20));
            RuleFor(x => x.Edicao, lrdf => lrdf.Random.Int(1, 20));
            RuleFor(x => x.AnoPublicacao, lrdf => lrdf.Random.String2(1, 20));
            RuleFor(x => x.Assuntos, lrdf => lrdf.Random.String2(1, 20));

        }

    }
}
