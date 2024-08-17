using Bogus;
using CadastroLivros.Models;

namespace CadastroLivrosTeste.Unitario.Fixture
{
    public class DetalhesLivroViewModelFaker : Faker<DetalhesLivroViewModel>
    {
        public DetalhesLivroViewModelFaker()
        {
            RuleFor(x => x.NomeLivro, dlvmf => dlvmf.Random.String2(1, 20));
            RuleFor(x => x.Precos, dlvmf => new PrecoFaker().Generate(2));

        }
    }
}
