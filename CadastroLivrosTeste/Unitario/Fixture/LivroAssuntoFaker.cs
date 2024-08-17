using Bogus;
using CadastroLivros.Models;

namespace CadastroLivrosTeste.Unitario.Fixture
{
    public class LivroAssuntoFaker : Faker<LivroAssunto>
    {
        public LivroAssuntoFaker()
        {
            RuleFor(x => x.LivroCodl, lasf => lasf.Random.Int(1, 10));
            RuleFor(x => x.AssuntoCodAs, lasf => lasf.Random.Int(1, 10));
            RuleFor(x => x.Livro, lasf => LivroFaker.Gerar(1).First());
            RuleFor(x => x.Assunto, lasf => new Lazy<Assunto>(() => new AssuntoFaker().Generate()).Value);

        }

        public static LivroAssunto LivroAssuntoFakerComCampoObrigatorioNulo()
        {
            var livroAssuntoFaker = new Faker<LivroAssunto>()
            .RuleFor(x => x.LivroCodl, lasf => lasf.Random.Int(1, 20))
            .RuleFor(x => x.AssuntoCodAs, lasf => lasf.Random.Int(1, 20))
            .RuleFor(x => x.Livro, lasf => LivroFaker.Gerar(1).First());

            var livroAssunto = livroAssuntoFaker.Generate();

            return livroAssunto;

        }
    }
}
