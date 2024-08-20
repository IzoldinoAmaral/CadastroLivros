using Bogus;
using CadastroLivros.Models;

namespace CadastroLivrosTeste.Unitario.Fixture
{
    public class AssuntoFaker : Faker<Assunto>
    {
        private static int _codAsCounter = 1;
        public AssuntoFaker()
        {
            RuleFor(x => x.CodAs, asf => _codAsCounter++);
            RuleFor(x => x.Descricao, asf => asf.Random.String2(1, 30));
            RuleFor(x => x.Ativo, asf => asf.Random.Bool());
            RuleFor(x => x.Livros, _ => []);
            RuleFor(x => x.LivroAssuntos, _ => []);

        }
        public static Assunto GerarAssunto()
        {
            var assuntoFaker = new AssuntoFaker();
            return assuntoFaker.Generate();
        }


        public static Assunto AssuntoFakerComCampoObrigatorioNulo()
        {
            var assuntoFaker = new Faker<Assunto>()
            .RuleFor(x => x.CodAs, asf => asf.Random.Int(1, 20))
            .RuleFor(x => x.Descricao, asf => asf.Random.String2(1, 30))
            .RuleFor(x => x.Livros, _ => []);

            var assunto = assuntoFaker.Generate();

            return assunto;

        }

        public static IList<Assunto> GerarLista(int quantidade)
        {
            var assuntoFaker = new AssuntoFaker();
            return assuntoFaker.Generate(quantidade);
        }
    }
}
