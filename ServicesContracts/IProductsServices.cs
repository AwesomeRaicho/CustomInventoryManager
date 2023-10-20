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
        ProductResponse AddProduct(ProductAddRequest? productAddRequest);
        List<ProductResponse> GetAllProducts();
        ProductResponse? GetProductByProductID(Guid? productID);
        List<ProductResponse> GetAllSoldProducts();
        bool SoldProductByProductID(Guid? guid);
        bool DeleteProduct(Guid? productID);
        List<ProductResponse> GetFilteredProduct(string filterBy, string? filterString);
        List<ProductResponse> GetSortedProducts(List<ProductResponse> allProducts, string sortBy, SortOrderOptions sortOrder);
        ProductResponse UpdateProduct(ProductUpdateRequest? productUpdateRequest);
    }
}
