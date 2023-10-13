using ServiceContracts;
using ServiceContracts.Enums;
using Entities;
using Services;
using Xunit.Sdk;
using Xunit.Abstractions;

namespace ServicesTests
{
    public class CostumeServiceTests
    {
        //private fields
        public readonly ICostumeService _costumeService;
        public readonly ITestOutputHelper _testOutputHelper;

        //constructor 
        public CostumeServiceTests(ITestOutputHelper testOutputHelper)
        {
            _costumeService = new CostumeService();
            _testOutputHelper = testOutputHelper;
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
                Age = "8",
                Size = "8",
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
                Size = "5",
                Age = "8",
                PurchasePrice = 355.50,
            };
            CostumeAddRequest costume2 = new CostumeAddRequest()
            {
                CostumeName = "SupaMa",
                Gender = GenderOptions.Male,
                Size = "7",
                Age = "5",
                PurchasePrice = 100.20,
            }; CostumeAddRequest costume3 = new CostumeAddRequest()
            {
                CostumeName = "Baturuman",
                Gender = GenderOptions.Male,
                Size = "10",
                Age = "13",
                PurchasePrice = 130.30,
            };

            List<CostumeAddRequest> costumes_requests = new List<CostumeAddRequest>
            {
                costume1,
                costume2,
                costume3
            };

            List<CostumeResponse> costumes_from_add = new List<CostumeResponse>();

            foreach (CostumeAddRequest costume_request in costumes_requests)
            {
                costumes_from_add.Add(_costumeService.AddCostume(costume_request));
            }

            //Act
            List<CostumeResponse> costumes_from_getall = _costumeService.GetAllCostumes();

            //Assert
            foreach (CostumeResponse costume in costumes_from_add)
            {
                Assert.Contains(costume, costumes_from_getall);
            }

        }
        #endregion

        #region GetCostumeByCostumeID

        [Fact]
        public void GetCostumeByCostumeID_NullCostumeID()
        {
            //Arrange
            Guid? costumeID = null;

            //Act
            CostumeResponse? costume_response = _costumeService.GetCostumeByCostumeID(costumeID);

            //Assert
            Assert.Null(costume_response);
        }

        [Fact]
        public void GetCostumeByCostumeID_GetCostume()
        {
            //Arrange
            CostumeAddRequest costumeAddRequest = new CostumeAddRequest()
            {
                CostumeName = "Batman",
                Gender = GenderOptions.Male,
                Age = "8",
                Size = "32",
                PurchasePrice = 250,
            };

            CostumeResponse costume_response_from_add = _costumeService.AddCostume(costumeAddRequest);

            Guid costumeID_of_added = costume_response_from_add.CostumeID;

            //Act
            CostumeResponse? costume_response_from_ID = _costumeService.GetCostumeByCostumeID(costumeID_of_added);

            //Assert
            Assert.Equal(costume_response_from_add, costume_response_from_ID);
        }

        #endregion

        #region GetAllSoldCostumes

        [Fact]
        public void GetAllSoldCostumes_EmptyList()
        {
            //Act
            List<CostumeResponse> costume_responses = _costumeService.GetAllSoldCostumes();

            //Assert
            Assert.Empty(costume_responses);
        }

        [Fact]
        public void GetAllSoldCostumes_ProperSoldCostumes()
        {
            //Arrange

            CostumeAddRequest costume1 = new CostumeAddRequest()
            {
                CostumeName = "Supaman",
                Gender = GenderOptions.Male,
                Age = "8",
                Size = "10",
                PurchasePrice = 150.35
            };
            CostumeAddRequest costume2 = new CostumeAddRequest()
            {
                CostumeName = "Batuman",
                Gender = GenderOptions.Male,
                Age = "13",
                Size = "20",
                PurchasePrice = 180.56
            };
            CostumeAddRequest costume3 = new CostumeAddRequest()
            {
                CostumeName = "Flash",
                Gender = GenderOptions.Male,
                Age = "4",
                Size = "2",
                PurchasePrice = 89.99
            };

            List<CostumeResponse> costumeResponses_from_add = new List<CostumeResponse>();
            costumeResponses_from_add.Add(_costumeService.AddCostume(costume1));
            costumeResponses_from_add.Add(_costumeService.AddCostume(costume2));
            costumeResponses_from_add.Add(_costumeService.AddCostume(costume3));

            foreach (CostumeResponse costume in costumeResponses_from_add)
            {
                _costumeService.SoldCostumeByCostumeID(costume.CostumeID);
            }

            //Act
            List<CostumeResponse> costumes_from_getSold = _costumeService.GetAllSoldCostumes();

            //Assert
            foreach (CostumeResponse costume in costumeResponses_from_add)
            {
                Assert.Contains(costume, costumes_from_getSold);
            }



        }


        #endregion

        #region SoldCostume

        [Fact]
        public void SoldCostumeByCostumeID_NullID()
        {
            //Arrange
            Guid? costumeID = null;
            //Act
            bool Response = _costumeService.SoldCostumeByCostumeID(costumeID);
            //Assert
            Assert.False(Response);
        }

        [Fact]
        public void SoldCostumeByCostumeID_ProperCostumeID()
        {
            //Arrange
            CostumeAddRequest costumeAddRequest = new CostumeAddRequest()
            {
                CostumeName = "Supaman",
                Gender = GenderOptions.Male,
                Age = "10",
                Size = "8",
                PurchasePrice = 130.34,
            };
            CostumeResponse costumeResponse_from_add = _costumeService.AddCostume(costumeAddRequest);

            //Act
            _costumeService.SoldCostumeByCostumeID(costumeResponse_from_add.CostumeID);

            List<CostumeResponse> soldList = _costumeService.GetAllSoldCostumes();

            //Assert
            Assert.Contains(costumeResponse_from_add, soldList);
            Assert.DoesNotContain(costumeResponse_from_add, _costumeService.GetAllCostumes());
        }
        #endregion

        #region GetFilteredCostumes

        [Fact]
        public void GetFilteredCostumes_CorrectFiltering()
        {
            //Arrange
            CostumeAddRequest costume1 = new CostumeAddRequest()
            {
                CostumeName = "Supaman",
                Gender = GenderOptions.Male,
                Age = "8",
                Size = "10",
                PurchasePrice = 150.35
            };
            CostumeAddRequest costume2 = new CostumeAddRequest()
            {
                CostumeName = "Batuman",
                Gender = GenderOptions.Male,
                Age = "13",
                Size = "20",
                PurchasePrice = 180.56
            };
            CostumeAddRequest costume3 = new CostumeAddRequest()
            {
                CostumeName = "Flash",
                Gender = GenderOptions.Male,
                Age = "4",
                Size = "2",
                PurchasePrice = 89.99
            };

            List<CostumeResponse> costumeResponses_from_add = new List<CostumeResponse>();
            costumeResponses_from_add.Add(_costumeService.AddCostume(costume1));
            costumeResponses_from_add.Add(_costumeService.AddCostume(costume2));
            costumeResponses_from_add.Add(_costumeService.AddCostume(costume3));



            //getting all person names that match
            List<CostumeResponse> filtered_costumeResponses_from_add = costumeResponses_from_add.Where(temp => (!string.IsNullOrEmpty(temp.CostumeName)) ? temp.CostumeName.Contains("man", StringComparison.OrdinalIgnoreCase) : false).ToList();
            _testOutputHelper.WriteLine("\n*** Expected ***");
            foreach(CostumeResponse costume in filtered_costumeResponses_from_add)
            {
                _testOutputHelper.WriteLine(costume.CostumeName);
                _testOutputHelper.WriteLine(costume.CostumeID.ToString());
            }
            //Act
            List<CostumeResponse> costumeResponses_from_getFiltered = _costumeService.GetFilteredCostumes(nameof(Costume.CostumeName), "man");

            _testOutputHelper.WriteLine("\n*** Actual ***");
            foreach (CostumeResponse costume in costumeResponses_from_getFiltered)
            {
                _testOutputHelper.WriteLine(costume.CostumeName);
                _testOutputHelper.WriteLine(costume.CostumeID.ToString());
            }


            //Assert

            //check that 'Flash' is not in the list
            Assert.DoesNotContain(costumeResponses_from_add[2], costumeResponses_from_getFiltered);

            //checkes that names containing 'man' are on the list
            foreach(CostumeResponse costume in filtered_costumeResponses_from_add)
            {
                Assert.Contains(costume, costumeResponses_from_getFiltered);
            }
        }

        #endregion

    }
}