using ServicesContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace ServicesContracts.DTO
{
    public class ClothesAddRequest
    {
        [Required(ErrorMessage = "Clothes type must be provided")]
        public ClothesTypeOptions ClothesType { get; set; }
        
        [Required(ErrorMessage = "Theme must be provided")]
        public ThemeOptions? Theme { get; set; }
        
        [Required(ErrorMessage = "Model must be provided")]
        public string? Model { get; set; }
        
        [Required(ErrorMessage = "Gender must be provided")]
        public GenderOptions? Gender { get; set; }
        
        [Required(ErrorMessage = "Size must be provided")]
        public string? Size { get; set; }
        
        [Required(ErrorMessage = "Purchase price must be must be provided")]
        public double? PurchasePrice { get; set; }

        public Clothes ToClothes()
        {
            return new Clothes()
            {
                ClothesType = ClothesType.ToString(),
                Theme = Theme.ToString(),
                Model = Model,
                Gender = Gender.ToString(),
                Size = Size,
                PurchasePrice = PurchasePrice,
            };
        }
    }
}
