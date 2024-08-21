using CadastroLivros.Data.Repositorio;
using CadastroLivros.Interfaces.Repositorios;
using CadastroLivros.Interfaces.Servicos;
using CadastroLivros.Models;
using CadastroLivros.Types;

namespace CadastroLivros.Servicos
{
    public class AssuntoServico : IAssuntoServico
    {
        private readonly IGenericoRepositorio<Assunto> _assuntoRepositorio;

        public AssuntoServico(IGenericoRepositorio<Assunto> assuntoRepositorio)
        {
            _assuntoRepositorio = assuntoRepositorio ?? throw new ArgumentNullException(nameof(assuntoRepositorio));
        }

        public async Task<bool> AdicionarAsync(Assunto assunto)
        {

            var existeAssunto = await BuscarPorNomeAsync(assunto.Descricao);
            if (existeAssunto.Sucesso)
            {
                var assuntoExistente = await _assuntoRepositorio.ObterPorNomeAsync(assunto.Descricao);
                assuntoExistente.Ativo = true;

                await _assuntoRepositorio.Atualizar(assuntoExistente);
                return true;
            }
            return await _assuntoRepositorio.AdicionarAsync(assunto);

        }

        public async Task<Resultado> AtualizarAsync(Assunto assunto)
        {
            var assuntoDb = await BuscarPorCodAsync(assunto.CodAs);
            if (assuntoDb == null)
            {
                return new Resultado { Sucesso = false, Mensagem = "Assunto não encontrado." };
            }
            assuntoDb.Descricao = assunto.Descricao;
            await _assuntoRepositorio.Atualizar(assuntoDb);
            return new Resultado { Sucesso = true, Assunto = assuntoDb };

        }

        public async Task<Assunto> BuscarPorCodAsync(int cod)
        {
            return await _assuntoRepositorio.BuscarPorCodAsync(cod);
        }

        public async Task<Resultado> BuscarPorNomeAsync(string titulo)
        {
            var assuntoDb = await _assuntoRepositorio.BuscarPorNomeAsync(titulo);
            if (!assuntoDb)
            {
                return new Resultado { Sucesso = false, Mensagem = "Assunto não encontrado." };
            }
            return new Resultado { Sucesso = true };

        }

        public async Task<IEnumerable<Assunto>> BuscarTodosAsync()
        {
            return await _assuntoRepositorio.BuscarTodosAsync();
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var assuntoDb = await BuscarPorCodAsync(id);
            if (assuntoDb == null)
            {
                return false;
            }
            assuntoDb.Ativo = false;
            await _assuntoRepositorio.DeletarAsync(assuntoDb);
            return true;
        }
    }
}
