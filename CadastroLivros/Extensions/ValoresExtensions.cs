namespace CadastroLivros.Extensions
{
    public static class ValoresExtensions
    {
        public static decimal ComDesconto(this decimal valor, decimal desconto)
        {
            return valor * (1 - desconto / 100);
        }
    }
}
