using Bogus;
using CadastroLivros.Models;

namespace CadastroLivrosTeste.Unitario.Fixture
{
    public class PrecoFaker : Faker<Preco>
    {
        public PrecoFaker()
        {
            RuleFor(x => x.FormaCompra, pf => pf.Random.String2(1, 20));
            RuleFor(x => x.Desconto, pf => pf.Random.Decimal(1, 20));
            RuleFor(x => x.ValorFinal, pf => pf.Random.Decimal(1, 200));

        }

        public static Preco PrecoFakerComCampoObrigatorioNulo()
        {
            var precoFaker = new Faker<Preco>()
            .RuleFor(x => x.FormaCompra, pf => pf.Random.String2(1, 20))
            .RuleFor(x => x.Desconto, pf => pf.Random.Decimal(1, 20));

            var preco = precoFaker.Generate();

            return preco;

        }
    }
}
