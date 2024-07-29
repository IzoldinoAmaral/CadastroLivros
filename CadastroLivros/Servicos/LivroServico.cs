using CadastroLivros.Interfaces.Repositorios;
using CadastroLivros.Interfaces.Servicos;
using CadastroLivros.Models;
using CadastroLivros.Types;

namespace CadastroLivros.Servicos
{
    public class LivroServico : ILivroServico
    {
        private readonly IGenericoRepositorio<Livro> _livroRepositorio;

        public LivroServico(IGenericoRepositorio<Livro> livroRepositorio)
        {
            _livroRepositorio = livroRepositorio ?? throw new ArgumentNullException(nameof(livroRepositorio));
        }

        public async Task<bool> AdicionarAsync(Livro livro)
        {
            var existeLivro = await BuscarPorNomeAsync(livro.Titulo);
            if (existeLivro.Sucesso)
            {
                throw new InvalidOperationException("O livro já existe.");
            }
            return await _livroRepositorio.AdicionarAsync(livro);
        }

        public async Task<Resultado> AtualizarAsync(Livro livro)
        {
            var livroDb = await BuscarPorCodAsync(livro.Codl);
            if (livroDb == null)
            {
                return new Resultado { Sucesso = false, Mensagem = "Livro não encontrado." };
            }
            livroDb.Titulo = livro.Titulo;
            livroDb.Editora = livro.Editora;
            livroDb.Edicao = livro.Edicao;
            livroDb.AnoPublicacao = livro.AnoPublicacao;

            await _livroRepositorio.Atualizar(livroDb);
            return new Resultado { Sucesso = true, Livro = livroDb };

        }

        public async Task<Livro> BuscarPorCodAsync(int cod)
        {
            return await _livroRepositorio.BuscarPorCodAsync(cod);
        }

        public async Task<Resultado> BuscarPorNomeAsync(string titulo)
        {
            var livroDb = await _livroRepositorio.BuscarPorNomeAsync(titulo);
            if (!livroDb)
            {
                return new Resultado { Sucesso = false, Mensagem = "Livro não encontrado." };
            }
            return new Resultado { Sucesso = true };

        }

        public async Task<IEnumerable<Livro>> BuscarTodosAsync()
        {
            return await _livroRepositorio.BuscarTodosAsync();
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var livroDb = await BuscarPorCodAsync(id);
            if (livroDb == null)
            {
                return false;
            }
            await _livroRepositorio.DeletarAsync(livroDb);
            return true;
        }
    }
}
