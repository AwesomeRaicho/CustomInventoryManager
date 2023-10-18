using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace ServicesContracts.DTO
{
    public class ClothesUpdateRequest
    {
        public Guid ClothesID { get; set; }
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
            return new Clothes
            {
                ClothesID = ClothesID,
                Theme = Theme,
                Model = Model,
                Gender = Gender,
                Size = Size,
                PurchasePrice = PurchasePrice,
                EntryDate = EntryDate,
                ExitDate = ExitDate,
                ClothesType = ClothesType,
            };
        }
    }
}
