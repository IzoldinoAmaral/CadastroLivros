using Bogus;
using CadastroLivros.Types;

namespace CadastroLivrosTeste.Unitario.Fixture
{
    public static class ResultadoFaker
    {

        public static Resultado GerarComLivro(bool sucesso = true)
        {

            return new Faker<Resultado>()
                .RuleFor(r => r.Sucesso, f => sucesso)
                .RuleFor(r => r.Mensagem, f => sucesso ? "Operação realizada com sucesso" : "Erro ao realizar a operação")
                .RuleFor(r => r.Livro, f => sucesso ? LivroFaker.GerarComTodosCamposPreenchidos() : null)
                .RuleFor(r => r.Autor, f => sucesso ? new AutorFaker().Generate() : null)
                .RuleFor(r => r.Assunto, f => sucesso ? new AssuntoFaker().Generate() : null)
                .RuleFor(r => r.FormaCompra, f => sucesso ? new FormaCompraFaker().Generate() : null)
                .Generate();
        }



        public static Task<Resultado> GerarComLivroAsync(bool sucesso = true)
        {
            return Task.FromResult(GerarComLivro(sucesso));
        }
    }
}
