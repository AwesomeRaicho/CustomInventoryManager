using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Sold
{
    public class SoldCostume
    {
        public Guid SoldCostumeID { get; set; }
        public string? CostumeName { get; set; }
        public string? Gender { get; set; }
        public string? Size { get; set; }
        public string? Age { get; set; }
        public double? PurchasePrice { get; set; }

        public DateTime? EntryDate { get; set; }
        public DateTime? ExitDate { get; set; }

        public Costume ToCostume()
        {
            return new Costume()
            {
                CostumeID = SoldCostumeID,
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
