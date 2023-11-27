using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public async Task<IActionResult>  Clothes(string filterBy, string? searchString, string orderBy = nameof(ClothesResponse.Model), SortOrderOptions sortOrder = SortOrderOptions.ASC)
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

            List<ClothesResponse> clothes = await _clothesService.GetFilteredClothes(filterBy, searchString);

            List<ClothesResponse> orderedClothes = await _clothesService.GetSortedClothes(clothes, orderBy, sortOrder);

            return View(orderedClothes);
        }

        [Route("clothes/create")]
        [HttpGet]
        public IActionResult  CreateClothes()
        {

            return View();
        }

        [Route("clothes/create")]
        [HttpPost]
        public async Task<IActionResult> CreateClothes(ClothesAddRequest clothesAddRequest)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return View();
            }

            await _clothesService.AddClothes(clothesAddRequest);
            return RedirectToAction("Clothes");
        }



        [Route("clothes/edit")]
        [HttpGet]
        public async Task<IActionResult>  Edit(Guid clothesID)
        {
            ClothesResponse? clothes = await _clothesService.GetClothesByClothesID(clothesID);

            if(clothes == null)
            {
                return RedirectToAction("clothes");
            }

            
            return View(clothes.ToClothesUpdateRequest());
        }


        [Route("clothes/edit")]
        [HttpPost]
        public async Task<IActionResult>  Edit(ClothesUpdateRequest clothesUpdateRequest)
        {
            //NEED TO RETURN ERRORS IF MODEL IS NOT CORRECT
            if(!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e=> e.ErrorMessage).ToList();

                return View(clothesUpdateRequest);
            }



            await _clothesService.UpdateClothes(clothesUpdateRequest);

            return RedirectToAction("clothes");
        }

        [Route("clothes/delete")]
        [HttpGet]
        public async Task<IActionResult>  Delete(Guid clothesID)
        {
            ClothesResponse? response = await _clothesService.GetClothesByClothesID(clothesID);

            if(response == null)
            {
                return RedirectToAction("clothes");
            }

            return View(response);
        }


        [Route("clothes/delete")]
        [HttpPost]
        public async Task<IActionResult>  Delete(ClothesResponse clothesResponse)
        {

            await _clothesService.DeleteClothes(clothesResponse.ClothesID);

            return RedirectToAction("clothes");
        }
    }
}
