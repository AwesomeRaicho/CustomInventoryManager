using ServicesContracts;
using ServicesContracts.DTO;
using ServicesContracts.Enums;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Helpers;

namespace Services
{
    public class ProductService : IProductsServices
    {
        //private fields 
        private readonly IRepository<Product> _repository;

        //constructor
        public ProductService(IRepository<Product> repository)
        {
            _repository = repository;
            if (false)
            {
                ProductAddRequest request1 = new ProductAddRequest()
                {
                    ProductType = ProductTypeOptions.Arras,
                    Color = "Gold",
                    Gender = GenderOptions.Other,
                    Size = "3",
                    Theme = ThemeOptions.Wedding,
                    PurchasePrice = 100.00,
                    ProductDescription = "coins for wedding"
                };
                ProductAddRequest request2 = new ProductAddRequest()
                {
                    ProductType = ProductTypeOptions.Bow,
                    Color = "Red",
                    Gender = GenderOptions.Female,
                    Size = "small",
                    Theme = ThemeOptions.Other,
                    PurchasePrice = 30.00,
                    ProductDescription = "regular bow"
                };
                ProductAddRequest request3 = new ProductAddRequest()
                {
                    ProductType = ProductTypeOptions.Underwear,
                    Color = "white",
                    Gender = GenderOptions.Female,
                    Size = "medium",
                    Theme = ThemeOptions.Other,
                    PurchasePrice = 35.00,
                    ProductDescription = "calsones"
                };
                ProductAddRequest request4 = new ProductAddRequest()
                {
                    ProductType = ProductTypeOptions.Arras,
                    Color = "Gold",
                    Gender = GenderOptions.Other,
                    Size = "5",
                    Theme = ThemeOptions.Wedding,
                    PurchasePrice = 100.00,
                    ProductDescription = "coins for wedding"
                };
                ProductAddRequest request5 = new ProductAddRequest()
                {
                    ProductType = ProductTypeOptions.Valerina,
                    Color = "Gray",
                    Gender = GenderOptions.Female,
                    Size = "7",
                    Theme = ThemeOptions.Other,
                    PurchasePrice = 50,
                    ProductDescription = ""
                };
                ProductAddRequest request6 = new ProductAddRequest()
                {
                    ProductType = ProductTypeOptions.Candles,
                    Color = "blue",
                    Gender = GenderOptions.Other,
                    Size = "large",
                    Theme = ThemeOptions.Communion,
                    PurchasePrice = 15.00,
                    ProductDescription = "Velas"
                };

                this.AddProduct(request1);
                this.AddProduct(request2);
                this.AddProduct(request3);
                this.AddProduct(request4);
                this.AddProduct(request5);
                this.AddProduct(request6);
            }
        }



        public ProductResponse AddProduct(ProductAddRequest? productAddRequest)
        {
            if(productAddRequest == null) throw new ArgumentNullException(nameof(productAddRequest));

            ValidationHelper.ModelValidation(productAddRequest);

            Product product = productAddRequest.ToProduct();
            product.ProductID = Guid.NewGuid();
            product.EntryDate = DateTime.Now;

            _repository.Add(product);

            return product.ToProductResponse();
        }

        public bool DeleteProduct(Guid? productID)
        {
            if(productID == null) throw new ArgumentNullException(nameof(productID));

            Product? prod = _repository.GetById((Guid)productID);

            if (prod == null) return false;

            _repository.Delete(prod);

            return true;
        }

        public List<ProductResponse> GetAllProducts()
        {
            IEnumerable<Product> products = _repository.GetAll(1,100);

            List<ProductResponse> toReturn = new List<ProductResponse>();

            foreach (Product product in products)
            {
                toReturn.Add(product.ToProductResponse());
            }

            return toReturn;
        }

        public List<ProductResponse> GetAllSoldProducts()
        {
            //return _soldProductsList.Select(temp => temp.ToProductResponse()).ToList();
            return new List<ProductResponse>();
        }

        public bool SoldProductByProductID(Guid? guid)
        {
            //if (guid == null) throw new ArgumentNullException();

            //Product? product = _productsList.FirstOrDefault(temp => temp.ProductID == guid);

            //if (product == null) return false;

            //_productsList.Remove(product);
            //product.ExitDate = DateTime.Now;

            //_soldProductsList.Add(product);

            return false;

        }

        public List<ProductResponse> GetFilteredProduct(string filterBy, string? filterString)
        {
            List<ProductResponse> allProducts = GetAllProducts();
            List<ProductResponse> filteredProducts = allProducts;

            
            if(string.IsNullOrEmpty(filterBy) || string.IsNullOrEmpty(filterString)) return filteredProducts;


            switch(filterBy)
            {
                case nameof(Product.ProductType): 
                    filteredProducts = allProducts.Where(temp => (!string.IsNullOrEmpty(temp.ProductType))? temp.ProductType.Contains(filterString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(Product.ProductDescription):
                    filteredProducts = allProducts.Where(temp => (!string.IsNullOrEmpty(temp.ProductDescription)) ? temp.ProductDescription.Contains(filterString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(Product.Color):
                    filteredProducts = allProducts.Where(temp => (!string.IsNullOrEmpty(temp.Color)) ? temp.Color.Contains(filterString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(Product.Theme):
                    filteredProducts = allProducts.Where(temp => (!string.IsNullOrEmpty(temp.Theme)) ? temp.Theme.Contains(filterString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(Product.Gender):
                    filteredProducts = allProducts.Where(temp => (!string.IsNullOrEmpty(temp.Gender)) ? temp.Gender.Contains(filterString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(Product.Size):
                    filteredProducts = allProducts.Where(temp => (!string.IsNullOrEmpty(temp.Size)) ? temp.Size.Contains(filterString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(Product.EntryDate):
                    filteredProducts = allProducts.Where(temp => (temp.EntryDate != null) ? temp.EntryDate.Value.ToString("yyyy-MM-dd").Contains(filterString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(Product.ExitDate):
                    filteredProducts = allProducts.Where(temp => (temp.ExitTime != null) ? temp.ExitTime.Value.ToString("yyyy-MM-dd").Contains(filterString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                default: 
                    return filteredProducts;
                   
            }

            return filteredProducts;

        }

        public ProductResponse? GetProductByProductID(Guid? productID)
        {
            if (productID == null) throw new ArgumentNullException(nameof(productID));
            Product? response = _repository.GetById((Guid)productID);

            if (response == null) return null;

            return response.ToProductResponse();
        }

        public List<ProductResponse> GetSortedProducts(List<ProductResponse> allProducts, string sortBy, SortOrderOptions sortOrder)
        {
            if(sortBy == null) return allProducts;

            List<ProductResponse> sorted = (sortBy, sortOrder) switch
            {
                (nameof(Product.ProductType), SortOrderOptions.ASC) => allProducts.OrderBy(temp => temp.ProductType).ToList(),
                (nameof(Product.ProductType), SortOrderOptions.DESC) => allProducts.OrderByDescending(temp => temp.ProductType).ToList(),
                (nameof(Product.ProductDescription), SortOrderOptions.ASC) => allProducts.OrderBy(temp => temp.ProductDescription).ToList(),
                (nameof(Product.ProductDescription), SortOrderOptions.DESC) => allProducts.OrderByDescending(temp => temp.ProductDescription).ToList(),
                (nameof(Product.Color), SortOrderOptions.ASC) => allProducts.OrderBy(temp => temp.Color).ToList(),
                (nameof(Product.Color), SortOrderOptions.DESC) => allProducts.OrderByDescending(temp => temp.Color).ToList(),
                (nameof(Product.Theme), SortOrderOptions.ASC) => allProducts.OrderBy(temp => temp.Theme).ToList(),
                (nameof(Product.Theme), SortOrderOptions.DESC) => allProducts.OrderByDescending(temp => temp.Theme).ToList(),
                (nameof(Product.Gender), SortOrderOptions.ASC) => allProducts.OrderBy(temp => temp.Gender).ToList(),
                (nameof(Product.Gender), SortOrderOptions.DESC) => allProducts.OrderByDescending(temp => temp.Gender).ToList(),
                (nameof(Product.Size), SortOrderOptions.ASC) => allProducts.OrderBy(temp => temp.Size).ToList(),
                (nameof(Product.Size), SortOrderOptions.DESC) => allProducts.OrderByDescending(temp => temp.Size).ToList(),
                (nameof(Product.PurchasePrice), SortOrderOptions.ASC) => allProducts.OrderBy(temp => temp.PurchasePrice).ToList(),
                (nameof(Product.PurchasePrice), SortOrderOptions.DESC) => allProducts.OrderByDescending(temp => temp.PurchasePrice).ToList(),
                (nameof(Product.EntryDate), SortOrderOptions.ASC) => allProducts.OrderBy(temp => temp.EntryDate).ToList(),
                (nameof(Product.EntryDate), SortOrderOptions.DESC) => allProducts.OrderByDescending(temp => temp.EntryDate).ToList(),
                (nameof(Product.ExitDate), SortOrderOptions.ASC) => allProducts.OrderBy(temp => temp.ExitTime).ToList(),
                (nameof(Product.ExitDate), SortOrderOptions.DESC) => allProducts.OrderByDescending(temp => temp.ExitTime).ToList(),
                _ => allProducts
            };

            return sorted;
        }

        public ProductResponse UpdateProduct(ProductUpdateRequest? productUpdateRequest)
        {
            if(productUpdateRequest == null) throw new ArgumentNullException(nameof(productUpdateRequest));

            ValidationHelper.ModelValidation(productUpdateRequest);

            Product? product = _repository.GetById(productUpdateRequest.ProductID);

            if (product == null) throw new ArgumentException("ID not found in DB");

            product.ProductType = productUpdateRequest.ProductType.ToString();
            product.ProductDescription = productUpdateRequest.ProductDescription;
            product.Color = productUpdateRequest.Color;
            product.Theme = productUpdateRequest.Theme.ToString();
            product.Gender = productUpdateRequest.Gender.ToString();
            product.Size = productUpdateRequest.Size;
            product.PurchasePrice = productUpdateRequest.PurchasePrice;
            product.EntryDate = productUpdateRequest.EntryDate;
            product.ExitDate = productUpdateRequest.ExitDate;

            _repository.Update(product);

            return product.ToProductResponse();
        }
    }
}
