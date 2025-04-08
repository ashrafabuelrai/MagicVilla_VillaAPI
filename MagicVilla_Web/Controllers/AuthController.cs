using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace MagicVilla_Web.Controllers
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
            LoginRequestDTO obj = new LoginRequestDTO();
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO obj)
        {
            ApiResponse reslut = await _authService.LoginAsync<ApiResponse>(obj);
            if (reslut != null && reslut.IsSuccess)
            {
                LoginResponsDTO respons = JsonConvert.DeserializeObject<LoginResponsDTO>(Convert.ToString(reslut.Result));
                HttpContext.Session.SetString(SD.SessionToken, respons.Token);

                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(respons.Token);

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == "unique_name").Value));
                identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u=>u.Type=="role").Value));
                var princepal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, princepal);


                return RedirectToAction("Index","Home");
            }
            ModelState.AddModelError("CustomError", reslut.ErrorMessage.FirstOrDefault());
            return View(obj);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterationRequestDTO obj)
        {
            ApiResponse reslut=await _authService.RegisterAsync<ApiResponse>(obj);
            if (reslut != null && reslut.IsSuccess)
            {
            
                return RedirectToAction("Login");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(SD.SessionToken, "");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
