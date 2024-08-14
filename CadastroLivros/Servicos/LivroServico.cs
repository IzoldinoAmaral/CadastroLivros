using CadastroLivros.Data;
using CadastroLivros.Extensions;
using CadastroLivros.Interfaces.Repositorios;
using CadastroLivros.Interfaces.Servicos;
using CadastroLivros.Models;
using CadastroLivros.Types;

namespace CadastroLivros.Servicos
{
    public class LivroServico : ILivroServico
    {
        private readonly ILivroRepositorio _livroRepositorio;
        private readonly IFormaCompraServico _formaCompraServico;

        public LivroServico(ILivroRepositorio livroRepositorio, IFormaCompraServico formaCompraServico)
        {
            _livroRepositorio = livroRepositorio ?? throw new ArgumentNullException(nameof(livroRepositorio));
            _formaCompraServico = formaCompraServico ?? throw new ArgumentNullException(nameof(formaCompraServico));
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
            livroDb.PrecoBase = livro.PrecoBase;
            
            if (livro.AssuntosSelecionados != null)
            {
                await _livroRepositorio.DeletarListaAssuntosAsync(livro.AssuntosSelecionados);
                foreach (var assuntoId in livro.AssuntosSelecionados)
                {
                    livroDb.LivroAssuntos.Add(new LivroAssunto
                    {
                        LivroCodl = livro.Codl,
                        AssuntoCodAs = assuntoId
                    });
                }
            }

            if (livro.AutoresSelecionados != null)
            {
                await _livroRepositorio.DeletarListaAutoresAsync(livro.AutoresSelecionados);

                foreach (var autorId in livro.AutoresSelecionados)
                {
                    livroDb.LivroAutores.Add(new LivroAutor
                    {
                        LivroCodl = livro.Codl,
                        AutorCodAu = autorId
                    });
                }
            }




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

        public async Task<IEnumerable<Assunto>> BuscarTodosAssuntosAsync()
        {
            return await _livroRepositorio.BuscarTodosAssuntosAsync();
        }

        public async Task<IEnumerable<Livro>> BuscarTodosAsync()
        {
            return await _livroRepositorio.BuscarTodosAsync();
        }

        public async Task<IEnumerable<Autor>> BuscarTodosAutoresAsync()
        {
            return await _livroRepositorio.BuscarTodosAutoresAsync();
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

        public async Task<DetalhesLivroViewModel> ListarDetalhesAsync(int codLivro)
        {
            var livro = await BuscarPorCodAsync(codLivro);
            var formasDeCompra = await _formaCompraServico.BuscarTodosAsync();

            var viewModel = new DetalhesLivroViewModel
            {
                NomeLivro = livro.Titulo,
                Precos = formasDeCompra.Select(forma => new Preco
                {
                    FormaCompra = forma.Descricao,
                    Desconto = forma.Desconto,
                    ValorFinal = livro.PrecoBase.ComDesconto(forma.Desconto)
                }).ToList()             

            };            
            return viewModel;
        }
    }
}
