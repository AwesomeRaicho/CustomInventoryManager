using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services;
using ServicesContracts;
using ServicesContracts.DTO;
using ServicesContracts.Enums;

namespace CustomStoreDB.Controllers
{
    public class CostumeController : Controller
    {
        //private fields
        private readonly ICostumeService _costumeService;

        //constructor
        public CostumeController(ICostumeService costumeService)
        {
            _costumeService = costumeService;
        }


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

        [Route("costumes/edit")]
        [HttpGet]
        public IActionResult EditCostume(Guid costumeID)
        {
            CostumeResponse? response = _costumeService.GetCostumeByCostumeID(costumeID);

            if(response == null)
            {
                return RedirectToAction("costumes");
            }

            return View(response.GetCostumeUpdateRequest());
        }

        [Route("costumes/edit")]
        [HttpPost]
        public IActionResult EditCostume(CostumeUpdateRequest costumeUpdateRequest)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return View(costumeUpdateRequest);
            }

            //ViewBag.Errors = ModelState.SelectMany(m => m.)

            _costumeService.UpdateCostume(costumeUpdateRequest);

            return RedirectToAction("costumes");
        }

        [Route("costumes/delete")]
        [HttpGet]
        public IActionResult DeleteCostume(Guid costumeID)
        {
            CostumeResponse? costumeResponse = _costumeService.GetCostumeByCostumeID(costumeID);

            if (costumeResponse == null) return RedirectToAction("costumes");



            return View(costumeResponse.GetCostumeUpdateRequest());
        }

        [Route("costumes/delete")]
        [HttpPost]
        public IActionResult DeleteCostume(CostumeUpdateRequest costumeUpdateRequest)
        {
            CostumeResponse? response = _costumeService.GetCostumeByCostumeID(costumeUpdateRequest.CostumeID);


            if (response == null) return RedirectToAction("costumes");

            _costumeService.DeleteCostume(costumeUpdateRequest.CostumeID);

            return RedirectToAction("costumes");
        }




    }
}
