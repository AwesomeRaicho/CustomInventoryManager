using ServicesContracts;
using ServicesContracts.DTO;
using ServicesContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService : IProductsServices
    {
        public ProductResponse AddProduct(ProductAddRequest? productAddRequest)
        {
            throw new NotImplementedException();
        }

        public bool DeleteProduct(Guid? productID)
        {
            throw new NotImplementedException();
        }

        public List<ProductResponse> GetAllProducts()
        {
            throw new NotImplementedException();
        }

        public List<ProductResponse> GetAllSoldProducts()
        {
            throw new NotImplementedException();
        }

        public List<ProductResponse> GetFilteredProduct(string filterBy, string? filterString)
        {
            throw new NotImplementedException();
        }

        public ProductResponse? GetProductByProductID(Guid? productID)
        {
            throw new NotImplementedException();
        }

        public List<ProductResponse> GetSortedProducts(List<ProductResponse> allProducts, string sortBy, SortOrderOptions sortOrder)
        {
            throw new NotImplementedException();
        }

        public bool SoldProductByProductID(Guid? guid)
        {
            throw new NotImplementedException();
        }

        public ProductResponse UpdateProduct(ProductUpdateRequest? productUpdateRequest)
        {
            throw new NotImplementedException();
        }
    }
}
