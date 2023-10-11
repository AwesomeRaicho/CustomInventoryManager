
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

    }
}