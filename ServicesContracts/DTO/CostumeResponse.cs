using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using ServicesContracts.DTO;
using ServicesContracts.Enums;


namespace ServicesContracts
{
    public class CostumeResponse
    {
        public Guid CostumeID { get; set; }
        public string? CostumeName { get; set; }
        public string? Gender { get; set; }
        public string? Size { get; set; }
        public string? Age { get; set; }
        public double? PurchasePrice { get; set; }
        public DateTime? EntryDate { get; set; }
        public DateTime? ExitDate { get; set; }


        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            
            if(obj.GetType() != typeof(CostumeResponse)) return false;

            CostumeResponse costume_to_compare = (CostumeResponse)obj;

            return
                this.CostumeName == costume_to_compare.CostumeName &&
                this.CostumeID == costume_to_compare.CostumeID &&
                this.EntryDate == costume_to_compare.EntryDate;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public CostumeUpdateRequest GetCostumeUpdateRequest()
        {
            return new CostumeUpdateRequest()
            {
                CostumeName = CostumeName,
                Age = Age,
                PurchasePrice = PurchasePrice,
                Size = Size,
                CostumeID = CostumeID,
                Gender = Enum.TryParse(this.Gender, true, out GenderOptions gender) ? gender : throw new ArgumentException("Incorrect gender for costume update request"),
                EntryDate = EntryDate,
                ExitDate = ExitDate,
            };
        }
    }

    public static class CostumeExtensions
    {
        public static CostumeResponse ToCostumeResponse(this Costume costume)
        {
            return new CostumeResponse()
            {
                CostumeID = costume.CostumeID,
                CostumeName = costume.CostumeName,
                Gender = costume.Gender,
                Size = costume.Size,
                Age = costume.Age,
                PurchasePrice = costume.PurchasePrice,
                EntryDate = costume.EntryDate,
                ExitDate = costume.ExitDate,
            };
        }
    }
}
