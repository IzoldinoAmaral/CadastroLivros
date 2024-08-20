using Bogus;
using CadastroLivros.Models;

namespace CadastroLivrosTeste.Unitario.Fixture
{
    public class AutorFaker : Faker<Autor>
    {
        public AutorFaker()
        {
            RuleFor(x => x.CodAu, af => af.Random.Int(1, 20));
            RuleFor(x => x.Nome, af => af.Random.String2(1, 30));
            RuleFor(x => x.Ativo, af => af.Random.Bool());
            RuleFor(x => x.Livros, _ => []);
            RuleFor(x => x.LivrosAutores, _ => []);

        }

        public static Autor AutorFakerComCampoObrigatorioNulo()
        {
            var autorFaker = new Faker<Autor>()
            .RuleFor(x => x.CodAu, af => af.Random.Int(1, 20))
            .RuleFor(x => x.Nome, af => af.Random.String2(1, 30))
            .RuleFor(x => x.Ativo, af => af.Random.Bool())
            .RuleFor(x => x.Livros, _ => []);
            var autor = autorFaker.Generate();

            return autor;

        }

        public static IList<Autor> GerarLista(int quantidade)
        {
            var autorFaker = new AutorFaker();
            return autorFaker.Generate(quantidade);
        }

    }
}
