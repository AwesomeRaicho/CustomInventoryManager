using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace ServiceContracts
{
    public class CostumeResponse
    {
        public Guid CostumeID { get; set; }
        public string? CostumeName { get; set; }
        public string? Gender { get; set; }
        public int? Size { get; set; }
        public int? Age { get; set; }
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
