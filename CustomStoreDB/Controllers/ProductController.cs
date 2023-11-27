using Microsoft.AspNetCore.Mvc;
using Services;
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
        public async Task<IActionResult> Products(string filterBy, string? SearchString, string orderBy = nameof(ProductResponse.ProductType), SortOrderOptions sortOrder = SortOrderOptions.ASC)
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

            List<ProductResponse> products = await _productsServices.GetFilteredProduct(filterBy, SearchString);

            List<ProductResponse> sortedProducts = await _productsServices.GetSortedProducts(products, orderBy, sortOrder);

            return View(sortedProducts);
        }

        [Route("products/create")]
        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }

        [Route("products/create")]
        [HttpPost]
        public IActionResult CreateProduct(ProductAddRequest productAddRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return View();
            }


            _productsServices.AddProduct(productAddRequest);

            return RedirectToAction("Products", "Product");
        }

        [Route("products/edit")]
        [HttpGet]
        public async Task<IActionResult> EditProduct(Guid productID)
        {
            ProductResponse? response = await _productsServices.GetProductByProductID(productID);

            if (response == null) return RedirectToAction("products");


            return View(response.ToProductUpdateRequest());
        }


        [Route("products/edit")]
        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductUpdateRequest productUpdateRequest)
        {
            ProductResponse? response = await _productsServices.GetProductByProductID(productUpdateRequest.ProductID);

            if (response == null) return RedirectToAction("products");

            await _productsServices.UpdateProduct(productUpdateRequest);

            return RedirectToAction("products");
        }


        // create Delete
        [Route("products/delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteProduct(Guid productID)
        {
            ProductResponse? response = await _productsServices.GetProductByProductID(productID);

            if (response == null) return RedirectToAction("product");



            return View(response.ToProductUpdateRequest());
        }

        [Route("products/delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(ProductUpdateRequest productUpdateRequest)
        {
            ProductResponse? response = await _productsServices.GetProductByProductID(productUpdateRequest.ProductID);

            if(response == null) return RedirectToAction("products");

            await _productsServices.DeleteProduct(productUpdateRequest.ProductID);

            return RedirectToAction("products");

        }

        //sell a product
        [Route("products/sell")]
        [HttpGet]
        public async Task<IActionResult> SellProduct(Guid productID)
        {
            ProductResponse? response = await _productsServices.GetProductByProductID(productID);

            if (response == null) return RedirectToAction("products");

            

            return View(response);
        }

        [Route("products/sell")]
        [HttpPost]
        public async Task<IActionResult> SellProduct(ProductResponse productResponse)
        {
            ProductResponse? response = await _productsServices.GetProductByProductID(productResponse.ProductID);

            if (response == null) return RedirectToAction("products");

            await _productsServices.SoldProductByProductID(productResponse.ProductID);

            return RedirectToAction("products");
        }
    }
}
