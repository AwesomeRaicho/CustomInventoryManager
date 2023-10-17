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
            List<ClothesResponse> allClothes = GetAllClothes();
            List<ClothesResponse> MatchingClothes = allClothes;

            if(string.IsNullOrEmpty(filterString) || string.IsNullOrEmpty(filterString)) return MatchingClothes;

            switch(filterBy)
            {
                case nameof(Clothes.Model):
                    MatchingClothes = allClothes.Where(temp => (!string.IsNullOrEmpty(temp.Model))? temp.Model.Contains(filterString, StringComparison.OrdinalIgnoreCase):true).ToList();
                    break;
                case nameof(Clothes.Size):
                    MatchingClothes = allClothes.Where(temp => (!string.IsNullOrEmpty(temp.Size) && temp.Size != null) ? temp.Size.Contains(filterString) : true).ToList();
                    break;
                case nameof(Clothes.ClothesType):
                    MatchingClothes = allClothes.Where(temp => (!string.IsNullOrEmpty(temp.ClothesType) && temp.ClothesType != null) ? temp.ClothesType.Contains(filterString) : true).ToList();
                    break;
                case nameof(Clothes.Gender):
                    MatchingClothes = allClothes.Where(temp => (!string.IsNullOrEmpty(temp.Gender)) ? temp.Gender.Equals(filterString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(Clothes.EntryDate):
                    MatchingClothes = allClothes.Where(temp => temp.EntryDate != null && temp.EntryDate.Value.ToString("yyyy-MM-dd").Contains(filterString)).ToList();
                    break;
                case nameof(Clothes.ExitDate):
                    MatchingClothes = allClothes.Where(temp => temp.ExitDate != null && temp.ExitDate.Value.ToString("yyyy-MM-dd").Contains(filterString)).ToList();
                    break;
                case nameof(Clothes.PurchasePrice):
                    MatchingClothes = allClothes.Where(temp => temp.PurchasePrice.ToString() == filterString).ToList();
                    break;
                case nameof(Clothes.Theme):
                    MatchingClothes = allClothes.Where(temp => (!string.IsNullOrEmpty(temp.Theme) && temp.Theme != null) ? temp.Theme.Contains(filterString) : true).ToList();
                    break;
                default: 
                    MatchingClothes = allClothes;
                    break;
            }

            return MatchingClothes;
        }

        public List<ClothesResponse> GetSortedClothes(List<ClothesResponse> allClothes, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy)) return allClothes;

            List<ClothesResponse> SortedClothes = (sortBy, sortOrder) switch
            {
                (nameof(Clothes.Size), SortOrderOptions.ASC) => allClothes.OrderBy(temp => temp.Size).ToList(),
                (nameof(Clothes.Size), SortOrderOptions.DESC) => allClothes.OrderBy(temp => temp.Size).ToList(),
                (nameof(Clothes.ClothesType), SortOrderOptions.ASC) => allClothes.OrderBy(temp => temp.ClothesType).ToList(),
                (nameof(Clothes.ClothesType), SortOrderOptions.DESC) => allClothes.OrderBy(temp => temp.ClothesType).ToList(),
                (nameof(Clothes.EntryDate), SortOrderOptions.ASC) => allClothes.OrderBy(temp => temp.EntryDate).ToList(),
                (nameof(Clothes.EntryDate), SortOrderOptions.DESC) => allClothes.OrderBy(temp => temp.EntryDate).ToList(),
                (nameof(Clothes.ExitDate), SortOrderOptions.ASC) => allClothes.OrderBy(temp => temp.ExitDate).ToList(),
                (nameof(Clothes.ExitDate), SortOrderOptions.DESC) => allClothes.OrderBy(temp => temp.ExitDate).ToList(),
                (nameof(Clothes.Gender), SortOrderOptions.ASC) => allClothes.OrderBy(temp => temp.Gender).ToList(),
                (nameof(Clothes.Gender), SortOrderOptions.DESC) => allClothes.OrderBy(temp => temp.Gender).ToList(),
                (nameof(Clothes.Theme), SortOrderOptions.ASC) => allClothes.OrderBy(temp => temp.Theme).ToList(),
                (nameof(Clothes.Theme), SortOrderOptions.DESC) => allClothes.OrderBy(temp => temp.Theme).ToList(),
                (nameof(Clothes.Model), SortOrderOptions.ASC) => allClothes.OrderBy(temp => temp.Model).ToList(),
                (nameof(Clothes.Model), SortOrderOptions.DESC) => allClothes.OrderBy(temp => temp.Model).ToList(),

                _ => allClothes,
            };

            return SortedClothes;
        }


        public ClothesResponse UpdateClothes(ClothesUpdateRequest? clothesUpdateRequest)
        {
            throw new NotImplementedException();
        }
    }
}
