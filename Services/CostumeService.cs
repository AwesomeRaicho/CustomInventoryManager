
using Entities;
using ServiceContracts;
using Services.Helpers;

namespace Services
{
    public class CostumeService : ICostumeService
    {
        //private fields
        List<Costume> _costumes;
        List<Costume> _soldCostumes;

        //constructor
        public CostumeService()
        {
            _costumes = new List<Costume>(); 
            _soldCostumes = new List<Costume>();
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

        public bool DeleteCostume(Guid? costumeID)
        {
            throw new NotImplementedException();
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
    }
}