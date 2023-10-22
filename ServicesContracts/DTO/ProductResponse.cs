using ServicesContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace ServicesContracts.DTO
{
    public class ProductResponse
    {
        public Guid ProductID { get; set; }
        public string? ProductType { get; set; }
        public string? ProductDescription { get; set; }
        public string? Color { get; set;}
        public string? Theme { get; set; }
        public string? Gender { get; set; }
        public string? Size { get; set; }
        public double? PurchasePrice { get; set; }
        public DateTime? EntryDate { get; set; }
        public DateTime? ExitTime { get; set; }

        public override string ToString()
        {
            return $"Type:{ProductType}, Desc:{ProductDescription}, color:{Color}, Theme:{Theme}, Gender:{Gender}, Size:{Size}, Price {PurchasePrice}";
        }

        public ProductUpdateRequest ToProductUpdateRequest()
        {
            return new ProductUpdateRequest()
            {
                ProductID = ProductID,
                Color = Color,
                ProductDescription = ProductDescription,
                ProductType = Enum.TryParse(this.ProductType, true, out ProductTypeOptions type) ? type : throw new ArgumentException("product type on pruduct update request is not correct"),
                Gender = Enum.TryParse(this.Gender, true, out GenderOptions gender) ? gender : null,
                Size = Size,
                Theme = Enum.TryParse(this.Theme, true, out ThemeOptions theme) ? theme : null,
                PurchasePrice = PurchasePrice,
                EntryDate = EntryDate,
                ExitDate = ExitTime,
            };
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if(obj.GetType() != typeof(ProductResponse)) return false;

            ProductResponse other = (ProductResponse)obj;

            return
                ProductID == other.ProductID &&
                ProductType == other.ProductType &&
                ProductDescription == other.ProductDescription &&
                Color == other.Color &&
                Theme == other.Theme &&
                Gender == other.Gender &&
                Size == other.Size &&
                EntryDate == other.EntryDate;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }

    public static class ProductExtention
    {
        public static ProductResponse ToProductResponse(this Product product)
        {
            return new ProductResponse()
            {
                ProductID = product.ProductID,
                ProductType = product.ProductType,
                ProductDescription = product.ProductDescription,
                Color = product.Color,
                Theme = product.Theme,
                Gender = product.Gender,
                Size = product.Size,
                PurchasePrice = product.PurchasePrice,
                EntryDate = product.EntryDate,
                ExitTime = product.ExitDate,                
            };
        }
    }
}
