
using Entities;
using Microsoft.VisualBasic.FileIO;
using ServicesContracts;
using Services.Helpers;
using ServicesContracts.DTO;
using ServicesContracts.Enums;

namespace Services
{
    public class CostumeService : ICostumeService
    {
        //private fields
        List<Costume> _costumes;
        List<Costume> _soldCostumes;

        //constructor
        public CostumeService(bool init = true)
        {
            _costumes = new List<Costume>(); 
            _soldCostumes = new List<Costume>();

            if (init)
            {
                List<CostumeAddRequest> requests1 = new List<CostumeAddRequest>();
                CostumeAddRequest request1 = new CostumeAddRequest()
                {
                    CostumeName = "wonda Woman",
                    Gender = GenderOptions.Female,
                    Age = "8",
                    Size = "8",
                    PurchasePrice = 150.00,
                };
                CostumeAddRequest request2 = new CostumeAddRequest()
                {
                    CostumeName = "Batman",
                    Gender = GenderOptions.Male,
                    Age = "10",
                    Size = "10",
                    PurchasePrice = 200.00,
                };
                CostumeAddRequest request3 = new CostumeAddRequest()
                {
                    CostumeName = "Supaman",
                    Gender = GenderOptions.Male,
                    Age = "13",
                    Size = "16",
                    PurchasePrice = 89.99,
                };
                CostumeAddRequest request4 = new CostumeAddRequest()
                {
                    CostumeName = "Super Girl",
                    Gender = GenderOptions.Female,
                    Age = "10",
                    Size = "10",
                    PurchasePrice = 122.22,
                };
                CostumeAddRequest request5 = new CostumeAddRequest()
                {
                    CostumeName = "Flash",
                    Gender = GenderOptions.Male,
                    Age = "15",
                    Size = "15",
                    PurchasePrice = 175.00,
                };

                AddCostume(request1);
                AddCostume(request2);
                AddCostume(request3);
                AddCostume(request4);
                AddCostume(request5);
            }
        }


        public CostumeResponse AddCostume(CostumeAddRequest? costumeAddRequest)
        {
            //validation
            if (costumeAddRequest == null) throw new ArgumentNullException();

            //validation
            ValidationHelper.ModelValidation(costumeAddRequest);

            Costume costume = costumeAddRequest.ToCostume();

            costume.CostumeID = Guid.NewGuid();
            costume.EntryDate = DateTime.Now;

            _costumes.Add(costume);

            return costume.ToCostumeResponse();

        }
        public List<CostumeResponse> GetAllCostumes()
        {
            return _costumes.Select(costume => costume.ToCostumeResponse()).ToList();
        }
        public CostumeResponse? GetCostumeByCostumeID(Guid? costumeID)
        {
            if (costumeID == null) return null;

            return _costumes.FirstOrDefault(costume => costume.CostumeID == costumeID)?.ToCostumeResponse();
        }

        public List<CostumeResponse> GetAllSoldCostumes()
        {
            return _soldCostumes.Select(costume => costume.ToCostumeResponse()).ToList();
        }

        public bool SoldCostumeByCostumeID(Guid? costumeID)
        {
            if(costumeID == null) return false;

            Costume? soldCostume = _costumes.FirstOrDefault(costume => costume.CostumeID == costumeID);

            if (soldCostume == null)
            {
                return false;
            }
            else
            {
                soldCostume.ExitDate = DateTime.Now;
                _soldCostumes.Add(soldCostume);
            }

            _costumes.RemoveAll(costume => costume.CostumeID == costumeID);
            return true;

        }


        public List<CostumeResponse> GetFilteredCostumes(string filterBy, string? searchString)
        {
            List<CostumeResponse> allCostumes = GetAllCostumes();
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

                default: return matchingCostumes;
            }

            return matchingCostumes;
        }
        public bool DeleteCostume(Guid? costumeID)
        {
            if(costumeID == null) throw new ArgumentNullException(nameof(costumeID));

            Costume? toRemove = _costumes.FirstOrDefault(temp => temp.CostumeID == costumeID);

            if(toRemove == null) return false;

            return  _costumes.Remove(toRemove);
        }

        public List<CostumeResponse> GetSortedCostumes(List<CostumeResponse> allCostumes, string orderBy, SortOrderOptions sortOrder)
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
                _ => allCostumes,
            };

            return OrderedCostumes;
        }

        public CostumeResponse UpdateCostume(CostumeUpdateRequest? costumeUpdateRequest)
        {
            if (costumeUpdateRequest == null) throw new ArgumentNullException(nameof(costumeUpdateRequest));

            ValidationHelper.ModelValidation(costumeUpdateRequest);

            Costume? costume = _costumes.FirstOrDefault(temp => temp.CostumeID == costumeUpdateRequest.CostumeID);

            if (costume == null) throw new ArgumentException(nameof(costume.CostumeID));

            costume.CostumeID = costumeUpdateRequest.CostumeID;
            costume.CostumeName = costumeUpdateRequest.CostumeName;
            costume.PurchasePrice = costumeUpdateRequest.PurchasePrice;
            costume.EntryDate = costumeUpdateRequest.EntryDate;
            costume.Age = costumeUpdateRequest.Age;
            costume.Gender = costumeUpdateRequest.Gender.ToString();
            costume.ExitDate = costumeUpdateRequest.ExitDate;
            costume.Size = costumeUpdateRequest.Size;

            return costume.ToCostumeResponse();
        }

    }
}