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
        private readonly List<Product> _productsList;
        private readonly List<Product> _soldProductsList;


        //constructor
        public ProductService()
        {
            _productsList = new List<Product>();
            _soldProductsList = new List<Product>();
        }



        public ProductResponse AddProduct(ProductAddRequest? productAddRequest)
        {
            if(productAddRequest == null) throw new ArgumentNullException(nameof(productAddRequest));

            ValidationHelper.ModelValidation(productAddRequest);

            Product product = productAddRequest.ToProduct();
            product.ProductID = Guid.NewGuid();
            product.EntryDate = DateTime.Now;

            _productsList.Add(product);

            return product.ToProductResponse();
        }

        public bool DeleteProduct(Guid? productID)
        {
            if(productID == null) throw new ArgumentNullException(nameof(productID));

            Product? prod = _productsList.FirstOrDefault(temp => temp.ProductID == productID);

            if (prod == null) return false;

            return _productsList.Remove(prod);
        }

        public List<ProductResponse> GetAllProducts()
        {
            return _productsList.Select(temp => temp.ToProductResponse()).ToList();
        }

        public List<ProductResponse> GetAllSoldProducts()
        {
            return _soldProductsList.Select(temp => temp.ToProductResponse()).ToList();
        }

        public bool SoldProductByProductID(Guid? guid)
        {
            if (guid == null) throw new ArgumentNullException();

            Product? product = _productsList.FirstOrDefault(temp => temp.ProductID == guid);

            if (product == null) return false;

            _productsList.Remove(product);
            product.ExitDate = DateTime.Now;

            _soldProductsList.Add(product);

            return true;

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
            Product? response = _productsList.FirstOrDefault(temp => temp.ProductID == productID);

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

            Product? product = _productsList.FirstOrDefault(temp => temp.ProductID == productUpdateRequest.ProductID);

            if (product == null) throw new ArgumentException("ID not found in DB");

            //update product with update request
            product.ProductType = productUpdateRequest.ProductType.ToString();
            product.ProductDescription = productUpdateRequest.ProductDescription;
            product.Color = productUpdateRequest.Color;
            product.Theme = productUpdateRequest.Theme.ToString();
            product.Gender = productUpdateRequest.Gender.ToString();
            product.Size = productUpdateRequest.Size;
            product.PurchasePrice = productUpdateRequest.PurchasePrice;
            product.EntryDate = productUpdateRequest.EntryDate;
            product.ExitDate = productUpdateRequest.ExitDate;


            return product.ToProductResponse();
        }
    }
}
