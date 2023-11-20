using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Sold
{
    public class SoldClothes
    {
        [Key]
        public Guid SoldClothesID { get; set; }
        public string? Theme { get; set; }
        public string? ClothesType { get; set; }
        public string? Model { get; set; }
        public string? Gender { get; set; }
        public string? Size { get; set; }
        public double? PurchasePrice { get; set; }
        public DateTime? EntryDate { get; set; }
        public DateTime? ExitDate { get; set; }


        public Clothes ToClothes()
        {
            return new Clothes() 
            {
                ClothesID = SoldClothesID,
                Theme = Theme,
                ClothesType = ClothesType,
                Model = Model,
                Gender = Gender,
                Size = Size,
                PurchasePrice = PurchasePrice,
                EntryDate = EntryDate,
                ExitDate = ExitDate,

            
            };
        }


    }
}
