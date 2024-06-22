using System.Security.Claims;
using System.Text;
using MagicVilla.Utility;
using MagicVilla.Web.Models;
using MagicVilla.Web.Models.Dto;
using MagicVilla.Web.Services.IService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto obj = new LoginRequestDto();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDto obj)
        {
            var response = await _authService.LoginAsync<ApiResponse>(obj);
            if (response != null && response.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.Result) ?? string.Empty);
                
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, model.User.Name));
                identity.AddClaim(new Claim(ClaimTypes.Role, model.User.Role));
                var principle = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle);
                HttpContext.Session.SetString(StaticDetails.SessionToken, model.Token);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("CustomError", response.ErrorMessage.FirstOrDefault());
            return View(obj);
        }

        [HttpGet]
        public async Task< IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationRequestDto obj)
        {
           var result =  await _authService.RegistrationAsync<ApiResponse>(obj);
           if (result != null && result.IsSuccess)
           {
               return RedirectToAction("Login");
           }
           return View(obj);
        }

        public async Task<IActionResult> LogOut( )
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(StaticDetails.SessionToken, "");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private  void CheckSession()
        {
            if (HttpContext.Session.TryGetValue(StaticDetails.SessionToken, out byte[] value))
            {
                // Session is set
                var token = Encoding.UTF8.GetString(value);
                // Do something with the token
            }
            else
            {
                // Session is not set
            }
        }
    }
}
