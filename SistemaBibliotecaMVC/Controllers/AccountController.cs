using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SistemaBibliotecaMVC.Models;
using System.Threading.Tasks;

namespace SistemaBibliotecaMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.NombreUsuario, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Nuevo usuario registrado exitosamente: {Usuario}.", model.NombreUsuario);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.UsuarioOEmail)
                           ?? await _userManager.FindByNameAsync(model.UsuarioOEmail);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName!, model.Password, model.Recordarme, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Inicio de sesión exitoso para el usuario {Usuario}.", user.UserName);
                        return LocalRedirect(returnUrl);
                    }
                }

                _logger.LogWarning("Intento de inicio de sesión fallido para {UsuarioOEmail}.", model.UsuarioOEmail);
                ModelState.AddModelError(string.Empty, "Credenciales de acceso no válidas.");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("El usuario {Usuario} cerró su sesión.", User.Identity?.Name);
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            _logger.LogWarning("Acceso denegado registrado para el usuario {Usuario} intentando ingresar a una ruta no autorizada.", User.Identity?.Name ?? "Anónimo");
            return View();
        }
    }
}