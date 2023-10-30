using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services;
using ServicesContracts;
using ServicesContracts.Enums;

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
        [Route("Costumes")]
        public IActionResult Costumes(string filterBy, string? searchString, string orderBy = nameof(CostumeResponse.CostumeName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {

            ViewBag.CurrentFilterBy = filterBy;
            ViewBag.CurrentSearchString = searchString;
            ViewBag.CurrentOrderBy = orderBy.ToString();
            ViewBag.CurrentSortOrder = sortOrder.ToString();


            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                {nameof(CostumeResponse.CostumeName), "Costume Name" },
                {nameof(CostumeResponse.Gender), "Gender" },
                {nameof(CostumeResponse.Size), "Size" },
                {nameof(CostumeResponse.Age), "Age" },

            };

            List<CostumeResponse> costumes = _costumeService.GetFilteredCostumes(filterBy, searchString);

            //sort 
            List<CostumeResponse> sortedCostumes = _costumeService.GetSortedCostumes(costumes, orderBy, sortOrder);

            return View(sortedCostumes);
        }

        [Route("costumes/create")]
        [HttpGet]
        public IActionResult CreateCostume()
        {

            return View();
        }

        [Route("costumes/create")]
        [HttpPost]
        public IActionResult CreateCostume(CostumeAddRequest costumeAddRequest)
        {

            if(!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                
                return View();


            }

            _costumeService.AddCostume(costumeAddRequest);

            return RedirectToAction("Costumes", "Costume");
        }
    }
}
