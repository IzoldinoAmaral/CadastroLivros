using CadastroLivros.DTOs;

namespace CadastroLivros.Interfaces.Servicos
{
    public interface ILivroRelatorioServico
    {
        Task GerarRelatorioAsync(List<LivroRelatorioDto> dadosRelatorio, Stream arquivoSaida);
        Task<List<LivroRelatorioDto>> ObterDadosRelatorioAsync();
    }
}
