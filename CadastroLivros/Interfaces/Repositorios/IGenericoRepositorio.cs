namespace CadastroLivros.Interfaces.Repositorios
{
    public interface IGenericoRepositorio<T> where T : class
    {
        Task<IEnumerable<T>> BuscarTodosAsync();
        Task<T> BuscarPorCodAsync(int cod);
        Task<bool> BuscarPorNomeAsync(string descricao);
        Task<bool> AdicionarAsync(T entidade);
        Task<T> Atualizar(T entidade);
        Task<bool> DeletarAsync(T entidade);
    }


}
