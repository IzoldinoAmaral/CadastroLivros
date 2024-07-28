using Microsoft.AspNetCore.Mvc;

namespace CadastroLivros.Controllers
{
    public class AutorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Criar()
        {
            return View();
        }

        public IActionResult Editar()
        {
            return View();
        }
        public IActionResult Deletar()
        {
            return View();
        }
    }
}
