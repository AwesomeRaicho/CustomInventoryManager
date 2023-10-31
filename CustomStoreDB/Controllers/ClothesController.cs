using Microsoft.AspNetCore.Mvc;
using ServicesContracts;
using ServicesContracts.DTO;
using ServicesContracts.Enums;

namespace CustomStoreDB.Controllers
{
    public class ClothesController : Controller
    {
        //private fields
        private readonly IClothesService _clothesService;

        //constructor
        public ClothesController(IClothesService clothesService)
        {
            _clothesService = clothesService;
        }


        [Route("clothes")]
        public IActionResult Clothes(string filterBy, string? searchString, string orderBy = nameof(ClothesResponse.Model), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            ViewBag.CurrentFilterBy = filterBy;
            ViewBag.CurrentSearchString = searchString;
            ViewBag.CurrentOrderBy = orderBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString();

            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                {nameof(ClothesResponse.Theme), "Theme" },
                {nameof(ClothesResponse.ClothesType), "Clothes Type" },
                {nameof(ClothesResponse.Model), "Model" },
                {nameof(ClothesResponse.Gender), "Gender" },
                {nameof(ClothesResponse.Size), "Size" }
            };

            List<ClothesResponse> clothes = _clothesService.GetFilteredClothes(filterBy, searchString);

            List<ClothesResponse> orderedClothes = _clothesService.GetSortedClothes(clothes, orderBy, sortOrder);

            return View(orderedClothes);
        }

        [Route("clothes/create")]
        [HttpGet]
        public IActionResult CreateClothes()
        {

            return View();
        }

        [Route("clothes/create")]
        [HttpPost]
        public IActionResult CreateClothes(ClothesAddRequest clothesAddRequest)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return View();
            }

            _clothesService.AddClothes(clothesAddRequest);
            return RedirectToAction("Clothes");
        }
    }
}
