using Bogus;
using CadastroLivros.Models;

namespace CadastroLivrosTeste.Unitario.Fixture
{
    public class FormaCompraFaker : Faker<FormaCompra>
    {
        public FormaCompraFaker()
        {
            RuleFor(x => x.CodCom, fcf => fcf.Random.Int(1, 20));
            RuleFor(x => x.Descricao, fcf => fcf.Random.String2(1, 20));
            RuleFor(x => x.Desconto, fcf => fcf.Random.Decimal(1, 20));            
        }

        public static FormaCompra FormaCompraFakerComCampoObrigatorioNulo()
        {
            var formaCompraFaker = new Faker<FormaCompra>()
            .RuleFor(x => x.CodCom, fcf => fcf.Random.Int(1, 20))
            .RuleFor(x => x.Descricao, fcf => fcf.Random.String2(1, 20));

            var formaCompra = formaCompraFaker.Generate();

            return formaCompra;

        }
    }
}
