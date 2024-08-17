using Bogus;
using CadastroLivros.Models;

namespace CadastroLivrosTeste.Unitario.Fixture
{
    public class LivroAutorFaker : Faker<LivroAutor>
    {
        public LivroAutorFaker()
        {
            RuleFor(x => x.LivroCodl, laf => laf.Random.Int(1, 10));
            RuleFor(x => x.AutorCodAu, laf => laf.Random.Int(1, 10));
            RuleFor(x => x.Livro, laf => LivroFaker.Gerar(1).First());
            RuleFor(x => x.Autor, laf => new Lazy<Autor>(() => new AutorFaker().Generate()).Value);

        }

        public static LivroAutor LivroAutorFakerComCampoObrigatorioNulo()
        {
            var livroAutorFaker = new Faker<LivroAutor>()
            .RuleFor(x => x.LivroCodl, laf => laf.Random.Int(1, 20))
            .RuleFor(x => x.AutorCodAu, laf => laf.Random.Int(1, 20))
            .RuleFor(x => x.Livro, laf => LivroFaker.Gerar(1).First());            

            var livroAutor = livroAutorFaker.Generate();

            return livroAutor;

        }
    }
}
