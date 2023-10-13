using Entities;
using System.ComponentModel.DataAnnotations;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    public class CostumeAddRequest
    {
        [Required(ErrorMessage = "Costume name must be supplied")]
        public string? CostumeName { get; set; }
        [Required(ErrorMessage = "Gender must be supplied")]
        public GenderOptions Gender { get; set; }
        [Required(ErrorMessage = "Size must be supplied")]
        public string? Size { get; set; }
        [Required(ErrorMessage = "Age must be supplied")]
        public string? Age { get; set; }
        [Required(ErrorMessage = "Purchace price must be supplied")]
        public double? PurchasePrice { get; set; }

        public Costume ToCostume()
        {
            return new Costume
            {
                CostumeName = CostumeName,
                Age = Age,
                Size = Size,
                Gender = Gender.ToString(),
                PurchasePrice = PurchasePrice
            };
        }
    }
}