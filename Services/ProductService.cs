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
using Entities.Sold;

namespace Services
{
    public class ProductService : IProductsServices
    {
        //private fields 
        private readonly IRepository<Product> _repo;
        private readonly IRepository<SoldProduct> _soldRepo;

        //constructor
        public ProductService(IRepository<Product> repository, IRepository<SoldProduct> soldRepo)
        {
            _repo = repository;
            _soldRepo = soldRepo;

        }



        public async Task<ProductResponse> AddProduct(ProductAddRequest? productAddRequest)
        {
            if(productAddRequest == null) throw new ArgumentNullException(nameof(productAddRequest));

            ValidationHelper.ModelValidation(productAddRequest);

            Product product = productAddRequest.ToProduct();
            product.ProductID = Guid.NewGuid();
            product.EntryDate = DateTime.Now;

            await _repo.Add(product);

            return product.ToProductResponse();
        }

        public async Task<bool> DeleteProduct(Guid? productID)
        {
            if(productID == null) throw new ArgumentNullException(nameof(productID));

            Product? prod = await _repo.GetById((Guid)productID);

            if (prod == null) return false;

            await _repo.Delete(prod);

            return true;
        }

        public async Task<List<ProductResponse>>  GetAllProducts()
        {
            IEnumerable<Product> products = await _repo.GetAll(1,100);

            List<ProductResponse> toReturn = new List<ProductResponse>();

            foreach (Product product in products)
            {
                toReturn.Add(product.ToProductResponse());
            }

            return toReturn;
        }

        public async Task<List<ProductResponse>>  GetAllSoldProducts()
        {
            List<ProductResponse> toReturn = new List<ProductResponse>();
            IEnumerable<SoldProduct> soldProducts = await _soldRepo.GetAll(1,100);

            foreach (SoldProduct product in soldProducts)
            {
                toReturn.Add(product.ToProduct().ToProductResponse());
            }

            return toReturn;

        }

        public async Task<bool>  SoldProductByProductID(Guid? guid)
        {
            if(guid == null) throw new ArgumentNullException(nameof(guid));

            Product? toRemove = await _repo.GetById((Guid)guid);

            if (toRemove == null) return false;

            toRemove.ExitDate = DateTime.Now;

            await _repo.Delete(toRemove);
            await _soldRepo.Add(toRemove.ToSoldProduct());

            return true;
        }

        public async Task<List<ProductResponse>> GetFilteredProduct(string filterBy, string? filterString)
        {
            List<ProductResponse> allProducts = await GetAllProducts();
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

        public async Task<ProductResponse?> GetProductByProductID(Guid? productID)
        {
            if (productID == null) throw new ArgumentNullException(nameof(productID));
            Product? response = await _repo.GetById((Guid)productID);

            if (response == null) return null;

            return response.ToProductResponse();
        }

        public async Task<List<ProductResponse>> GetSortedProducts(List<ProductResponse> allProducts, string sortBy, SortOrderOptions sortOrder)
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

        public async Task<ProductResponse> UpdateProduct(ProductUpdateRequest? productUpdateRequest)
        {
            if(productUpdateRequest == null) throw new ArgumentNullException(nameof(productUpdateRequest));

            ValidationHelper.ModelValidation(productUpdateRequest);

            Product? product = await _repo.GetById(productUpdateRequest.ProductID);

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

            await _repo.Update(product);

            return product.ToProductResponse();
        }
    }
}
