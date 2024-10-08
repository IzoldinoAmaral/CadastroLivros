﻿using CadastroLivros.Controllers.Base;
using CadastroLivros.Interfaces.Servicos;
using CadastroLivros.Models;
using Microsoft.AspNetCore.Mvc;

namespace CadastroLivros.Controllers
{
    public class AutorController : BaseController
    {
        private readonly IAutorServico _autorServico;
        public AutorController(IAutorServico autorServico)
        {
            _autorServico = autorServico ?? throw new ArgumentNullException(nameof(autorServico));
        }
        public async Task<IActionResult> Index()
        {
            var autors = await _autorServico.BuscarTodosAsync();
            return View(autors);
        }

        public IActionResult Criar()
        {
            return View();
        }

        public async Task<IActionResult> Editar(int id)
        {

            Autor autor = await _autorServico.BuscarPorCodAsync(id);
            return View(autor);
        }

        [HttpPost]
        public async Task<IActionResult> Atualizar(Autor autor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _autorServico.AtualizarAsync(autor);
                    TempData["MensagemSucesso"] = "Autor atualizado com sucesso";
                    return RedirectToAction("Index");
                }
                return View("Editar", autor);
            }
            catch (Exception erro)
            {
                return ExceptionHandler(erro, "Atualizar autor");
            }
        }
        public async Task<IActionResult> ConfirmarDelecao(int id)
        {
            try
            {
                bool deletado = await _autorServico.DeletarAsync(id);
                if (deletado)
                {
                    TempData["MensagemSucesso"] = "Autor deletado com sucesso";
                }
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                return ExceptionHandler(erro, "Deletar autor");

            }
        }
        public async Task<IActionResult> Deletar(int id)
        {
            Autor autor = await _autorServico.BuscarPorCodAsync(id);
            return View(autor);
        }
        [HttpPost]
        public async Task<IActionResult> Criar(Autor autor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _autorServico.AdicionarAsync(autor);
                    TempData["MensagemSucesso"] = "Autor Cadastrado com sucesso";
                    return RedirectToAction("Index");
                }

                return View(autor);

            }
            catch (Exception erro)
            {
                return ExceptionHandler(erro, "Cadastrar autor");
            }

        }
    }
}
