using CadastroLivros.Interfaces.Repositorios;
using CadastroLivros.Interfaces.Servicos;
using CadastroLivros.Models;
using CadastroLivros.Types;

namespace CadastroLivros.Servicos
{
    public class AutorServico: IAutorServico
    {
        private readonly IGenericoRepositorio<Autor> _autorRepositorio;

        public AutorServico(IGenericoRepositorio<Autor> autorRepositorio)
        {
            _autorRepositorio = autorRepositorio ?? throw new ArgumentNullException(nameof(autorRepositorio));
        }

        public async Task<bool> AdicionarAsync(Autor autor)
        {
            var existeAutor = await BuscarPorNomeAsync(autor.Nome);
            if (existeAutor.Sucesso)
            {
                throw new InvalidOperationException("O autor já existe.");
            }
            return await _autorRepositorio.AdicionarAsync(autor);
        }

        public async Task<Resultado> AtualizarAsync(Autor autor)
        {
            var autorDb = await BuscarPorCodAsync(autor.CodAu);
            if (autorDb == null)
            {
                return new Resultado { Sucesso = false, Mensagem = "Autor não encontrado." };
            }
            autorDb.Nome = autor.Nome;            
            await _autorRepositorio.Atualizar(autorDb);
            return new Resultado { Sucesso = true, Autor = autorDb };

        }

        public async Task<Autor> BuscarPorCodAsync(int cod)
        {
            return await _autorRepositorio.BuscarPorCodAsync(cod);
        }

        public async Task<Resultado> BuscarPorNomeAsync(string nome)
        {
            var autorDb = await _autorRepositorio.BuscarPorNomeAsync(nome);
            if (!autorDb)
            {
                return new Resultado { Sucesso = false, Mensagem = "Autor não encontrado." };
            }
            return new Resultado { Sucesso = true };

        }

        public async Task<IEnumerable<Autor>> BuscarTodosAsync()
        {
            return await _autorRepositorio.BuscarTodosAsync();
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var autorDb = await BuscarPorCodAsync(id);
            if (autorDb == null)
            {
                return false;
            }
            await _autorRepositorio.DeletarAsync(autorDb);
            return true;
        }
    }
}
