using ServiceContracts;
using ServiceContracts.Enums;

using Services;
using Xunit.Sdk;

namespace ServicesTests
{
    public class CostumeServiceTests
    {
        //private fields
        public readonly ICostumeService _costumeService;

        //constructor 
        public CostumeServiceTests()
        {
            _costumeService = new CostumeService();
        }


        //Tests

        #region AddCostume

        // if costume obj is null it should throw exception
        [Fact]
        public void AddCostume_NullCostume()
        {
            
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _costumeService.AddCostume(null);
            });
        }

        //if any properties in costume are null, it should throw exception
        [Fact]
        public void AddCostume_NullCostumeProperties()
        {
            //Arrange
            CostumeAddRequest costume_add_request = new CostumeAddRequest();

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _costumeService.AddCostume(costume_add_request);
            });
        }

        //if proper details are provided, it should return CostumeResponse
        [Fact]
        public void AddCostume_ProperCostumeDetails()
        {
            //Arrange
            CostumeAddRequest costume_add_request = new CostumeAddRequest()
            {
                CostumeName = "Wonda Woman",
                Age = 8,
                Size = 8,
                Gender = GenderOptions.Female,
                PurchasePrice = 250.50
            };

            //Act 
            CostumeResponse? costume_response_from_add = _costumeService.AddCostume(costume_add_request);

            //Assert
            Assert.True(costume_response_from_add.CostumeID != Guid.Empty);
        }

        #endregion

        #region GetAllCostumes

        [Fact]
        public void GetAllCostumes_EmptyList()
        {
            //Act
            List<CostumeResponse> costumes_from_getall = _costumeService.GetAllCostumes();

            //Assert
            Assert.Empty(costumes_from_getall);


        }

        [Fact]
        public void GetAllCostumes_AddFewCostumes()
        {
            //Arrange
            CostumeAddRequest costume1 = new CostumeAddRequest()
            {
                CostumeName = "WondaWuman",
                Gender = GenderOptions.Female,
                Size = 5,
                Age=8,
                PurchasePrice = 355.50,
            };
            CostumeAddRequest costume2 = new CostumeAddRequest()
            {
                CostumeName = "SupaMa",
                Gender = GenderOptions.Male,
                Size = 7,
                Age = 5,
                PurchasePrice = 100.20,
            }; CostumeAddRequest costume3 = new CostumeAddRequest()
            {
                CostumeName = "Baturuman",
                Gender = GenderOptions.Male,
                Size = 10,
                Age = 13,
                PurchasePrice = 130.30,
            };

            List<CostumeAddRequest> costumes_requests = new List<CostumeAddRequest>
            { 
                costume1, 
                costume2, 
                costume3 
            };

            List<CostumeResponse> costumes_from_add = new List<CostumeResponse>();

            foreach(CostumeAddRequest costume_request in costumes_requests)
            {
                costumes_from_add.Add(_costumeService.AddCostume(costume_request));
            }

            //Act
            List<CostumeResponse> costumes_from_getall = _costumeService.GetAllCostumes();

            //Assert
            foreach(CostumeResponse costume in costumes_from_add)
            {
                Assert.Contains(costume, costumes_from_getall);
            }

        }
        #endregion
    }
}