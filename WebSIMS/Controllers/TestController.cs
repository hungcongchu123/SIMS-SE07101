using Microsoft.AspNetCore.Mvc;

namespace WebSIMS.Controllers
{
    public class TestController : Controller
    {
        public string About(int id , string name)
        {
            return "Test web - demo ; ID = " + id + ", Name = " + name;
            // test/about?id=10&name=SE07101
            // tren url : test => ten cua controller
            // tren url : about => method cua controller Test
            // ?id - &name : tham so tren url
        }
        public IActionResult Index()
        {
            return View(); // lam viec voi thu muc View
            // ben trong thu muc Views , tao 1 thu muc trung ten cua controller
            // ben trong thu muc trung ten controller , tao 1 file trung voi ten ham trong controllet

        }
    }
}
