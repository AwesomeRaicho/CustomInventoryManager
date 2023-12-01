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
using Moq;
using Entities.Sold;
using System.Runtime.CompilerServices;

namespace ServicesTests
{
    public class ClothesServiceTests
    {
        //Private fields
        private readonly Mock<IRepository<Clothes>> _clothesRepo;
        private readonly Mock<IRepository<SoldClothes>> _soldClothesRepo;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IClothesService _clothesService;

        //contructor
        public ClothesServiceTests(ITestOutputHelper testOutputHelper)
        {
            _clothesRepo = new Mock<IRepository<Clothes>>();
            _soldClothesRepo = new Mock<IRepository<SoldClothes>>();
            _testOutputHelper = testOutputHelper;
            _clothesService = new ClothesService(_clothesRepo.Object, _soldClothesRepo.Object);
        }

        //Tests

        #region AddClothes

        //should throw argumentnullException if provided with null request
        [Fact]
        public async Task AddClothes_NullRequest()
        {
            // Arrange
            ClothesAddRequest? clothes_add_request = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>( async () =>
            {
                //Act
                await _clothesService.AddClothes(clothes_add_request);
            });
        }

        //should throw exception if any values are null
        [Fact]
        public async Task AddClothes_NullProperties()
        {
            //Arrange
            ClothesAddRequest clothes_add_request = new ClothesAddRequest();

            //Assert
            await Assert.ThrowsAsync<ArgumentException>( async() =>
            {
                //Act
                await _clothesService.AddClothes(clothes_add_request);
            });
        }

        //should return ClothesResponse with Guid id (ClothesID) if proper details are provided
        [Fact]
        public async Task AddClothes_ProperDetails()
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
            ClothesResponse? clothes_response = await _clothesService.AddClothes(clothes_add_requerst);

            //Assert
            Assert.True(clothes_response.ClothesID != Guid.Empty);
        }
        #endregion

        #region GetClothesByClothesID

        //should throw exception if null Guid (ClothesID)
        [Fact]
        public async Task GetClothesByClothesID_NullClothesID()
        {
            //Arrange
            Guid? clothesID = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>( async () =>
            {
                //Act
                await _clothesService.GetClothesByClothesID(clothesID);
            });
        }

        //if guid is not found, it should return null
        [Fact]
        public async Task GetClothesByClothesID_NonExistingClothesID()
        {
            //Arrange
            Guid? clothesID = Guid.NewGuid();

            //Act
            ClothesResponse? clothes_response = await _clothesService.GetClothesByClothesID(clothesID);

            //Assert
            Assert.Null(clothes_response);
        }

        //Should get proper ClothesResponse when providing correct ClothesID (Guid)
        [Fact]
        public async Task GetClothesByClothesID_ProperID()
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


            ClothesResponse? clothes_response_from_add = await _clothesService.AddClothes(clothes_add_requerst);
            Guid? clothesID = clothes_response_from_add.ClothesID;

            _clothesRepo.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(clothes_response_from_add.ToClothesUpdateRequest().ToClothes());

            //Act
            ClothesResponse? clothes_responmse_from_get = await _clothesService.GetClothesByClothesID(clothesID);

            //Assert
            Assert.Equal(clothes_response_from_add, clothes_responmse_from_get);
        }
        #endregion

        #region GetAllClothes

        //Should get an empty list of no Clothes in DB
        [Fact]
        public async Task GetAllClothes_EmptyList()
        {
            //Act
            List<ClothesResponse> clothes_response_list = await _clothesService.GetAllClothes();

            //Arrange
            Assert.Empty(clothes_response_list);
        }

        //should get clothes in DB if DB contains entries
        [Fact]
        public async Task GetClothes_GetListOfClothes()
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

            foreach (ClothesAddRequest request in clothes_add_request_list)
            {
                clothes_responses_from_add.Add(request.ToClothes().ToClothesResponse());
            }

            List<Clothes> repoClothesResponse = new List<Clothes>();

            foreach (ClothesAddRequest request in clothes_add_request_list)
            {
                repoClothesResponse.Add(request.ToClothes());
            }

            _clothesRepo.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(repoClothesResponse);

            //Act
            List<ClothesResponse> clothes_responses_from_getAll = await _clothesService.GetAllClothes();

            //Assert
            foreach (ClothesResponse response in clothes_responses_from_add)
            {
                Assert.Contains(response, clothes_responses_from_getAll);
            }
        }
        #endregion

        #region DeleteClothes

        //should throw argumentnullexception if guid is null
        [Fact]
        public async Task DeleteClothes_NullClothesID()
        {
            //Arrange
            Guid? clothesID = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>( async () =>
            {
                //Act
                await _clothesService.DeleteClothes(clothesID);
            });
        }

        //should should return false if ClothesID does not exist in the DB
        [Fact]
        public async Task DeleteClothes_NonExistingClothesID()
        {
            //Arrange 
            Guid? clothesID = Guid.NewGuid();

            //Act
            bool response = await _clothesService.DeleteClothes(clothesID);

            //Assert
            Assert.False(response);

        }


        //Should get true if existing clothes was properly removed from the DB
        [Fact]
        public async Task DeleteClothes_ProperImplementation()
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

            ClothesResponse clothes_response_from_add = await _clothesService.AddClothes(clothes_add_requerst1);
            Guid? clothesID_to_delete = clothes_response_from_add.ClothesID;

            Clothes repoReturn = clothes_add_requerst1.ToClothes();
            repoReturn.ClothesID = (Guid)clothesID_to_delete;

            _clothesRepo.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(repoReturn);
            
            //Act
            bool response = await _clothesService.DeleteClothes(clothesID_to_delete);


            //Assert
            Assert.True(response);


        }

        #endregion

        #region GetAllSoldClothes
        //Should throw empty list if "Sold" DB is empty
        [Fact]
        public async Task GetAllSoldClothes_EmptyList()
        {
            //Act
            List<ClothesResponse> clothes_response = await _clothesService.GetAllSoldClothes();

            //Assert
            Assert.Empty(clothes_response);
        }

        //should return all sold Clothes if "Sold" DB containes entries
        [Fact]
        public async Task GetAllSoldClothes_GetProperList()
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
                clothes_responses_from_add.Add(await _clothesService.AddClothes(request));
            }



            //remove the clothes from the DB using the clothesID returned on the previous list
            for (int i = 0; i < clothes_add_request_list.Count; i++)
            {

                _clothesRepo.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(clothes_responses_from_add[i].ToClothesUpdateRequest().ToClothes());

                await _clothesService.SoldClothesByClothesID(clothes_responses_from_add[i].ClothesID);
            }

            _soldClothesRepo.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(() =>
            {
                List<SoldClothes> clothes_Response = new List<SoldClothes>();
                foreach(ClothesResponse clothes in clothes_responses_from_add)
                {
                    clothes_Response.Add(clothes.ToClothesUpdateRequest().ToClothes().ToSoldClothes());
                }
                return clothes_Response;
            });

            //Act 
            List<ClothesResponse> clothes_response_from_getAllSold = await _clothesService.GetAllSoldClothes();

            //Assert

            //loop through the response list to verify if the ClothesID (Guid) is in the list from the Sold DB (consider that the equal is overriden to simply check Guid id)
            foreach (ClothesResponse response in clothes_responses_from_add)
            {
                Assert.Contains(response, clothes_response_from_getAllSold);
            }

        }


        #endregion

        #region SoldClothesByClothesID

        //if clothesID is null, should throw argumentnullexception
        [Fact]
        public async Task SoldClothesByClothesID_NullID()
        {
            //Arrange
            Guid? ClothesID = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>( async () =>
            {
                //Act
                await _clothesService.SoldClothesByClothesID(ClothesID);
            });
        }

        //should return false if ClothesID does not exists
        [Fact]
        public async Task SoldClothesByClothesID_IDDoesNotExists()
        {
            //Arrange
            Guid clothesID = Guid.NewGuid();

            //Act
            bool response = await _clothesService.SoldClothesByClothesID(clothesID);

            //Assert
            Assert.False(response);
        }

        //should properly return Clothes response with correct ID provisded
        [Fact]
        public async Task SoldClothesByClothesID_ProperIDProvided()
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


            ClothesResponse clothes_response_from_add = await _clothesService.AddClothes(clothes_add_requerst);
            Guid clothesID = clothes_response_from_add.ClothesID;

            _clothesRepo.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(clothes_response_from_add.ToClothesUpdateRequest().ToClothes());

            //Act
            bool response = await _clothesService.SoldClothesByClothesID(clothesID);

            //Assert
            Assert.True(response);

            _soldClothesRepo.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(() =>
            {
                return  new List<SoldClothes>()
                {
                    clothes_response_from_add.ToClothesUpdateRequest().ToClothes().ToSoldClothes()
                };
            });

            //also,check if the removed ID is contained in "Sold" DB
            List<ClothesResponse> clothesResponses_from_SoldList = await  _clothesService.GetAllSoldClothes();

            Assert.Contains(clothes_response_from_add, clothesResponses_from_SoldList);


        }
        #endregion

        #region GetFilteredClothes
        //correctly filter by Gender
        [Fact]
        public async Task GetFilteredClothes_FilterByGender()
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
            clothes_responses_list_from_add.Add(await _clothesService.AddClothes(clothes_add_requerst1));
            clothes_responses_list_from_add.Add(await _clothesService.AddClothes(clothes_add_requerst2));
            clothes_responses_list_from_add.Add(await _clothesService.AddClothes(clothes_add_requerst3));

            List<ClothesResponse> filtered_clothes_responses_from_add = clothes_responses_list_from_add.Where(temp => (!string.IsNullOrEmpty(temp.Gender)) ? temp.Gender.Equals("male", StringComparison.OrdinalIgnoreCase) : true).ToList();
            _testOutputHelper.WriteLine("Expected");

            foreach (ClothesResponse response in filtered_clothes_responses_from_add)
            {
                _testOutputHelper.WriteLine(response.ToString());
            }

            _clothesRepo.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(()=>
            {
                List<Clothes> list = new List<Clothes>();

                foreach(ClothesResponse response in clothes_responses_list_from_add)
                {
                    list.Add(response.ToClothesUpdateRequest().ToClothes());
                }
                return list;
            });

            //Act
            List<ClothesResponse> clothes_responses_from_getfiltered = await _clothesService.GetFilteredClothes(nameof(Clothes.Gender), "fe");



            _testOutputHelper.WriteLine("Actual");
            foreach (ClothesResponse response in clothes_responses_from_getfiltered)
            {
                _testOutputHelper.WriteLine(response.ToString());
            }



            //Assert
            Assert.DoesNotContain(clothes_responses_list_from_add[1], clothes_responses_from_getfiltered);
            Assert.Contains(clothes_responses_list_from_add[0], clothes_responses_from_getfiltered);
            Assert.Contains(clothes_responses_list_from_add[2], clothes_responses_from_getfiltered);

        }

        //correctly filter by Model
        [Fact]
        public async Task GetFilteredClothes_filterByModel()
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
            clothes_responses_list_from_add.Add( await _clothesService.AddClothes(clothes_add_requerst1));
            clothes_responses_list_from_add.Add( await _clothesService.AddClothes(clothes_add_requerst2));
            clothes_responses_list_from_add.Add( await _clothesService.AddClothes(clothes_add_requerst3));

            _clothesRepo.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(() =>
            {
                List<Clothes> list = new List<Clothes>();
                foreach(ClothesResponse clothes in clothes_responses_list_from_add)
                {
                    list.Add(clothes.ToClothesUpdateRequest().ToClothes());
                }
                return list;
            });

            //Act
            List<ClothesResponse> clothes_responses_from_getfiltered = await _clothesService.GetFilteredClothes(nameof(Clothes.Model), "Suave");

            //Assert
            Assert.Contains(clothes_responses_list_from_add[2], clothes_responses_from_getfiltered);
            Assert.DoesNotContain(clothes_responses_list_from_add[0], clothes_responses_from_getfiltered);
            Assert.DoesNotContain(clothes_responses_list_from_add[1], clothes_responses_from_getfiltered);

        }

        //correctly get all Clothes if to filterstring is provided
        [Fact]
        public async void GetFilteredClothes_EmptySearchString()
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
            clothes_responses_list_from_add.Add( await _clothesService.AddClothes(clothes_add_requerst1));
            clothes_responses_list_from_add.Add( await _clothesService.AddClothes(clothes_add_requerst2));
            clothes_responses_list_from_add.Add( await _clothesService.AddClothes(clothes_add_requerst3));

            _clothesRepo.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(()=>
            {
                List<Clothes> list = new List<Clothes>();
                foreach(ClothesResponse clothes in clothes_responses_list_from_add)
                {
                    list.Add(clothes.ToClothesUpdateRequest().ToClothes());
                }
                return list;
            });

            //Act
            List<ClothesResponse> clothes_responses_from_getfiltered = await _clothesService.GetFilteredClothes(nameof(Clothes.Gender), "");


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
        public async Task GetSortedClothes_SortASC()
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
            clothes_responses_list_from_add.Add(await _clothesService.AddClothes(clothes_add_requerst1));
            clothes_responses_list_from_add.Add(await _clothesService.AddClothes(clothes_add_requerst2));
            clothes_responses_list_from_add.Add(await _clothesService.AddClothes(clothes_add_requerst3));

            List<ClothesResponse> sorted_clothes_responses_list_from_add = clothes_responses_list_from_add.OrderBy(temp => temp.Model).ToList();

            _clothesRepo.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(() =>
            {
                List<Clothes> list = new List<Clothes>();

                foreach(ClothesResponse response in clothes_responses_list_from_add)
                {
                    list.Add(response.ToClothesUpdateRequest().ToClothes());
                }
                return list;
            });

            //Act 
            List<ClothesResponse> sorted_clothes_from_GetSorted = await _clothesService.GetSortedClothes(clothes_responses_list_from_add, nameof(Clothes.Model), SortOrderOptions.ASC);


            //Assert
            for (int i = 0; i < sorted_clothes_responses_list_from_add.Count; i++)
            {
                Assert.Equal(sorted_clothes_responses_list_from_add[i], sorted_clothes_from_GetSorted[i]);
            }

        }

        //should get desending order of purchase prices
        [Fact]
        public async Task GetSortedClothes_SortDESC()
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
            clothes_responses_list_from_add.Add(await _clothesService.AddClothes(clothes_add_requerst1));
            clothes_responses_list_from_add.Add(await _clothesService.AddClothes(clothes_add_requerst2));
            clothes_responses_list_from_add.Add(await _clothesService.AddClothes(clothes_add_requerst3));

            List<ClothesResponse> sorted_clothes_responses_list_from_add = clothes_responses_list_from_add.OrderByDescending(temp => temp.PurchasePrice).ToList();

            _clothesRepo.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(()=>
            {
                List<Clothes> list = new List<Clothes>();

                foreach (ClothesResponse response in clothes_responses_list_from_add)
                {
                    list.Add(response.ToClothesUpdateRequest().ToClothes());
                }
                return list;
            });

            //Act 
            List<ClothesResponse> sorted_clothes_from_GetSorted = await _clothesService.GetSortedClothes(clothes_responses_list_from_add, nameof(Clothes.PurchasePrice), SortOrderOptions.DESC);


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
        public async Task ClothesUpdateRequest_ProperUpdatingOfName()
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
            ClothesResponse clothes_response_from_add = await _clothesService.AddClothes(clothes_add_requerst);

            ClothesUpdateRequest clothes_update_response = clothes_response_from_add.ToClothesUpdateRequest();
            clothes_update_response.ClothesType = ClothesTypeOptions.Ropon.ToString();

            _clothesRepo.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(clothes_response_from_add.ToClothesUpdateRequest().ToClothes());
            

            //Act
            ClothesResponse clothes_response_from_update = await _clothesService.UpdateClothes(clothes_update_response);

            ClothesResponse? clothes_from_get = await _clothesService.GetClothesByClothesID(clothes_response_from_update.ClothesID);

            //Assert
            if (clothes_from_get != null)
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
        public async Task ClothesUpdateRequest_NullUpdateRequest()
        {
            //Arrange
            ClothesUpdateRequest? clothesUpdateRequest = null;

            //Arrange
            await Assert.ThrowsAsync<ArgumentNullException>( async () =>
            {
                //Act
                await _clothesService.UpdateClothes(clothesUpdateRequest);
            });
        }
        #endregion

    }//end of class
}//end of namespace
