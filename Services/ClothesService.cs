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
        private readonly IRepository<Clothes> _repository;

        //Contructor
        public ClothesService(IRepository<Clothes> clothes) 
        { 
            _repository = clothes;

            if(false)
            {
                ClothesAddRequest request1 = new ClothesAddRequest()
                {
                    Theme = ThemeOptions.Baptism,
                    ClothesType = ClothesTypeOptions.Vestido,
                    Gender = GenderOptions.Female,
                    Model = "17",
                    PurchasePrice = 155.23,
                    Size = "8"
                };
                ClothesAddRequest request2 = new ClothesAddRequest()
                {
                    Theme = ThemeOptions.Baptism,
                    ClothesType = ClothesTypeOptions.Zapatos,
                    Gender = GenderOptions.Male,
                    Model = "Cupon",
                    PurchasePrice = 200.00,
                    Size = "3"
                };
                ClothesAddRequest request3 = new ClothesAddRequest()
                {
                    Theme = ThemeOptions.Wedding,
                    ClothesType = ClothesTypeOptions.Vestido,
                    Gender = GenderOptions.Female,
                    Model = "S23",
                    PurchasePrice = 250.36,
                    Size = "17"
                };
                ClothesAddRequest request4 = new ClothesAddRequest()
                {
                    Theme = ThemeOptions.Other,
                    ClothesType = ClothesTypeOptions.Ropon,
                    Gender = GenderOptions.Female,
                    Model = "2",
                    PurchasePrice = 222.22,
                    Size = "1"
                };
                ClothesAddRequest request5 = new ClothesAddRequest()
                {
                    Theme = ThemeOptions.Other,
                    ClothesType = ClothesTypeOptions.Misc,
                    Gender = GenderOptions.Male,
                    Model = "3.4",
                    PurchasePrice = 36.00,
                    Size = "2"
                };
                ClothesAddRequest request6 = new ClothesAddRequest()
                {
                    Theme = ThemeOptions.Communion,
                    ClothesType = ClothesTypeOptions.Traje,
                    Gender = GenderOptions.Male,
                    Model = "5",
                    PurchasePrice = 136.57,
                    Size = "5"
                };
                this.AddClothes(request1);
                this.AddClothes(request2);
                this.AddClothes(request3);
                this.AddClothes(request4);
                this.AddClothes(request5);
                this.AddClothes(request6);

            }
        }

        public ClothesResponse AddClothes(ClothesAddRequest? clothesAddRequest)
        {
            if(clothesAddRequest == null) throw new ArgumentNullException(nameof(clothesAddRequest));

            ValidationHelper.ModelValidation(clothesAddRequest);

            Clothes clothes = clothesAddRequest.ToClothes();
            clothes.ClothesID = Guid.NewGuid();
            clothes.EntryDate = DateTime.Now;
            _repository.Add(clothes);

            return clothes.ToClothesResponse();
        }
        public ClothesResponse? GetClothesByClothesID(Guid? guid)
        {
            if (guid == null) throw new ArgumentNullException();

            Clothes? clothes = _repository.GetById((Guid)guid);
            
            if (clothes == null) return null;

            return clothes.ToClothesResponse();

        }
        public List<ClothesResponse> GetAllClothes()
        {
            IEnumerable<Clothes> clothes = _repository.GetAll(1, 100);
            List<ClothesResponse> toReturn = new List<ClothesResponse>();

            foreach(Clothes piece in clothes)
            {
                toReturn.Add(piece.ToClothesResponse());
            }

            return toReturn;
        }


        public bool DeleteClothes(Guid? guid)
        {
            if(guid == null) throw new ArgumentNullException(nameof(guid));

            Clothes? toRemove = _repository.GetById((Guid)guid);
            
            if(toRemove == null) return false;

            _repository.Delete(toRemove);

            return true;
        }

        public bool SoldClothesByClothesID(Guid? guid)
        {
            //if (guid == null) throw new ArgumentNullException(nameof(guid));

            //Clothes? clothes = _clothes.FirstOrDefault(temp => temp.ClothesID == guid);

            //if (clothes == null) return false;

            //clothes.ExitDate = DateTime.Now;
            //_soldClothes.Add(clothes);

            //return _clothes.Remove(clothes);
            return false;

        }

        public List<ClothesResponse> GetAllSoldClothes()
        {

            //return _soldClothes.Select(temp => temp.ToClothesResponse()).ToList();
            return new List<ClothesResponse>();
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
                    MatchingClothes = allClothes.Where(temp => (!string.IsNullOrEmpty(temp.Gender)) ? temp.Gender.Contains(filterString, StringComparison.OrdinalIgnoreCase) : true).ToList();
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
                    MatchingClothes = allClothes.Where(temp => (!string.IsNullOrEmpty(temp.Theme)) ? temp.Theme.Contains(filterString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                default: 
                    MatchingClothes = allClothes;
                    break;
            }

            return MatchingClothes;
        }

        public List<ClothesResponse> GetSortedClothes(List<ClothesResponse> allClothes, string orderBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(orderBy)) return allClothes;

            List<ClothesResponse> SortedClothes = (orderBy, sortOrder) switch
            {
                (nameof(Clothes.Size), SortOrderOptions.ASC) => allClothes.OrderBy(temp =>  int.TryParse(temp.Size, out var size) ? size : int.MaxValue).ToList(),
                (nameof(Clothes.Size), SortOrderOptions.DESC) => allClothes.OrderByDescending(temp => int.TryParse(temp.Size, out var size) ? size : int.MaxValue).ToList(),
                (nameof(Clothes.ClothesType), SortOrderOptions.ASC) => allClothes.OrderBy(temp => temp.ClothesType).ToList(),
                (nameof(Clothes.ClothesType), SortOrderOptions.DESC) => allClothes.OrderByDescending(temp => temp.ClothesType).ToList(),
                (nameof(Clothes.EntryDate), SortOrderOptions.ASC) => allClothes.OrderBy(temp => temp.EntryDate).ToList(),
                (nameof(Clothes.EntryDate), SortOrderOptions.DESC) => allClothes.OrderByDescending(temp => temp.EntryDate).ToList(),
                (nameof(Clothes.ExitDate), SortOrderOptions.ASC) => allClothes.OrderBy(temp => temp.ExitDate).ToList(),
                (nameof(Clothes.ExitDate), SortOrderOptions.DESC) => allClothes.OrderByDescending(temp => temp.ExitDate).ToList(),
                (nameof(Clothes.Gender), SortOrderOptions.ASC) => allClothes.OrderBy(temp => temp.Gender).ToList(),
                (nameof(Clothes.Gender), SortOrderOptions.DESC) => allClothes.OrderByDescending(temp => temp.Gender).ToList(),
                (nameof(Clothes.Theme), SortOrderOptions.ASC) => allClothes.OrderBy(temp => temp.Theme).ToList(),
                (nameof(Clothes.Theme), SortOrderOptions.DESC) => allClothes.OrderByDescending(temp => temp.Theme).ToList(),
                (nameof(Clothes.Model), SortOrderOptions.ASC) => allClothes.OrderBy(temp => temp.Model).ToList(),
                (nameof(Clothes.Model), SortOrderOptions.DESC) => allClothes.OrderByDescending(temp => temp.Model).ToList(),
                (nameof(Clothes.PurchasePrice), SortOrderOptions.ASC) => allClothes.OrderBy(temp => temp.PurchasePrice).ToList(),
                (nameof(Clothes.PurchasePrice), SortOrderOptions.DESC) => allClothes.OrderByDescending(temp => temp.PurchasePrice).ToList(),

                _ => allClothes,
            };

            return SortedClothes;
        }


        public ClothesResponse UpdateClothes(ClothesUpdateRequest? clothesUpdateRequest)
        {
            if(clothesUpdateRequest == null) throw new ArgumentNullException(nameof(clothesUpdateRequest));

            Clothes? clothesResponse = _repository.GetById(clothesUpdateRequest.ClothesID);

            if(clothesResponse == null) throw new ArgumentNullException($"ClothesID does not have a match");

            clothesResponse.ClothesID = clothesUpdateRequest.ClothesID;
            clothesResponse.Model = clothesUpdateRequest.Model;
            clothesResponse.ClothesType = clothesUpdateRequest.ClothesType;
            clothesResponse.Gender = clothesUpdateRequest.Gender;
            clothesResponse.EntryDate = clothesUpdateRequest.EntryDate;
            clothesResponse.PurchasePrice = clothesUpdateRequest.PurchasePrice;
            clothesResponse.ExitDate = clothesUpdateRequest.ExitDate;
            clothesResponse.Size = clothesUpdateRequest.Size;
            clothesResponse.Theme = clothesUpdateRequest.Theme;


            _repository.Update(clothesResponse);

            return clothesResponse.ToClothesResponse();
        }
    }
}
