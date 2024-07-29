using CadastroLivros.Interfaces.Repositorios;
using CadastroLivros.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroLivros.Data.Repositorio
{
    public class AssuntoRepositorio : IGenericoRepositorio<Assunto>
    {
        private readonly BancoContext _bancoContext;
        public AssuntoRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public async Task<bool> AdicionarAsync(Assunto assunto)
        {

            _bancoContext.Assuntos.Add(assunto);
            await _bancoContext.SaveChangesAsync();
            return true;

        }

        public async Task<Assunto> Atualizar(Assunto assunto)
        {
            _bancoContext.Assuntos.Update(assunto);
            await _bancoContext.SaveChangesAsync();
            return assunto;
        }

        public async Task<bool> BuscarPorNomeAsync(string descricao)
        {

            return await _bancoContext.Assuntos.AnyAsync(l => l.Descricao == descricao);

        }

        public async Task<Assunto> BuscarPorCodAsync(int cod)
        {
            return await _bancoContext.Assuntos.FirstOrDefaultAsync(c => c.CodAs == cod);
        }

        public async Task<IEnumerable<Assunto>> BuscarTodosAsync()
        {
            return await _bancoContext.Assuntos.ToListAsync();
        }

        public async Task<bool> DeletarAsync(Assunto assunto)
        {
            _bancoContext.Assuntos.Remove(assunto);
            await _bancoContext.SaveChangesAsync();
            return true;
        }
    }
}
