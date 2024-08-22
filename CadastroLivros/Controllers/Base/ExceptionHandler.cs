using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CadastroLivros.Controllers.Base
{
    public abstract class BaseController : Controller
    {
        public IActionResult ExceptionHandler(Exception ex, string actionName)
        {
            string mensagemErro;

            if (ex is DbUpdateException dbEx)
            {
                mensagemErro = $"Erro ao {actionName} no banco de dados, detalhe do erro: {dbEx.Message}";
            }
            else if (ex is SqlException sqlEx)
            {
                mensagemErro = $"Erro de SQL, detalhe do erro: {sqlEx.Message}";
            }
            else
            {
                mensagemErro = $"Erro ao {actionName}, detalhe do erro: {ex.Message}";
            }

            TempData["MensagemErro"] = mensagemErro;
            return RedirectToAction("Index");
        }
    }
}
