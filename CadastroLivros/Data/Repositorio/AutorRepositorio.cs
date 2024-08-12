using CadastroLivros.Interfaces.Repositorios;
using CadastroLivros.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroLivros.Data.Repositorio
{
    public class AutorRepositorio : IGenericoRepositorio<Autor>
    {
        private readonly BancoContext _bancoContext;
        public AutorRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public async Task<bool> AdicionarAsync(Autor autor)
        {

            _bancoContext.Autores.Add(autor);
            await _bancoContext.SaveChangesAsync();
            return true;

        }

        public async Task<Autor> Atualizar(Autor autor)
        {
            _bancoContext.Autores.Update(autor);
            await _bancoContext.SaveChangesAsync();
            return autor;
        }

        public async Task<bool> BuscarPorNomeAsync(string nome)
        {

            return await _bancoContext.Autores.AnyAsync(l => l.Nome == nome);

        }

        public async Task<Autor> BuscarPorCodAsync(int cod)
        {
            return await _bancoContext.Autores.FirstOrDefaultAsync(c => c.CodAu == cod);
        }

        public async Task<IEnumerable<Autor>> BuscarTodosAsync()
        {
            return await _bancoContext.Autores.ToListAsync();
        }

        public async Task<bool> DeletarAsync(Autor autor)
        {
            _bancoContext.Autores.Remove(autor);
            await _bancoContext.SaveChangesAsync();
            return true;
        }

        public Task<IEnumerable<Autor>> BuscarTodosAutoresAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Assunto>> BuscarTodosAssuntosAsync()
        {
            throw new NotImplementedException();
        }
    }
}
