
using Entities;
using ServiceContracts;
using Services.Helpers;

namespace Services
{
    public class CostumeService : ICostumeService
    {
        //private fields
        List<Costume> _costumes;

        //constructor
        public CostumeService()
        {
            _costumes = new List<Costume>();
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

        public bool DeleteCostume(Guid? costumeID)
        {
            throw new NotImplementedException();
        }


        public CostumeResponse GetCostumeByCostumeID(Guid? costumeID)
        {
            throw new NotImplementedException();
        }

        public bool RemoveCostume(Guid? costumeID)
        {
            throw new NotImplementedException();
        }
    }
}