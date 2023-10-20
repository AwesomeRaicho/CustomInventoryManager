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

        public ProductUpdateRequest ToProductUpdateRequest()
        {
            return new ProductUpdateRequest()
            {
                ProductID = ProductID,
                Color = Color,
                ProductDescription = ProductDescription,
                ProductType = (ProductTypeOptions)Enum.Parse(typeof(ProductTypeOptions), ProductType, true),
                Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true),
                Size = Size,
                Theme = (ThemeOptions)Enum.Parse(typeof(ThemeOptions), Theme, true),
                PurchasePrice = PurchasePrice,
                EntryDate = EntryDate,
                ExitDate = ExitTime,
            };
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if(obj.GetType() != typeof(ProductResponse)) return false;

            return
                ProductID == ProductID &&
                ProductType == ProductType &&
                ProductDescription == ProductDescription &&
                Color == Color &&
                Theme == Theme &&
                Gender == Gender &&
                Size == Size &&
                EntryDate == EntryDate;
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
                EntryDate = product.EntryDate,
                ExitTime = product.ExitDate,                
            };
        }
    }
}
