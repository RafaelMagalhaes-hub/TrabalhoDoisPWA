using System;
using System.Collections.Generic;
using System.Diagnostics;
using Buffet.Models.Acesso;
using Buffet.Models.Buffet.Cliente;
using Buffet.RequestModels;
using Buffet.ViewModels.Acesso;
using Microsoft.AspNetCore.Mvc;

namespace Buffet.Controllers
{
    public class AcessoController : Controller
    {
        private readonly AcessoService _acessoService;

        public AcessoController(AcessoService acessoService)
        {
            _acessoService = acessoService;
        }


        public IActionResult RecuperarConta()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            var viewModels = new CadastrarViewModels();
            viewModels.mensagem = (string)TempData["msg-cadastro"];
            return View(viewModels);
        }

        [HttpGet]
        public IActionResult Login()
        {
            var viewModels2 = new AutenticarViewModels();
            viewModels2.mensagem = (string)TempData["msg-autenticacao"];
            return View(viewModels2);
        }

        public RedirectResult chamaArea() {
            return Redirect("/PainelDoUsuario/Index");
        }


        [HttpPost]
        public async System.Threading.Tasks.Task<RedirectToActionResult> CadastrarAsync(AcessoCadastrarRequestModels request)
        {
            var email = request.email;
            var senha = request.senha;
            var confirmacao = request.confirmacao;

            if (email == null) {
                TempData["msg-cadastro"] = "Por favor, informe o email.";
                return RedirectToAction("Cadastrar");
            }

            try
            {
                await _acessoService.registrarUsuarioAsync(email, senha);
                TempData["msg-autenticacao"] = "Cadastro realizado com sucesso! Agora faça o login.";
                return RedirectToAction("Login");
            }
            catch (Exception exception){
                TempData["msg-cadastro"] = exception.Message;
                return RedirectToAction("Cadastrar");
            }
           

        }

        //metodo pedido para redirecionar para a área interna.
        [HttpPost]
        public async System.Threading.Tasks.Task<RedirectResult> AutenticarAsync(AcessoAutenticarRequestModels request)
        {
            var email = request.email;
            var senha = request.senha;

            if (email == null)
            {
                TempData["msg-autenticacao"] = "Por favor, informe o email.";
                return Redirect("/Acesso/Login");
            }

            try
            {
                await _acessoService.AutenticarUsuario(email, senha);
                return chamaArea();
            }
            catch (Exception exception)
            {
                TempData["msg-autenticacao"] = "Email ou senha incorretos";
                return Redirect("/Acesso/Login");
            }


        }
    }
}