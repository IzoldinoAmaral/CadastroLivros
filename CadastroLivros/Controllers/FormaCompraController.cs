using CadastroLivros.Interfaces.Servicos;
using CadastroLivros.Models;
using Microsoft.AspNetCore.Mvc;

namespace CadastroLivros.Controllers
{
    public class FormaCompraController : Controller
    {
        private readonly IFormaCompraServico _formaCompraServico;
        public FormaCompraController(IFormaCompraServico formaCompraServico)
        {
            _formaCompraServico = formaCompraServico ?? throw new ArgumentNullException(nameof(formaCompraServico));
        }
        public async Task<IActionResult> Index()
        {
            var formaCompras = await _formaCompraServico.BuscarTodosAsync();
            return View(formaCompras);
        }

        public IActionResult Criar()
        {
            return View();
        }

        public async Task<IActionResult> Editar(int id)
        {

            FormaCompra formaCompra = await _formaCompraServico.BuscarPorCodAsync(id);
            return View(formaCompra);
        }

        [HttpPost]
        public async Task<IActionResult> Atualizar(FormaCompra formaCompra)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _formaCompraServico.AtualizarAsync(formaCompra);
                    TempData["MensagemSucesso"] = "Forma de Compra atualizado com sucesso";
                    return RedirectToAction("Index");
                }
                return View("Editar", formaCompra);
            }
            catch (Exception erro)
            {

                TempData["MensagemErro"] = $"Erro ao atualizar forma de Compra, detalhe do erro:{erro.Message}";
                return RedirectToAction("Index");
            }
        }
        public async Task<IActionResult> ConfirmarDelecao(int id)
        {
            try
            {
                bool deletado = await _formaCompraServico.DeletarAsync(id);
                if (deletado)
                {
                    TempData["MensagemSucesso"] = "Forma de Compra deletado com sucesso";
                }
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Erro ao deletar forma de Compra, detalhe do erro:{erro.Message}";
                return RedirectToAction("Index");

            }
        }
        public async Task<IActionResult> Deletar(int id)
        {
            FormaCompra formaCompra = await _formaCompraServico.BuscarPorCodAsync(id);
            return View(formaCompra);
        }
        [HttpPost]
        public async Task<IActionResult> Criar(FormaCompra formaCompra)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _formaCompraServico.AdicionarAsync(formaCompra);
                    TempData["MensagemSucesso"] = "Forma de Compra Cadastrado com sucesso";
                    return RedirectToAction("Index");
                }

                return View(formaCompra);

            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Erro ao cadastrar forma de Compra, detalhe do erro:{erro.Message}";
                return RedirectToAction("Index");
            }

        }
    }
}
