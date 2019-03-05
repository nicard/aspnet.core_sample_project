﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Alura.ListaLeitura.HttpClientes;
using Alura.ListaLeitura.Seguranca;
using Alura.ListaLeitura.WebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Alura.ListaLeitura.WebApp.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly AuthApiClient _auth;

        public UsuarioController(AuthApiClient auth)
        {
            _auth = auth;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _auth.PostLoginAsync(model);
                //var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, false, false);
                if (result.Succeeded)
                {
                    List<Claim> politicas = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Login),
                        new Claim("Token", result.Token)
                    };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(politicas, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(String.Empty, "Erro na autenticação");
                return View(model);
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                /*var user = new Usuario { UserName = model.Login };
                var result = await HttpContext.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    await HttpContext.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }*/
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

    }
}