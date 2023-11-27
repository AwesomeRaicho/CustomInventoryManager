using ServicesContracts.DTO;
using ServicesContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesContracts
{
    public interface IProductsServices
    {
        Task<ProductResponse> AddProduct(ProductAddRequest? productAddRequest);
        Task<List<ProductResponse>>  GetAllProducts();
        Task<ProductResponse?> GetProductByProductID(Guid? productID);
        Task<List<ProductResponse>>  GetAllSoldProducts();
        Task<bool>  SoldProductByProductID(Guid? guid);
        Task<bool>  DeleteProduct(Guid? productID);
        Task<List<ProductResponse>>  GetFilteredProduct(string filterBy, string? filterString);
        Task<List<ProductResponse>>  GetSortedProducts(List<ProductResponse> allProducts, string sortBy, SortOrderOptions sortOrder);
        Task<ProductResponse>  UpdateProduct(ProductUpdateRequest? productUpdateRequest);
    }
}
