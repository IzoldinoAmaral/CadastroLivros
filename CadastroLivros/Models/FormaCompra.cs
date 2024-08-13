using System.ComponentModel.DataAnnotations;

namespace CadastroLivros.Models
{
    public class FormaCompra
    {
        public int CodCom { get; set; }
        [Required(ErrorMessage = "Digite a Forma de Compra")]
        [StringLength(20)]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "Digite o desconto para a forma de Compra")]
        public decimal Desconto { get; set; }

    }
}
