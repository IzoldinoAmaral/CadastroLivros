using CadastroLivros.Data.Repositorio;
using CadastroLivros.Interfaces.Servicos;
using CadastroLivros.Models;
using Microsoft.AspNetCore.Mvc;

namespace CadastroLivros.Controllers
{
    public class LivroController : Controller
    {
        private readonly ILivroServico _livroServico;
        public LivroController(ILivroServico livroServico)
        {
            _livroServico = livroServico ?? throw new ArgumentNullException(nameof(livroServico));
        }
        public async Task<IActionResult> Index()
        {
            var livros = await _livroServico.BuscarTodosAsync();
            return View(livros);
        }

        public IActionResult Criar()
        {
            return View();
        }

        public async Task<IActionResult> Editar(int id)
        {

            Livro livro = await _livroServico.BuscarPorCodAsync(id);
            return View(livro);
        }

        [HttpPost]
        public async Task<IActionResult> Atualizar(Livro livro)
        {
             var resultado = await _livroServico.AtualizarAsync(livro);
             if (!resultado.Sucesso)
             {
                 return NotFound(resultado.Mensagem);
             }
             return RedirectToAction("Index");
        }
        public async Task<IActionResult> ConfirmarDelecao(int id)
        {
            await _livroServico.DeletarAsync(id);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Deletar(int id)
        {
            Livro livro = await _livroServico.BuscarPorCodAsync(id);
            return View(livro);
        }
        [HttpPost]
        public async Task<IActionResult> Criar(Livro livro)
        {
            await _livroServico.AdicionarAsync(livro);
            return RedirectToAction("Index");

        }

    }
}
