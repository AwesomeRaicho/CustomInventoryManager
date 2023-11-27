
using Entities;
using Entities.Sold;
using Microsoft.VisualBasic.FileIO;
using ServicesContracts;
using Services.Helpers;
using ServicesContracts.DTO;
using ServicesContracts.Enums;
using Azure;

namespace Services
{
    public class CostumeService : ICostumeService
    {
        //private fields
        private readonly IRepository<Costume> _repo;
        private readonly IRepository<SoldCostume> _soldRepo;



        //constructor
        public CostumeService( IRepository<Costume> repository, IRepository<SoldCostume> soldRepo)
        {
            _repo = repository;
            _soldRepo = soldRepo;
        }


        public async Task<CostumeResponse>  AddCostume(CostumeAddRequest? costumeAddRequest)
        {
            //validation
            if (costumeAddRequest == null) throw new ArgumentNullException();

            //validation
            ValidationHelper.ModelValidation(costumeAddRequest);

            Costume costume = costumeAddRequest.ToCostume();


            costume.CostumeID = Guid.NewGuid();
            costume.EntryDate = DateTime.Now;

            await _repo.Add(costume);

            return costume.ToCostumeResponse();

        }
        public async Task<List<CostumeResponse>>  GetAllCostumes()
        {
            List<CostumeResponse> costumes = new List<CostumeResponse>();

            IEnumerable<Costume> fromRepo = await _repo.GetAll(1 , 100);

            foreach (Costume costume in fromRepo)
            {
                costumes.Add(costume.ToCostumeResponse());
            }

            return costumes;
        }
        public async Task<CostumeResponse?>  GetCostumeByCostumeID(Guid? costumeID)
        {
            if (costumeID == null) return null;

            Costume? costume = await _repo.GetById((Guid)costumeID);

            if (costume == null) return null;


            return costume.ToCostumeResponse();
        }

        public async Task<List<CostumeResponse>>  GetAllSoldCostumes()
        {
            List<CostumeResponse> toReturn = new List<CostumeResponse>();

            IEnumerable<SoldCostume> soldCostumes = await _soldRepo.GetAll(1 , 100);

            foreach(SoldCostume costume in soldCostumes)
            {
                toReturn.Add(costume.ToCostume().ToCostumeResponse());
            }

            return toReturn;
        }

        public async Task<bool>  SoldCostumeByCostumeID(Guid? costumeID)
        {
            if (costumeID == null) return false;

            Costume? soldCostume = await _repo.GetById((Guid)costumeID);

            if (soldCostume == null)
            {
                return false;
            }
            else
            {
                soldCostume.ExitDate = DateTime.Now;
                await _repo.Delete(soldCostume);

                await _soldRepo.Add(soldCostume.ToSoldCostume());
                return true;
            }

            

        }


        public async Task<List<CostumeResponse>>  GetFilteredCostumes(string filterBy, string? searchString)
        {
            List<CostumeResponse> allCostumes = await GetAllCostumes();
            List<CostumeResponse> matchingCostumes = allCostumes;

            if(string.IsNullOrEmpty(filterBy) || string.IsNullOrEmpty(searchString))
                return matchingCostumes;

            switch(filterBy)
            {
                case nameof(Costume.CostumeName):
                    matchingCostumes = allCostumes.Where(temp => (!string.IsNullOrEmpty(temp.CostumeName)) ? temp.CostumeName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();

                    break;
                case nameof(Costume.Gender):
                    matchingCostumes = allCostumes.Where(temp => (!string.IsNullOrEmpty(temp.Gender)) ? temp.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();

                    break;

                case nameof(Costume.Size):
                    matchingCostumes = allCostumes.Where(temp => (!string.IsNullOrEmpty(temp.Size)) ? temp.Size == searchString : true).ToList();

                    break;
                case nameof(Costume.Age):
                    matchingCostumes = allCostumes.Where(temp => (!string.IsNullOrEmpty(temp.Age)) ? temp.Age == searchString : true).ToList();

                    break;

                case nameof(Costume.PurchasePrice):
                    matchingCostumes = allCostumes.Where(temp => (!string.IsNullOrEmpty(temp.Size)) ? temp.Size == searchString : true).ToList();

                    break;

                case nameof(Costume.EntryDate):
                    matchingCostumes = allCostumes.Where(temp => (!string.IsNullOrEmpty(temp.Size)) ? temp.Size == searchString : true).ToList();

                    break;

                case nameof(Costume.ExitDate):
                    matchingCostumes = allCostumes.Where(temp => (!string.IsNullOrEmpty(temp.Size)) ? temp.Size == searchString : true).ToList();

                    break;

                default: return matchingCostumes;
            }

            return matchingCostumes;
        }
        public async Task<bool> DeleteCostume(Guid? costumeID)
        {
            if(costumeID == null) throw new ArgumentNullException(nameof(costumeID));

            Costume? toRemove = await _repo.GetById((Guid)costumeID);

            if(toRemove == null)
            {
                return false;
            }
            else
            {
                await _repo.Delete(toRemove);

                return true;
            }

        }

        public async Task<List<CostumeResponse>>  GetSortedCostumes(List<CostumeResponse> allCostumes, string orderBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(orderBy)) return allCostumes;

            List<CostumeResponse> OrderedCostumes = (orderBy, sortOrder) switch
            {
                (nameof(Costume.CostumeName), SortOrderOptions.ASC) => allCostumes.OrderBy(temp => temp.CostumeName).ToList(),
                (nameof(Costume.CostumeName), SortOrderOptions.DESC) => allCostumes.OrderByDescending(temp => temp.CostumeName).ToList(),
                (nameof(Costume.Gender), SortOrderOptions.ASC) => allCostumes.OrderBy(temp => temp.Gender).ToList(),
                (nameof(Costume.Gender), SortOrderOptions.DESC) => allCostumes.OrderByDescending(temp => temp.Gender).ToList(),
                (nameof(Costume.Size), SortOrderOptions.ASC) => allCostumes.OrderBy(temp => int.TryParse(temp.Size, out var size) ? size : int.MaxValue).ToList(),
                (nameof(Costume.Size), SortOrderOptions.DESC) => allCostumes.OrderByDescending(temp => int.TryParse(temp.Size, out var size) ? size : int.MaxValue).ToList(),
                (nameof(Costume.Age), SortOrderOptions.ASC) => allCostumes.OrderBy(temp => int.TryParse(temp.Age, out var age) ? age : int.MaxValue).ToList(),
                (nameof(Costume.Age), SortOrderOptions.DESC) => allCostumes.OrderByDescending(temp => int.TryParse(temp.Age, out var age) ? age : int.MaxValue).ToList(),
                (nameof(Costume.EntryDate), SortOrderOptions.ASC) => allCostumes.OrderBy(temp => temp.EntryDate).ToList(),
                (nameof(Costume.EntryDate), SortOrderOptions.DESC) => allCostumes.OrderByDescending(temp => temp.EntryDate).ToList(),
                (nameof(Costume.ExitDate), SortOrderOptions.ASC) => allCostumes.OrderBy(temp => temp.ExitDate).ToList(),
                (nameof(Costume.ExitDate), SortOrderOptions.DESC) => allCostumes.OrderByDescending(temp => temp.ExitDate).ToList(),
                (nameof(Costume.PurchasePrice), SortOrderOptions.ASC) => allCostumes.OrderBy(temp => temp.PurchasePrice).ToList(),
                (nameof(Costume.PurchasePrice), SortOrderOptions.DESC) => allCostumes.OrderByDescending(temp => temp.PurchasePrice).ToList(),

                _ => allCostumes,
            };

            return OrderedCostumes;
        }

        public async Task<CostumeResponse> UpdateCostume(CostumeUpdateRequest? costumeUpdateRequest)
        {
            if (costumeUpdateRequest == null) throw new ArgumentNullException(nameof(costumeUpdateRequest));

            ValidationHelper.ModelValidation(costumeUpdateRequest);

            Costume? costume = await _repo.GetById(costumeUpdateRequest.CostumeID);



            if (costume == null) throw new ArgumentException(nameof(costume.CostumeID));

            costume.CostumeID = costumeUpdateRequest.CostumeID;
            costume.CostumeName = costumeUpdateRequest.CostumeName;
            costume.PurchasePrice = costumeUpdateRequest.PurchasePrice;
            costume.EntryDate = costumeUpdateRequest.EntryDate;
            costume.Age = costumeUpdateRequest.Age;
            costume.Gender = costumeUpdateRequest.Gender.ToString();
            costume.ExitDate = costumeUpdateRequest.ExitDate;
            costume.Size = costumeUpdateRequest.Size;

            await _repo.Update(costume);

            return costume.ToCostumeResponse();
        }

    }
}