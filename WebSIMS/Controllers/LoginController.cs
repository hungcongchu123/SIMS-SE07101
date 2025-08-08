using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebSIMS.Models;
using WebSIMS.DBContext.Entities;
using WebSIMS.Services;

namespace WebSIMS.Controllers
{
    [Authorize] // muon truy cap vao controller nay thi phai dang nhap
    public class LoginController : Controller
    {
        private readonly UserService _userService;
        public LoginController(UserService service)
        {
            _userService = service;
        }

        [HttpGet]
        [AllowAnonymous] // khong bat phai dang nhap
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // khong co loi
                string username = model.Username.Trim();
                string password = model.Password.Trim();
                var user = await _userService.LoginUserAsync(username, password);
                if (user == null)
                {
                    ViewData["MessageLogin"] = "Account Invalid, please try again !";
                    return View(model);
                }
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    // ✅ Dòng code đã được thêm để lưu vai trò của người dùng
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("UserID", user.UserID.ToString()) // 🟢 thêm dòng này
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                return RedirectToAction("Index", "Dashboard");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            foreach (var item in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(item);
            }
            return RedirectToAction("Index", "Login");
        }

    }
}