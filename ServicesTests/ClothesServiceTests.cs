using ServicesContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services;
using ServicesContracts.DTO;
using ServicesContracts.Enums;
using Entities;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ServicesTests
{
    public class ClothesServiceTests
    {
        //Private fields
        private readonly IClothesService _clothesService;
        private readonly ITestOutputHelper _testOutputHelper;

        //contructor
        public ClothesServiceTests(ITestOutputHelper testOutputHelper)
        {
            _clothesService = new ClothesService();
            _testOutputHelper = testOutputHelper;
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
            ClothesResponse? clothes_responmse_from_get = _clothesService.GetClothesByClothesID(clothesID);

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

        #region GetFilteredClothes
        //correctly filter by Gender
        [Fact]
        public void GetFilteredClothes_FilterByGender()
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

            List<ClothesResponse> clothes_responses_list_from_add = new List<ClothesResponse>();
            clothes_responses_list_from_add.Add(_clothesService.AddClothes(clothes_add_requerst1));
            clothes_responses_list_from_add.Add(_clothesService.AddClothes(clothes_add_requerst2));
            clothes_responses_list_from_add.Add(_clothesService.AddClothes(clothes_add_requerst3));

            List<ClothesResponse> filtered_clothes_responses_from_add = clothes_responses_list_from_add.Where(temp => (!string.IsNullOrEmpty(temp.Gender)) ? temp.Gender.Equals("male", StringComparison.OrdinalIgnoreCase) : true).ToList();
            _testOutputHelper.WriteLine("Expected");
            foreach (ClothesResponse response in filtered_clothes_responses_from_add)
            {
                _testOutputHelper.WriteLine(response.ToString());
            }
            //Act
            List<ClothesResponse> clothes_responses_from_getfiltered = _clothesService.GetFilteredClothes(nameof(Clothes.Gender), "Male");

            

            _testOutputHelper.WriteLine("Actual");
            foreach(ClothesResponse response in clothes_responses_from_getfiltered)
            {
                _testOutputHelper.WriteLine(response.ToString());
            }



            //Assert
            Assert.Contains(clothes_responses_list_from_add[1], clothes_responses_from_getfiltered);
            Assert.DoesNotContain(clothes_responses_list_from_add[0], clothes_responses_from_getfiltered);
            Assert.DoesNotContain(clothes_responses_list_from_add[2], clothes_responses_from_getfiltered);

        }

        //correctly filter by Model
        [Fact]
        public void GetFilteredClothes_filterByModel()
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

            List<ClothesResponse> clothes_responses_list_from_add = new List<ClothesResponse>();
            clothes_responses_list_from_add.Add(_clothesService.AddClothes(clothes_add_requerst1));
            clothes_responses_list_from_add.Add(_clothesService.AddClothes(clothes_add_requerst2));
            clothes_responses_list_from_add.Add(_clothesService.AddClothes(clothes_add_requerst3));

            //Act
            List<ClothesResponse> clothes_responses_from_getfiltered = _clothesService.GetFilteredClothes(nameof(Clothes.Model), "Suave");

            //Assert
            Assert.Contains(clothes_responses_list_from_add[2], clothes_responses_from_getfiltered);
            Assert.DoesNotContain(clothes_responses_list_from_add[0], clothes_responses_from_getfiltered);
            Assert.DoesNotContain(clothes_responses_list_from_add[1], clothes_responses_from_getfiltered);

        }

        //correctly get all Clothes if to filterstring is provided
        [Fact]
        public void GetFilteredClothes_EmptySearchString()
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

            List<ClothesResponse> clothes_responses_list_from_add = new List<ClothesResponse>();
            clothes_responses_list_from_add.Add(_clothesService.AddClothes(clothes_add_requerst1));
            clothes_responses_list_from_add.Add(_clothesService.AddClothes(clothes_add_requerst2));
            clothes_responses_list_from_add.Add(_clothesService.AddClothes(clothes_add_requerst3));

            

            //Act
            List<ClothesResponse> clothes_responses_from_getfiltered = _clothesService.GetFilteredClothes(nameof(Clothes.Gender), "");
            

            //Assert
            foreach (ClothesResponse clothes in clothes_responses_list_from_add)
            {
                Assert.Contains(clothes, clothes_responses_from_getfiltered);
            }

        }
        #endregion

        #region GetSortedClothes

        //should get asending model order
        [Fact]
        public void GetSortedClothes_SortASC()
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

            List<ClothesResponse> clothes_responses_list_from_add = new List<ClothesResponse>();
            clothes_responses_list_from_add.Add(_clothesService.AddClothes(clothes_add_requerst1));
            clothes_responses_list_from_add.Add(_clothesService.AddClothes(clothes_add_requerst2));
            clothes_responses_list_from_add.Add(_clothesService.AddClothes(clothes_add_requerst3));

            List<ClothesResponse> sorted_clothes_responses_list_from_add = clothes_responses_list_from_add.OrderBy(temp => temp.Model).ToList();

            //Act 
            List<ClothesResponse> sorted_clothes_from_GetSorted = _clothesService.GetSortedClothes(clothes_responses_list_from_add, nameof(Clothes.Model), SortOrderOptions.ASC);


            //Assert
            for(int i = 0; i< sorted_clothes_responses_list_from_add.Count; i++)
            {
                Assert.Equal(sorted_clothes_responses_list_from_add[i], sorted_clothes_from_GetSorted[i]);
            }

        }

        //should get desending order of purchase prices
        [Fact]
        public void GetSortedClothes_SortDESC()
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

            List<ClothesResponse> clothes_responses_list_from_add = new List<ClothesResponse>();
            clothes_responses_list_from_add.Add(_clothesService.AddClothes(clothes_add_requerst1));
            clothes_responses_list_from_add.Add(_clothesService.AddClothes(clothes_add_requerst2));
            clothes_responses_list_from_add.Add(_clothesService.AddClothes(clothes_add_requerst3));

            List<ClothesResponse> sorted_clothes_responses_list_from_add = clothes_responses_list_from_add.OrderByDescending(temp => temp.PurchasePrice).ToList();

            //Act 
            List<ClothesResponse> sorted_clothes_from_GetSorted = _clothesService.GetSortedClothes(clothes_responses_list_from_add, nameof(Clothes.PurchasePrice), SortOrderOptions.DESC);


            //Assert
            for (int i = 0; i < sorted_clothes_responses_list_from_add.Count; i++)
            {
                Assert.Equal(sorted_clothes_responses_list_from_add[i], sorted_clothes_from_GetSorted[i]);
            }

        }
        #endregion

        #region ClothesUpdateRequest

        //should get updates
        [Fact]
        public void ClothesUpdateRequest_ProperUpdatingOfName()
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

            ClothesUpdateRequest clothes_update_response = clothes_response_from_add.ToClothesUpdateRequest();
            clothes_update_response.ClothesType = ClothesTypeOptions.Ropon.ToString();
            
            //Act
            ClothesResponse clothes_response_from_update = _clothesService.UpdateClothes(clothes_update_response);

            ClothesResponse? clothes_from_get = _clothesService.GetClothesByClothesID(clothes_response_from_add.ClothesID);

            //Assert
            if(clothes_from_get != null)
            {
                Assert.Equal(clothes_update_response.ClothesType, clothes_from_get.ClothesType);

            }
            else
            {
                Assert.True(false, "GetClothesByClothesID() is returning null in the test");
            }

        }

        // should throw argumentnullexception if object is null
        [Fact]
        public void ClothesUpdateRequest_NullUpdateRequest()
        {
            //Arrange
            ClothesUpdateRequest? clothesUpdateRequest = null;

            //Arrange
            Assert.Throws<ArgumentNullException>(()=>
            {
                //Act
                _clothesService.UpdateClothes(clothesUpdateRequest);
            });
        }
        #endregion

    }//end of class
}//end of namespace
