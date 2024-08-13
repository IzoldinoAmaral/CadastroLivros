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

        public async Task<IActionResult> ListarDetalhes(int CodLivro)
        {
            var livros = await _livroServico.ListarDetalhesAsync(CodLivro);
            return View(livros);

        }

        public async Task<IActionResult> Criar()
        {
            var livro = new Livro()
            {                
                Autores = await _livroServico.BuscarTodosAutoresAsync(),
                Assuntos = await _livroServico.BuscarTodosAssuntosAsync()
            };

            return View(livro);
        }

        public async Task<IActionResult> Editar(int id)
        {

            Livro livro = await _livroServico.BuscarPorCodAsync(id);
            return View(livro);
        }

        [HttpPost]
        public async Task<IActionResult> Atualizar(Livro livro)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _livroServico.AtualizarAsync(livro);
                    TempData["MensagemSucesso"] = "Livro atualizado com sucesso";
                    return RedirectToAction("Index");
                }
                return View("Editar", livro);
            }
            catch (Exception erro)
            {

                TempData["MensagemErro"] = $"Erro ao atualizar livro, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }

        }
        public async Task<IActionResult> ConfirmarDelecao(int id)
        {
            try
            {
                bool deletado = await _livroServico.DeletarAsync(id);
                if (deletado)
                {
                    TempData["MensagemSucesso"] = "Livro deletado com sucesso";
                }
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Erro ao deletar livro, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");

            }
        }
        public async Task<IActionResult> Deletar(int id)
        {
            Livro livro = await _livroServico.BuscarPorCodAsync(id);
            return View(livro);
        }
        [HttpPost]
        public async Task<IActionResult> Criar(Livro livro)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _livroServico.AdicionarAsync(livro);
                    TempData["MensagemSucesso"] = "Livro Cadastrado com sucesso";
                    return RedirectToAction("Index");
                }

                return View(livro);

            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Erro ao cadastrar livro, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }

        }

    }
}
