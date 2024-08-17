using Bogus;
using CadastroLivros.Models;

namespace CadastroLivrosTeste.Unitario.Fixture
{
    public class ErrorViewModelFaker : Faker<ErrorViewModel>
    {
        public ErrorViewModelFaker()
        {
            RuleFor(x => x.RequestId, evmf => evmf.Random.String2(1, 20));
            RuleFor(x => x.ShowRequestId, evmf => evmf.Random.Bool());

        }
    }
}
