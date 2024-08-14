using CadastroLivros.DTOs;

namespace CadastroLivros.Interfaces.Repositorios
{
    public interface ILivroRelatorioRepositorio
    {
        Task<List<LivroRelatorioDto>> ObterDadosRelatorioAsync();
    }
}
