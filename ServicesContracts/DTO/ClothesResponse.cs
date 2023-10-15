using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesContracts.DTO
{
    public class ClothesResponse
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


        /// <summary>
        /// makes sure to compare equiality by value andf not by reference
        /// </summary>
        /// <param name="obj">the ClothesResponse object to compare</param>
        /// <returns>True or False depending opn if the values match</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if(obj.GetType() != typeof(ClothesResponse)) return false; 

            ClothesResponse other = (ClothesResponse)obj;

            return 
                ClothesID == ClothesID && 
                Theme == other.Theme && 
                Model == other.Model && 
                Gender == other.Gender && 
                Size == other.Size && 
                PurchasePrice == other.PurchasePrice && 
                EntryDate == other.EntryDate && 
                ExitDate == other.ExitDate;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }

    public static class ClothesExtension
    {
        public static ClothesResponse ToClothesResponse(this Clothes clothes)
        {
            return new ClothesResponse
            {
                ClothesID = clothes.ClothesID,
            };
        }
    }
}
