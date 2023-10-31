using Microsoft.AspNetCore.Mvc;

namespace CustomStoreDB.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {

            return View();
        }
    }
}
