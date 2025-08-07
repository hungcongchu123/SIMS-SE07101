using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebSIMS.Controllers
{
    public class AuthController : Controller
    {
        [AllowAnonymous] // ai cung co the truy cap dc
        public IActionResult AccessDenied()
        {

            return View();
        }
    }
}
