using Microsoft.AspNetCore.Mvc;
using ServicesContracts;
using ServicesContracts.DTO;
using ServicesContracts.Enums;

namespace CustomStoreDB.Controllers
{
    public class ProductController : Controller
    {
        //private fields
        private readonly IProductsServices _productsServices;

        //constructor
        public ProductController(IProductsServices productsServices)
        {
            _productsServices = productsServices;
        }

        [Route("products")]
        public IActionResult Products(string filterBy, string? SearchString, string orderBy = nameof(ProductResponse.ProductType), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            ViewBag.CurrentFilterBy = filterBy;
            ViewBag.CurrentSearchString = SearchString;
            ViewBag.CurrentOrderBy = orderBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString();

            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                {nameof(ProductResponse.ProductType), "Type" },
                {nameof(ProductResponse.ProductDescription), "Description" },
                {nameof(ProductResponse.Color), "Color" },
                {nameof(ProductResponse.Theme), "Theme" },
                {nameof(ProductResponse.Gender), "Gender" },
                {nameof(ProductResponse.Size), "Size" },

            };

            List<ProductResponse> products = _productsServices.GetFilteredProduct(filterBy, SearchString);

            List<ProductResponse> sortedProducts = _productsServices.GetSortedProducts(products, orderBy, sortOrder);

            return View(sortedProducts);
        }
    }
}
