using CadastroLivros.Interfaces.Servicos;
using CadastroLivros.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CadastroLivros.Controllers
{
    public class LivroController : Controller
    {
        private readonly ILivroServico _livroServico;
        private readonly IAutorServico _autorServico;
        private readonly IAssuntoServico _assuntoServico;
        private readonly ILivroRelatorioServico _livroRelatorioServico;
        public LivroController(ILivroServico livroServico, IAutorServico autorServico, IAssuntoServico assuntoServico, ILivroRelatorioServico livroRelatorioServico)
        {
            _livroServico = livroServico ?? throw new ArgumentNullException(nameof(livroServico));
            _autorServico = autorServico ?? throw new ArgumentNullException(nameof(autorServico));
            _assuntoServico = assuntoServico ?? throw new ArgumentNullException(nameof(assuntoServico));
            _livroRelatorioServico = livroRelatorioServico ?? throw new ArgumentNullException(nameof(livroRelatorioServico));
        }
        public async Task<IActionResult> GerarRelatorio()
        {

            var dadosRelatorio = await _livroRelatorioServico.ObterDadosRelatorioAsync();

            using var memoryStream = new MemoryStream();
            await _livroRelatorioServico.GerarRelatorioAsync(dadosRelatorio, memoryStream);

            byte[] fileBytes = memoryStream.ToArray();

            return File(fileBytes, "application/pdf", "RelatorioLivros.pdf");
        }
        public async Task<IActionResult> Index()
        {
            var livros = await _livroServico.BuscarTodosAsync();
            return View(livros);

        }

        public async Task<IActionResult> ListarDetalhes(int id)
        {
            var livros = await _livroServico.ListarDetalhesAsync(id);
            return View(livros);

        }

        public async Task<IActionResult> Criar()
        {
            var autores = await _livroServico.BuscarTodosAutoresAsync() ?? [];
            var assuntos = await _livroServico.BuscarTodosAssuntosAsync() ?? [];

            ViewBag.Autores = new SelectList(autores, "CodAu", "Nome");
            ViewBag.Assuntos = new SelectList(assuntos, "CodAs", "Descricao");

            var livro = new Livro();
            return View(livro);
        }

        public async Task<IActionResult> Editar(int id)
        {

            Livro livro = await _livroServico.BuscarPorCodAsync(id);
            ViewBag.Autores = new SelectList(await _autorServico.BuscarTodosAsync(), "CodAu", "Nome");
            ViewBag.Assuntos = new SelectList(await _assuntoServico.BuscarTodosAsync(), "CodAs", "Descricao");
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
            if (livro == null)
            {
                TempData["MensagemErro"] = "Livro não encontrado.";
                return RedirectToAction("Index");
            }

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

                ViewBag.Autores = new SelectList(await _livroServico.BuscarTodosAutoresAsync() ?? [], "CodAu", "Nome");
                ViewBag.Assuntos = new SelectList(await _livroServico.BuscarTodosAssuntosAsync() ?? [], "CodAs", "Descricao");
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
