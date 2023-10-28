using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using Services;
using System.Security.Cryptography.X509Certificates;

namespace CustomStoreDB.Controllers
{
    public class CostumeController : Controller
    {
        //private fields
        private readonly ICostumeService _costumeService;

        //constructor
        public CostumeController()
        {
            _costumeService = new CostumeService();
        }

        [Route("/")]
        [Route("/Costumes")]
        public IActionResult Costumes()
        {


            return View();
        }
    }
}
