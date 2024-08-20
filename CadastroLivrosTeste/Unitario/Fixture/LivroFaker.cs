using Bogus;
using CadastroLivros.Models;

namespace CadastroLivrosTeste.Unitario.Fixture
{
    public static class LivroFaker
    {
        public static IEnumerable<Livro> Gerar(int quantidade) 
        {
            return new Faker<Livro>()
            .RuleFor(x => x.Codl, lf => lf.Random.Int(1, 20))
            .RuleFor(x => x.Titulo, lf => lf.Random.String2(1, 20))
            .RuleFor(x => x.Editora, lf => lf.Random.String2(1, 10))
            .RuleFor(x => x.Edicao, lf => lf.Random.Int(1, 5))
            .RuleFor(x => x.AnoPublicacao, lf => lf.Random.String2(2000, 2024))
            .RuleFor(x => x.PrecoBase, lf => lf.Random.Decimal(1, 100))
            .Generate(quantidade);

        }

        public static Livro LivroFakerComCampoObrigatorioNulo()
        {
            var livroFaker = new Faker<Livro>()
            .RuleFor(x => x.Codl, lf => lf.Random.Int(1, 20))
            .RuleFor(x => x.Titulo, lf => lf.Random.String2(1, 20))
            .RuleFor(x => x.Editora, lf => lf.Random.String2(1, 10))
            .RuleFor(x => x.Edicao, lf => lf.Random.Int(1, 5))
            .RuleFor(x => x.AnoPublicacao, lf => lf.Random.String2(2000, 2024));
            

            var livro = livroFaker.Generate();

            return livro;

        }

        public static Livro GerarComTodosCamposPreenchidos()
        {
            return new Faker<Livro>()
                            .RuleFor(x => x.Codl, lf => lf.Random.Int(1, 20))
                            .RuleFor(x => x.Titulo, lf => lf.Random.String2(5, 20))  
                            .RuleFor(x => x.Editora, lf => lf.Random.String2(5, 50)) 
                            .RuleFor(x => x.Edicao, lf => lf.Random.Int(1, 5))
                            .RuleFor(x => x.AnoPublicacao, lf => lf.Random.String2(1900, DateTime.Now.Year))
                            .RuleFor(x => x.PrecoBase, lf => lf.Random.Decimal(1, 100))
                            .RuleFor(x => x.Assuntos, lf => new AssuntoFaker().Generate(3))  
                            .RuleFor(x => x.Autores, lf => new AutorFaker().Generate(2))  
                            .RuleFor(x => x.LivroAutores, lf => new LivroAutorFaker().Generate(2))  
                            .RuleFor(x => x.LivroAssuntos, lf => new LivroAssuntoFaker().Generate(3))  
                            .RuleFor(x => x.AssuntosSelecionados, lf => lf.Random.ListItems([1, 2, 3, 4, 5], 2))  
                            .RuleFor(x => x.AutoresSelecionados, lf => lf.Random.ListItems([1, 2, 3, 4, 5], 2))  
                            .Generate();
        }

        public static Task<Livro> GerarComTodosCamposPreenchidosAsync()
        {
            return Task.FromResult(GerarComTodosCamposPreenchidos());
        }
    }
}
