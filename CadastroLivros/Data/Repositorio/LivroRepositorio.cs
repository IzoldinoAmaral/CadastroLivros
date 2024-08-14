using CadastroLivros.Interfaces.Repositorios;
using CadastroLivros.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroLivros.Data.Repositorio
{
    public class LivroRepositorio : IGenericoRepositorio<Livro>
    {
        private readonly BancoContext _bancoContext;
        public LivroRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public async Task<bool> AdicionarAsync(Livro livro)
        {

            _bancoContext.Livros.Add(livro);
            
            await _bancoContext.SaveChangesAsync();

            if (livro.AutoresI != null && livro.AutoresI.Any())
            {
                foreach (var autorId in livro.AutoresI)
                {
                    _bancoContext.LivroAutores.Add(new LivroAutor
                    {
                        LivroCodl = livro.Codl,
                        AutorCodAu = autorId
                    });
                }
            }

            if (livro.AssuntosI != null && livro.AssuntosI.Any())
            {
                foreach (var assuntoId in livro.AssuntosI)
                {
                    _bancoContext.LivroAssuntos.Add(new LivroAssunto
                    {
                        LivroCodl = livro.Codl,
                        AssuntoCodAs = assuntoId
                    });
                }
            }

            await _bancoContext.SaveChangesAsync();

            return true;
            
        }

        public async Task<Livro> Atualizar(Livro livro)
        {         
            _bancoContext.Livros.Update(livro);
            await _bancoContext.SaveChangesAsync();
            return livro;           
        }

        public async Task<bool> BuscarPorNomeAsync(string titulo)
        {

            return await _bancoContext.Livros.AnyAsync(l => l.Titulo == titulo);    
            
        }

        public async Task<Livro> BuscarPorCodAsync(int cod)
        {
            return await _bancoContext.Livros.FirstOrDefaultAsync(c => c.Codl == cod);
        }

        public async Task<IEnumerable<Livro>> BuscarTodosAsync()
        {
            return await _bancoContext.Livros.ToListAsync();
        }

        public async Task<bool> DeletarAsync(Livro livro)
        {
            _bancoContext.Livros.Remove(livro);
            await _bancoContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Autor>> BuscarTodosAutoresAsync()
        {
            return await _bancoContext.Autores.ToListAsync();
        }

        public async Task<IEnumerable<Assunto>> BuscarTodosAssuntosAsync()
        {
            return await _bancoContext.Assuntos.ToListAsync();
        }

        public Task<IEnumerable<Livro>> ListarDetalhesAsync(int cod)
        {
            throw new NotImplementedException();
        }
    }
}
