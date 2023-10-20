using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesContracts.Enums;

namespace ServicesContracts.DTO
{
    public class ProductAddRequest
    {
        [Required(ErrorMessage = "Must add product type price")]
        public ProductTypeOptions ProductType { get; set; }
        public string? ProductDescription { get; set; }
        public string? Color { get; set;}
        [Required(ErrorMessage = "Must add theme price")]
        public ThemeOptions? Theme { get; set; }
        public GenderOptions? Gender { get; set; }
        public string? Size { get; set; }
        [Required(ErrorMessage = "Must add purchase price")]
        public double? PurchasePrice { get; set; }
    }
}
