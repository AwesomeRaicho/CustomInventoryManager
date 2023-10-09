

namespace Entities
{
    public class Costume
    {
        public Guid CostumeID { get; set; }
        public string? CostumeName { get; set; }
        public string? Gender { get; set; }
        public int? Size { get; set; }
        public int? Age { get; set; }
        public double? PurchasePrice { get; set; }
        public DateTime? EntryDate { get; set; }
        public DateTime? ExitDate { get; set;}
    }
}

