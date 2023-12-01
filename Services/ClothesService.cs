using ServicesContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Sold;
using ServicesContracts.DTO;
using ServicesContracts.Enums;
using Services.Helpers;

namespace Services
{
    public class ClothesService : IClothesService
    {
        //Private Fields
        private readonly IRepository<Clothes> _repo;
        private readonly IRepository<SoldClothes> _soldRepo;


        //Contructor
        public ClothesService(IRepository<Clothes> clothes, IRepository<SoldClothes> soldRepo) 
        { 
            _repo = clothes;
            _soldRepo = soldRepo;

            
        }

        public async Task<ClothesResponse>  AddClothes(ClothesAddRequest? clothesAddRequest)
        {
            if(clothesAddRequest == null) throw new ArgumentNullException(nameof(clothesAddRequest));

            ValidationHelper.ModelValidation(clothesAddRequest);

            Clothes clothes = clothesAddRequest.ToClothes();
            clothes.ClothesID = Guid.NewGuid();
            clothes.EntryDate = DateTime.Now;
            await _repo.Add(clothes);

            return clothes.ToClothesResponse();
        }
        public async Task<ClothesResponse?>  GetClothesByClothesID(Guid? guid)
        {
            if (guid == null) throw new ArgumentNullException();

            Clothes? clothes = await _repo.GetById((Guid)guid);
            
            if (clothes == null) return null;

            return clothes.ToClothesResponse();

        }
        public async Task<List<ClothesResponse>>  GetAllClothes()
        {
            IEnumerable<Clothes> clothes = await _repo.GetAll(1, 100);
            List<ClothesResponse> toReturn = new List<ClothesResponse>();

            foreach(Clothes piece in clothes)
            {
                toReturn.Add(piece.ToClothesResponse());
            }

            return toReturn;
        }


        public async Task<bool>  DeleteClothes(Guid? guid)
        {
            if(guid == null) throw new ArgumentNullException(nameof(guid));

            Clothes? toRemove = await _repo.GetById((Guid)guid);
            
            if(toRemove == null) return false;

            await _repo.Delete(toRemove);

            return true;
        }

        public async Task<bool>  SoldClothesByClothesID(Guid? guid)
        {
            if (guid == null) throw new ArgumentNullException(nameof(guid));

            Clothes? clothes = await _repo.GetById((Guid)guid);

            if (clothes == null) return false;


            clothes.ExitDate = DateTime.Now;


            await _repo.Delete(clothes);

            await _soldRepo.Add(clothes.ToSoldClothes());

            return true;

        }

        public async Task<List<ClothesResponse>> GetAllSoldClothes()
        {
            List<ClothesResponse> toReturn = new List<ClothesResponse>();

            IEnumerable<SoldClothes> clothes = await _soldRepo.GetAll(1, 100);

            foreach(SoldClothes piece in clothes)
            {
                toReturn.Add(piece.ToClothes().ToClothesResponse());
            }

            return toReturn;
        }


        public async Task<List<ClothesResponse>>  GetFilteredClothes(string filterBy, string? filterString)
        {
            List<ClothesResponse> allClothes = await GetAllClothes();
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

        public async Task<List<ClothesResponse>>  GetSortedClothes(List<ClothesResponse> allClothes, string orderBy, SortOrderOptions sortOrder)
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


        public async Task<ClothesResponse>  UpdateClothes(ClothesUpdateRequest? clothesUpdateRequest)
        {
            if(clothesUpdateRequest == null) throw new ArgumentNullException(nameof(clothesUpdateRequest));

            Clothes? clothesResponse = await _repo.GetById(clothesUpdateRequest.ClothesID);

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


            await _repo.Update(clothesResponse);

            return clothesResponse.ToClothesResponse();
        }
    }
}
