

using Entities.Sold;

namespace Entities
{
    public class Costume
    {
        public Guid CostumeID { get; set; }
        public string? CostumeName { get; set; }
        public string? Gender { get; set; }
        public string? Size { get; set; }
        public string? Age { get; set; }
        public double? PurchasePrice { get; set; }

        public DateTime? EntryDate { get; set; }
        public DateTime? ExitDate { get; set;}

        public SoldCostume ToSoldCostume()
        {
            return new SoldCostume()
            {
                SoldCostumeID = CostumeID,
                CostumeName = CostumeName,
                Gender = Gender,
                Size = Size,
                Age = Age,
                PurchasePrice = PurchasePrice,
                EntryDate = EntryDate,
                ExitDate = ExitDate,
            };
        }
    }
}

