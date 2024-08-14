using CadastroLivros.DTOs;
using CadastroLivros.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace CadastroLivros.Data.Repositorio
{
    public class LivroRelatorioRepositorio: ILivroRelatorioRepositorio
    {
        private readonly BancoContext _bancoContext;
        public LivroRelatorioRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public async Task<List<LivroRelatorioDto>> ObterDadosRelatorioAsync()
        {
            return await _bancoContext.VwLivroRelatorio.ToListAsync();
        }
    }
}
