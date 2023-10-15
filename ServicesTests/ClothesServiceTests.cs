using ServicesContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services;
using ServicesContracts.DTO;
using ServicesContracts.Enums;
using NuGet.Frameworks;

namespace ServicesTests
{
    public class ClothesServiceTests
    {
        //Private fields
        private readonly IClothesService _clothesService;

        //contructor
        public ClothesServiceTests()
        {
            _clothesService = new ClothesService();
        }

        //Tests

        #region AddClothes

        //should throw argumentnullException if provided with null request
        [Fact]
        public void AddClothes_NullRequest()
        {
            // Arrange
            ClothesAddRequest? clothes_add_request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _clothesService.AddClothes(clothes_add_request);
            });
        }

        //should throw exception if any values are null
        [Fact]
        public void AddClothes_NullProperties()
        {
            //Arrange
            ClothesAddRequest clothes_add_request = new ClothesAddRequest();

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _clothesService.AddClothes(clothes_add_request);
            });
        }

        //should return ClothesResponse with Guid id (ClothesID) if proper details are provided
        [Fact]
        public void AddClothes_ProperDetails()
        {
            //Arrange 
            ClothesAddRequest clothes_add_requerst = new ClothesAddRequest()
            {
                ClothesType = ClothesTypeOptions.Vestido,
                Theme = ThemeOptions.Baptism,
                Gender = GenderOptions.Female,
                Model = "003",
                Size = "10",
                PurchasePrice = 450.50
            };

            //Act
            ClothesResponse? clothes_response = _clothesService.AddClothes(clothes_add_requerst);

            //Assert
            Assert.True(clothes_response.ClothesID != Guid.Empty);
        }
        #endregion

        #region GetClothesByClothesID

        //should throw exception if null Guid (ClothesID)
        [Fact]
        public void GetClothesByClothesID_NullClothesID()
        {
            //Arrange
            Guid? clothesID = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _clothesService.GetClothesByClothesID(clothesID);
            });
        }

        //if guid is not found, it should return null
        [Fact]
        public void GetClothesByClothesID_NonExistingClothesID()
        {
            //Arrange
            Guid? clothesID = Guid.NewGuid();

            //Act
            ClothesResponse? clothes_response = _clothesService.GetClothesByClothesID(clothesID);

            //Assert
            Assert.Null(clothes_response);
        }

        //Should get proper ClothesResponse when providing correct ClothesID (Guid)
        [Fact]
        public void GetClothesByClothesID_ProperID()
        {
            //Arrange
            ClothesAddRequest clothes_add_requerst = new ClothesAddRequest()
            {
                ClothesType = ClothesTypeOptions.Vestido,
                Theme = ThemeOptions.Baptism,
                Gender = GenderOptions.Female,
                Model = "003",
                Size = "10",
                PurchasePrice = 450.50
            };

            ClothesResponse? clothes_response_from_add = _clothesService.AddClothes(clothes_add_requerst);
            Guid? clothesID = clothes_response_from_add.ClothesID;
            //Act
            ClothesResponse clothes_responmse_from_get = _clothesService.GetClothesByClothesID(clothesID);

            //Assert
            Assert.Equal(clothes_response_from_add, clothes_responmse_from_get);
        }
        #endregion

        #region GetAllClothes

        //Should get an empty list of no Clothes in DB
        [Fact]
        public void GetAllClothes_EmptyList()
        {
            //Act
            List<ClothesResponse> clothes_response_list = _clothesService.GetAllClothes();

            //Arrange
            Assert.Empty(clothes_response_list);
        }

        //should get clothes in DB if DB contains entries
        [Fact]
        public void GetClothes_GetListOfClothes() 
        {
            //Arrange 
            ClothesAddRequest clothes_add_requerst1 = new ClothesAddRequest()
            {
                ClothesType = ClothesTypeOptions.Vestido,
                Theme = ThemeOptions.Baptism,
                Gender = GenderOptions.Female,
                Model = "003",
                Size = "10",
                PurchasePrice = 450.50
            };
            ClothesAddRequest clothes_add_requerst2 = new ClothesAddRequest()
            {
                ClothesType = ClothesTypeOptions.Tunica,
                Theme = ThemeOptions.Baptism,
                Gender = GenderOptions.Male,
                Model = "3",
                Size = "1",
                PurchasePrice = 450.50
            };            
            ClothesAddRequest clothes_add_requerst3 = new ClothesAddRequest()
            {
                ClothesType = ClothesTypeOptions.Misc,
                Theme = ThemeOptions.Baptism,
                Gender = GenderOptions.Female,
                Model = "Suave",
                Size = "16",
                PurchasePrice = 450.50
            };

            List<ClothesAddRequest> clothes_add_request_list = new List<ClothesAddRequest>();

            clothes_add_request_list.Add(clothes_add_requerst1);
            clothes_add_request_list.Add(clothes_add_requerst2);
            clothes_add_request_list.Add(clothes_add_requerst3);

            List<ClothesResponse> clothes_responses_from_add = new List<ClothesResponse>();

            foreach(ClothesAddRequest request in clothes_add_request_list)
            {
                clothes_responses_from_add.Add(_clothesService.AddClothes(request));
            }
            
            //Act
            List<ClothesResponse> clothes_responses_from_getAll = _clothesService.GetAllClothes();

            //Assert
            foreach(ClothesResponse response in clothes_responses_from_add)
            {
                Assert.Contains(response, clothes_responses_from_getAll);
            }
        }
        #endregion

        #region DeleteClothes

        //should throw argumentnullexception if guid is null
        [Fact]
        public void DeleteClothes_NullClothesID()
        {
            //Arrange
            Guid? clothesID = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() => 
            { 
                //Act
                _clothesService.DeleteClothes(clothesID); 
            });
        }

        //should should return false if ClothesID does not exist in the DB
        [Fact]
        public void DeleteClothes_NonExistingClothesID()
        {
            //Arrange 
            Guid? clothesID = Guid.NewGuid();

            //Act
            bool response = _clothesService.DeleteClothes(clothesID);

            //Assert
            Assert.False(response);

        }


        //Should get true if existing clothes was properly removed from the DB
        [Fact]
        public void DeleteClothes_ProperImplementation()
        {
            //Arrange 
            ClothesAddRequest clothes_add_requerst1 = new ClothesAddRequest()
            {
                ClothesType = ClothesTypeOptions.Vestido,
                Theme = ThemeOptions.Baptism,
                Gender = GenderOptions.Female,
                Model = "003",
                Size = "10",
                PurchasePrice = 450.50
            };

            ClothesResponse clothes_response_from_add = _clothesService.AddClothes(clothes_add_requerst1);
            Guid? clothesID_to_delete = clothes_response_from_add.ClothesID;

            //Act
            bool response = _clothesService.DeleteClothes(clothesID_to_delete);


            //Assert
            Assert.True(response);


        }

        #endregion

        #region GetAllSoldClothes
        //Should throw empty list if "Sold" DB is empty
        [Fact]
        public void GetAllSoldClothes_EmptyList()
        {
            //Act
            List<ClothesResponse> clothes_response = _clothesService.GetAllSoldClothes();

            //Assert
            Assert.Empty(clothes_response);
        }

        //should return all sold Clothes if "Sold" DB containes entries
        [Fact]
        public void GetAllSoldClothes_GetProperList()
        {
            //Arrange 
            ClothesAddRequest clothes_add_requerst1 = new ClothesAddRequest()
            {
                ClothesType = ClothesTypeOptions.Vestido,
                Theme = ThemeOptions.Baptism,
                Gender = GenderOptions.Female,
                Model = "003",
                Size = "10",
                PurchasePrice = 450.50
            };
            ClothesAddRequest clothes_add_requerst2 = new ClothesAddRequest()
            {
                ClothesType = ClothesTypeOptions.Tunica,
                Theme = ThemeOptions.Baptism,
                Gender = GenderOptions.Male,
                Model = "3",
                Size = "1",
                PurchasePrice = 450.50
            };
            ClothesAddRequest clothes_add_requerst3 = new ClothesAddRequest()
            {
                ClothesType = ClothesTypeOptions.Misc,
                Theme = ThemeOptions.Baptism,
                Gender = GenderOptions.Female,
                Model = "Suave",
                Size = "16",
                PurchasePrice = 450.50
            };

            List<ClothesAddRequest> clothes_add_request_list = new List<ClothesAddRequest>();

            //make a List of requests
            clothes_add_request_list.Add(clothes_add_requerst1);
            clothes_add_request_list.Add(clothes_add_requerst2);
            clothes_add_request_list.Add(clothes_add_requerst3);

            //create a list of the responses from adding to DB
            List<ClothesResponse> clothes_responses_from_add = new List<ClothesResponse>();

            foreach (ClothesAddRequest request in clothes_add_request_list)
            {
                clothes_responses_from_add.Add(_clothesService.AddClothes(request));
            }

            //remove the clothes from the DB using the clothesID returned on the previous list
            foreach(ClothesResponse response in clothes_responses_from_add)
            {
                _clothesService.SoldClothesByClothesID(response.ClothesID);
            }

            //Act 
            List<ClothesResponse> clothes_response_from_getAllSold = _clothesService.GetAllSoldClothes();

            //Assert

            //loop through the response list to verify if the ClothesID (Guid) is in the list from the Sold DB (consider that the equal is overriden to simply check Guid id)
            foreach(ClothesResponse response in clothes_responses_from_add)
            {
                Assert.Contains(response, clothes_response_from_getAllSold);
            }

        }


        #endregion

        #region SoldClothesByClothesID

        //if clothesID is null, should throw argumentnullexception
        [Fact]
        public void SoldClothesByClothesID_NullID()
        {
            //Arrange
            Guid? ClothesID = null;

            //Assert
            Assert.Throws<ArgumentNullException>(()=>
            {
                //Act
                _clothesService.SoldClothesByClothesID(ClothesID);
            });
        }

        //should return false if ClothesID does not exists
        [Fact]
        public void SoldClothesByClothesID_IDDoesNotExists()
        {
            //Arrange
            Guid clothesID = Guid.NewGuid();

            //Act
            bool response = _clothesService.SoldClothesByClothesID(clothesID);

            //Assert
            Assert.False(response);
        }

        //should properly return Clothes response with correct ID provisded
        [Fact]
        public void SoldClothesByClothesID_ProperIDProvided()
        {
            //Arrange
            ClothesAddRequest clothes_add_requerst = new ClothesAddRequest()
            {
                ClothesType = ClothesTypeOptions.Misc,
                Theme = ThemeOptions.Baptism,
                Gender = GenderOptions.Female,
                Model = "Suave",
                Size = "16",
                PurchasePrice = 450.50
            };

            ClothesResponse clothes_response_from_add = _clothesService.AddClothes(clothes_add_requerst);
            Guid clothesID = clothes_response_from_add.ClothesID;


            //Act
            bool response = _clothesService.SoldClothesByClothesID(clothesID);

            //Assert
            Assert.True(response);

            //also,check if the removed ID is contained in "Sold" DB
            List<ClothesResponse> clothesResponses_from_SoldList = _clothesService.GetAllSoldClothes();

            Assert.Contains(clothes_response_from_add, clothesResponses_from_SoldList);


        }

        #endregion

    }//end of class
}//end of namespace
