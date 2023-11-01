using ServicesContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services;
using ServicesContracts;
using ServicesContracts.Enums;
using Entities;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ServicesTests
{
    public class ProductServiceTests
    {
        //private fields
        private readonly IProductsServices _productsService;
        private readonly ITestOutputHelper _testOutputHelper;

        //constructor
        public ProductServiceTests(ITestOutputHelper testOutputHelper)
        {
            _productsService = new ProductService(false);
            _testOutputHelper = testOutputHelper;
        }

        //tests

        //addproduct
        #region AddProduct

        //null request object
        [Fact]
        public void AddProduct_NullRequestObject()
        {

            //Assert
            Assert.Throws<ArgumentNullException>(()=>
            {
                //Act
                _productsService.AddProduct(null);
            });
            
        }

        //Invalid props
        [Fact]
        public void AddProduct_invalidProps()
        {
            //Arrange
            ProductAddRequest request = new ProductAddRequest();


            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _productsService.AddProduct(request);
            });
        }

        //Valid adding of product
        [Fact]
        public void AddProduct_ValidAdding()
        {
            //Arrange
            ProductAddRequest productAddRequest = new ProductAddRequest()
            {
                ProductDescription = "",
                Gender = GenderOptions.Female,
                Color = "Red",
                ProductType = ProductTypeOptions.Crowns,
                Size = "large",
                Theme = ThemeOptions.XV,
                PurchasePrice = 230.00,
            };

            //Act 
            ProductResponse productResponse = _productsService.AddProduct(productAddRequest);

            //Assert
            Assert.True(productResponse.ProductID != Guid.Empty);

        }

        #endregion

        //GetAllProducts
        #region GetAllProducts
        //get empty list if no products in DB
        [Fact]
        public void GetAllProducts_EmptyList()
        {
            //Act
            List<ProductResponse> products = _productsService.GetAllProducts();

            //Assert
            Assert.Empty(products);
        }

        //add few costumes
        [Fact]
        public void GetAllProducts_GetList()
        {
            List<ProductAddRequest> request_list = new List<ProductAddRequest>()
            {
                new ProductAddRequest()
                {
                    Color = "Red",
                    Gender = GenderOptions.Male,
                    ProductDescription = "something to wear",
                    ProductType= ProductTypeOptions.Misc,
                    PurchasePrice = 233.33,
                    Size = "small",
                    Theme = ThemeOptions.Communion,
                },
                new ProductAddRequest()
                {
                    Color = "Red",
                    ProductDescription = "something to wear",
                    ProductType= ProductTypeOptions.Bow,
                    PurchasePrice = 30.00,
                    Theme = ThemeOptions.Other,
                },

            };
            List<ProductResponse> product_response_from_add = new List<ProductResponse>();

            foreach (ProductAddRequest request in request_list)
            {
                product_response_from_add.Add(_productsService.AddProduct(request));
            }

            //Act
            List<ProductResponse> product_response_from_Get = _productsService.GetAllProducts();

            //Assert
            foreach(ProductResponse  response in product_response_from_add)
            {
                Assert.Contains(response, product_response_from_Get);
            }

        }

        #endregion

        //GetProductByProductID
        #region GetProductByProductID
        //null costumeID
        [Fact]
        public void GetProductByProductID_NullID()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _productsService.SoldProductByProductID(null);
            });
        }
        //GetCostume
        [Fact]
        public void GetProductByProductID_GetProperProduct()
        {
            //Arrange 
            ProductAddRequest productAddRequest = new ProductAddRequest()
            {
                Color = "Yellow",
                Gender = GenderOptions.Female,
                ProductDescription = "Leotard",
                ProductType = ProductTypeOptions.Leotards,
                PurchasePrice = 25.00,
                Size = "Medium",
                Theme = ThemeOptions.Underware,
            };

            ProductResponse response_from_add = _productsService.AddProduct(productAddRequest);

            Guid guid = response_from_add.ProductID;

            //Act
            ProductResponse? response_from_get = _productsService.GetProductByProductID(guid);

            //Assert
            Assert.Equal(response_from_add, response_from_get);
        }
        #endregion

        //GetAllSoldProducts
        #region
        // get empty List if none in DB
        [Fact]
        public void GetAllSoldProducts_emptyList()
        {
            //Act
            List<ProductResponse> productResponses = _productsService.GetAllSoldProducts();

            //Assert
            Assert.Empty(productResponses);
        }
        //get sold costumes
        [Fact]
        public void GetAllSoldProducts_getEmptyList()
        {
            //Arrange
            List<ProductAddRequest> request_list = new List<ProductAddRequest>()
            {
                new ProductAddRequest()
                {
                    Color = "Red",
                    Gender = GenderOptions.Male,
                    ProductDescription = "something to wear",
                    ProductType= ProductTypeOptions.Misc,
                    PurchasePrice = 233.33,
                    Size = "small",
                    Theme = ThemeOptions.Communion,
                },
                new ProductAddRequest()
                {
                    Color = "Red",
                    ProductDescription = "something to wear",
                    ProductType= ProductTypeOptions.Bow,
                    PurchasePrice = 30.00,
                    Theme = ThemeOptions.Other,
                },

            };
            List<ProductResponse> product_response_from_add = new List<ProductResponse>();

            foreach (ProductAddRequest request in request_list)
            {
                product_response_from_add.Add(_productsService.AddProduct(request));
            }

            //remove from the DB (Sell)
            foreach (ProductResponse product in product_response_from_add)
            {
                _productsService.SoldProductByProductID(product.ProductID);
            }

            //Act 
            List<ProductResponse> responses_from_get = _productsService.GetAllSoldProducts();

            //Assert
            foreach(ProductResponse product in product_response_from_add)
            {
                Assert.Contains(product, responses_from_get);
            }
        }
        #endregion

        //SoldProductByProductID
        #region SoldProductByProductID
        //should Throw exceptuion if ID if null
        [Fact]
        public void SoldProductByProductID_NullID()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(()=>
            {
                //Act
                _productsService.SoldProductByProductID(null);
            });
        }

        //should be false if id does not exist
        [Fact]
        public void SoldProductByProductID_NoneExistantID()
        {
            //Arrange
            Guid id = Guid.NewGuid();

            //Act
            bool response = _productsService.SoldProductByProductID(id);

            //Assert
            Assert.False(response);
        }

        // should throw true if properly removed valid product
        [Fact]
        public void SoldProductByProductID_ProperSoldOfProduct()
        {
            //Arrange
            ProductAddRequest product_request = new ProductAddRequest()
            {
                Color = "Red",
                ProductDescription = "something to wear",
                ProductType = ProductTypeOptions.Bow,
                PurchasePrice = 30.00,
                Theme = ThemeOptions.Other,
            };

            ProductResponse response_from_add = _productsService.AddProduct(product_request);

            //Act
            bool response_from_soldProduct = _productsService.SoldProductByProductID(response_from_add.ProductID);

            //Assert
            Assert.True(response_from_soldProduct);

            //check if it is contained in the "Sold" DB
            List<ProductResponse> response_list_from_getSold = _productsService.GetAllSoldProducts();

            Assert.Contains(response_from_add, response_list_from_getSold);
        }

        #endregion

        //DeleteProduct
        #region DeleteProduct
        //should get exception if null ID
        [Fact]
        public void DeleteProduct_NullID()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(()=>
            {
                //Act
                _productsService.DeleteProduct(null);
            });
        }

        //Should throw false if ID does not exist
        [Fact]
        public void DeleteProduct_NoneExistingID()
        {
            //Arrange
            Guid guid = Guid.NewGuid();

            //Act
            bool response = _productsService.DeleteProduct(guid);

            //Assert
            Assert.False(response);

        }

        //Proper deletion of product
        [Fact]
        public void DeleteProduct_ExistingID()
        {
            //Arrange
            ProductAddRequest request = new ProductAddRequest()
            {
                Color = "Azul",
                ProductDescription = "Para la boda",
                ProductType = ProductTypeOptions.Arras,
                PurchasePrice = 450.00,
                Theme = ThemeOptions.Wedding,
            };
            ProductResponse response_from_add = _productsService.AddProduct(request);

            //Act
            bool response_bool_from_deletion = _productsService.DeleteProduct(response_from_add.ProductID);

            //Assert
            Assert.True(response_bool_from_deletion);
        }

        #endregion

        //GetFilteredProduct
        #region GetFilteredProducts



        //should properly filter products
        [Fact]
        public void GetFilteredProducts_properFiltered()
        {
            List<ProductAddRequest> request_list = new List<ProductAddRequest>()
            {
                new ProductAddRequest()
                {
                    Color = "Red",
                    Gender = GenderOptions.Male,
                    ProductDescription = "something to wear",
                    ProductType= ProductTypeOptions.Bow,
                    PurchasePrice = 233.33,
                    Size = "small",
                    Theme = ThemeOptions.Communion,
                },
                new ProductAddRequest()
                {
                    Color = "Red",
                    ProductDescription = "for hair",
                    ProductType= ProductTypeOptions.Bow,
                    PurchasePrice = 30.00,
                    Theme = ThemeOptions.Other,
                },
                new ProductAddRequest()
                {
                    Color = "Azul",
                    ProductDescription = "Para la boda",
                    ProductType = ProductTypeOptions.Arras,
                    PurchasePrice = 450.00,
                    Theme = ThemeOptions.Wedding,
                }
            };

            List<ProductResponse> response_from_add = new List<ProductResponse>();

            foreach (ProductAddRequest request in request_list)
            {
                response_from_add.Add(_productsService.AddProduct(request));
            }

            List<ProductResponse> expected_filter = response_from_add.Where(temp => (!string.IsNullOrEmpty(temp.ProductType))? temp.ProductType.Contains("Bo", StringComparison.OrdinalIgnoreCase) : true).ToList();

            _testOutputHelper.WriteLine("Expected");
            foreach (ProductResponse response in expected_filter)
            {
                _testOutputHelper.WriteLine(response.ToString());
            }



            //Act
            List<ProductResponse> response_from_Getfiltered = _productsService.GetFilteredProduct(nameof(Product.ProductType), "bo");

            _testOutputHelper.WriteLine("Actual Return: ");

            foreach(ProductResponse response in response_from_Getfiltered)
            {
                _testOutputHelper.WriteLine(response.ToString());

            }


            //Assert
            Assert.DoesNotContain(response_from_add[2], response_from_Getfiltered);
            Assert.Contains(response_from_add[0], response_from_Getfiltered);
            Assert.Contains(response_from_add[1], response_from_Getfiltered);



        }

        //should get all products if search string is empty
        [Fact]
        public void GetFilteredProducts_getAllWithEmptySearchString()
        {
            //Arrange
            List<ProductAddRequest> request_list = new List<ProductAddRequest>()
            {
                new ProductAddRequest()
                {
                    Color = "Red",
                    Gender = GenderOptions.Male,
                    ProductDescription = "something to wear",
                    ProductType= ProductTypeOptions.Bow,
                    PurchasePrice = 233.33,
                    Size = "small",
                    Theme = ThemeOptions.Communion,
                },
                new ProductAddRequest()
                {
                    Color = "Red",
                    ProductDescription = "for hair",
                    ProductType= ProductTypeOptions.Bow,
                    PurchasePrice = 30.00,
                    Theme = ThemeOptions.Other,
                },
                new ProductAddRequest()
                {
                    Color = "Azul",
                    ProductDescription = "Para la boda",
                    ProductType = ProductTypeOptions.Arras,
                    PurchasePrice = 450.00,
                    Theme = ThemeOptions.Wedding,
                }
            };

            List<ProductResponse> response_from_add = new List<ProductResponse>();

            foreach (ProductAddRequest request in request_list)
            {
                response_from_add.Add(_productsService.AddProduct(request));
            }

            //Act
            List<ProductResponse> response_from_Getfiltered = _productsService.GetFilteredProduct(nameof(Product.ProductType), "");

            //Assert
            foreach(ProductResponse response in response_from_add)
            {
                Assert.Contains(response, response_from_Getfiltered);
            }
        }
        #endregion

        //GetSortedProducts
        #region
        //correctlty returns a sorted List of products
        [Fact]
        public void GetSortedProducts_PropertSorting()
        {
            //Arrange
            List<ProductAddRequest> request_list = new List<ProductAddRequest>()
            {
                new ProductAddRequest()
                {
                    Color = "Red",
                    Gender = GenderOptions.Male,
                    ProductDescription = "something to wear",
                    ProductType= ProductTypeOptions.Bow,
                    PurchasePrice = 233.33,
                    Size = "small",
                    Theme = ThemeOptions.Communion,
                },
                new ProductAddRequest()
                {
                    Color = "Yello",
                    ProductDescription = "for hair",
                    ProductType= ProductTypeOptions.Bow,
                    PurchasePrice = 30.00,
                    Theme = ThemeOptions.Other,
                },
                new ProductAddRequest()
                {
                    Color = "Blue",
                    ProductDescription = "Para la boda",
                    ProductType = ProductTypeOptions.Arras,
                    PurchasePrice = 450.00,
                    Theme = ThemeOptions.Wedding,
                }
            };

            List<ProductResponse> response_from_add = new List<ProductResponse>();

            foreach (ProductAddRequest request in request_list)
            {
                response_from_add.Add(_productsService.AddProduct(request));
            }

            List<ProductResponse> productResponses_ordered = response_from_add.OrderBy(temp => temp.Color).ToList();

            //Act
            List<ProductResponse> productResponses_ordered_from_get = _productsService.GetSortedProducts(response_from_add, nameof(Product.Color), SortOrderOptions.ASC);

            //Assert
            for(int i = 0; i< productResponses_ordered.Count; i++)
            {
                Assert.Equal(productResponses_ordered[i], productResponses_ordered_from_get[i]);
            }
        }
        #endregion

        //UpdateProduct
        #region UpdateProduct
        //should throw error from if update object is null
        [Fact]
        public void UpdateProduct_NullUpdateObject()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(()=>
            {
                //Act
                _productsService.UpdateProduct(null);
            });
        }

        //properly update a producty
        [Fact]
        public void UpdateProduct_UpdateProduct()
        {
            //Arrange
            ProductAddRequest request = new ProductAddRequest()
            {
                Color = "Blue",
                ProductDescription = "Para la boda",
                ProductType = ProductTypeOptions.Arras,
                PurchasePrice = 450.00,
                Theme = ThemeOptions.Wedding,
            };


            ProductResponse response_from_add = _productsService.AddProduct(request);
            _testOutputHelper.WriteLine(response_from_add.ToString());

            ProductUpdateRequest productUpdateRequest = response_from_add.ToProductUpdateRequest();

            productUpdateRequest.Color = "Red";
            productUpdateRequest.ProductDescription = "";
            _testOutputHelper.WriteLine(productUpdateRequest.ToString());

            //Act
            ProductResponse product_response_from_update = _productsService.UpdateProduct(productUpdateRequest);

            //Assert
            _testOutputHelper.WriteLine($"{product_response_from_update.Color}");

            Assert.Equal("Red", product_response_from_update.Color);
            Assert.Equal("", product_response_from_update.ProductDescription);
        }
        #endregion
    }
}
