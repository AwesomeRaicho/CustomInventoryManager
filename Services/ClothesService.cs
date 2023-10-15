using ServicesContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using ServicesContracts.DTO;
using ServicesContracts.Enums;
using Services.Helpers;

namespace Services
{
    public class ClothesService : IClothesService
    {
        //Private Fields
        private readonly List<Clothes> _clothes;
        private readonly List<Clothes> _soldClothes;

        //Contructor
        public ClothesService() 
        { 
            _clothes = new List<Clothes>();
            _soldClothes = new List<Clothes>();
        }

        public ClothesResponse AddClothes(ClothesAddRequest? clothesAddRequest)
        {
            if(clothesAddRequest == null) throw new ArgumentNullException(nameof(clothesAddRequest));

            ValidationHelper.ModelValidation(clothesAddRequest);

            Clothes clothes = clothesAddRequest.ToClothes();
            clothes.ClothesID = Guid.NewGuid();
            clothes.EntryDate = DateTime.Now;
            _clothes.Add(clothes);

            return clothes.ToClothesResponse();


        }
        public ClothesResponse? GetClothesByClothesID(Guid? guid)
        {
            if (guid == null) throw new ArgumentNullException();

            Clothes? clothes = _clothes.FirstOrDefault(temp => temp.ClothesID == guid);
            
            if (clothes == null) return null;

            return clothes.ToClothesResponse();

        }
        public List<ClothesResponse> GetAllClothes()
        {
            return _clothes.Select(temp => temp.ToClothesResponse()).ToList();
        }


        public bool DeleteClothes(Guid? guid)
        {
            if(guid == null) throw new ArgumentNullException(nameof(guid));

            Clothes? toRemove = _clothes.FirstOrDefault(temp => temp.ClothesID == guid);
            
            if(toRemove == null) return false;

            return _clothes.Remove(toRemove);
        }

        public bool SoldClothesByClothesID(Guid? guid)
        {
            if(guid == null) throw new ArgumentNullException(nameof(guid));

            Clothes? clothes = _clothes.FirstOrDefault(temp => temp.ClothesID == guid);

            if (clothes == null) return false;

            clothes.ExitDate = DateTime.Now;
            _soldClothes.Add(clothes);

            return _clothes.Remove(clothes);

        }

        public List<ClothesResponse> GetAllSoldClothes()
        {
            return _soldClothes.Select(temp => temp.ToClothesResponse()).ToList();
        }


        public List<ClothesResponse> GetFilteredClothes(string filterBy, string? filterString)
        {
            throw new NotImplementedException();
        }

        public List<ClothesResponse> GetSortedClothes(List<ClothesResponse>? allClothes, string sortBy, SortOrderOptions sortOrder)
        {
            throw new NotImplementedException();
        }


        public ClothesResponse UpdateClothes(ClothesUpdateRequest? clothesUpdateRequest)
        {
            throw new NotImplementedException();
        }
    }
}
