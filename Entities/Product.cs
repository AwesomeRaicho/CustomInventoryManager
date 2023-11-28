using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Sold;

namespace Entities
{
    public class Product
    {
        public Guid ProductID { get; set; }
        public string? ProductType { get; set; }
        public string? ProductDescription { get; set; }
        public string? Color { get; set; }
        public string? Theme { get; set; }
        public string? Gender { get; set; }
        public string? Size { get; set; }
        public double? PurchasePrice { get; set; }
        public DateTime? EntryDate { get; set; }
        public DateTime? ExitDate { get; set; }

        public SoldProduct ToSoldProduct()
        {
            return new SoldProduct()
            {
                SoldProductID = ProductID,
                ProductType = ProductType,
                ProductDescription = ProductDescription,
                Color = Color,
                Theme = Theme,
                Gender = Gender,
                Size = Size,
                PurchasePrice = PurchasePrice,
                EntryDate = EntryDate,
                ExitDate = DateTime.Now
            };
        }
    }

}
