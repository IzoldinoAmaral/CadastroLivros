﻿using CadastroLivros.Controllers.Base;
using CadastroLivros.Interfaces.Servicos;
using CadastroLivros.Models;
using Microsoft.AspNetCore.Mvc;

namespace CadastroLivros.Controllers
{
    public class AssuntoController : BaseController
    {
        private readonly IAssuntoServico _assuntoServico;
        public AssuntoController(IAssuntoServico assuntoServico)
        {
            _assuntoServico = assuntoServico ?? throw new ArgumentNullException(nameof(assuntoServico));
        }
        public async Task<IActionResult> Index()
        {
            var assuntos = await _assuntoServico.BuscarTodosAsync();
            return View(assuntos);
        }

        public IActionResult Criar()
        {
            return View();
        }

        public async Task<IActionResult> Editar(int id)
        {

            Assunto assunto = await _assuntoServico.BuscarPorCodAsync(id);
            return View(assunto);
        }

        [HttpPost]
        public async Task<IActionResult> Atualizar(Assunto assunto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _assuntoServico.AtualizarAsync(assunto);
                    TempData["MensagemSucesso"] = "Assunto atualizado com sucesso";
                    return RedirectToAction("Index");
                }
                return View("Editar", assunto);
            }
            catch (Exception erro)
            {

                return ExceptionHandler(erro, "Atualizar assunto");
            }
        }
        public async Task<IActionResult> ConfirmarDelecao(int id)
        {
            try
            {
                bool deletado = await _assuntoServico.DeletarAsync(id);
                if (deletado)
                {
                    TempData["MensagemSucesso"] = "Assunto deletado com sucesso";
                }
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                return ExceptionHandler(erro, "Deletar assunto");

            }
        }
        public async Task<IActionResult> Deletar(int id)
        {
            Assunto assunto = await _assuntoServico.BuscarPorCodAsync(id);
            return View(assunto);
        }
        [HttpPost]
        public async Task<IActionResult> Criar(Assunto assunto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _assuntoServico.AdicionarAsync(assunto);
                    TempData["MensagemSucesso"] = "Assunto Cadastrado com sucesso";
                    return RedirectToAction("Index");
                }

                return View(assunto);

            }
            catch (Exception erro)
            {
                return ExceptionHandler(erro, "Cadastrar assunto");
            }

        }
    }
}
