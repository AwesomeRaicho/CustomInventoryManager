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
using Moq;
using Entities.Sold;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace ServicesTests
{
    public class ProductServiceTests
    {
        //private fields
        private readonly ITestOutputHelper _testOutputHelper;

        private Mock<IRepository<Product>> _productRepo;
        private Mock<IRepository<SoldProduct>> _soldProductRepo;
        private ProductService _productsService;
        private TestDataSeeder _dataSeeder;

        //constructor
        public ProductServiceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _productRepo = new Mock<IRepository<Product>>();
            _soldProductRepo = new Mock<IRepository<SoldProduct>>();
            _productsService = new ProductService(_productRepo.Object, _soldProductRepo.Object);
            _dataSeeder = new TestDataSeeder();
        }

        //tests

        //addproduct
        #region AddProduct

        //null request object
        [Fact]
        public async Task AddProduct_NullRequestObject()
        {
            //Arrange





            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await _productsService.AddProduct(null);
            });

        }

        //Invalid props
        [Fact]
        public async void AddProduct_invalidProps()
        {
            //Arrange
            ProductAddRequest request = new ProductAddRequest();

            //Assert
            await Assert.ThrowsAsync<ArgumentException>( async () =>
            {
                //Act
                await _productsService.AddProduct(request);
            });
        }

        //Valid adding of product
        [Fact]
        public async Task AddProduct_ValidAdding()
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

            _productRepo.Setup(x => x.Add(productAddRequest.ToProduct()));

            //Act 
            ProductResponse productResponse = await _productsService.AddProduct(productAddRequest);

            //Assert
            Assert.True(productResponse.ProductID != Guid.Empty);

        }

        #endregion

        //GetAllProducts
        #region GetAllProducts
        //get empty list if no products in DB
        [Fact]
        public async Task GetAllProducts_EmptyList()
        {
            //Arrange
            _productRepo.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new List<Product>());

            //Act
            List<ProductResponse> products = await _productsService.GetAllProducts();

            //Assert
            Assert.Empty(products);
        }

        //add few costumes
        [Fact]
        public async Task GetAllProducts_GetList()
        {
            //Arrange
            IEnumerable<Product> products = _dataSeeder.GetProducts();

            _productRepo.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(products);

            List<ProductResponse> responses_from_add = new List<ProductResponse>();

            foreach (var product in products)
            {
                responses_from_add.Add(product.ToProductResponse());
            }

            //Act
            List<ProductResponse> product_response_from_Get = await _productsService.GetAllProducts();

            //Assert
            Assert.Equal(responses_from_add, product_response_from_Get);

        }

        #endregion

        //GetProductByProductID
        #region GetProductByProductID
        //null costumeID
        [Fact]
        public async Task GetProductByProductID_NullID()
        {
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await _productsService.SoldProductByProductID(null);
            });
        }
        //GetCostume
        [Fact]
        public async Task GetProductByProductID_GetProperProduct()
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

            Product product = productAddRequest.ToProduct();
            product.ProductID = Guid.NewGuid();

            _productRepo.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(product);

            //Act
            ProductResponse? response_from_get = await _productsService.GetProductByProductID(product.ProductID);

            //Assert
            Assert.Equal(product.ToProductResponse(), response_from_get);
        }
        #endregion

        //GetAllSoldProducts
        #region GetAllSoldProducts
        // get empty List if none in DB
        [Fact]
        public async Task GetAllSoldProducts_emptyList()
        {
            //Act
            List<ProductResponse> productResponses = await _productsService.GetAllSoldProducts();

            //Assert
            Assert.Empty(productResponses);
        }
        //get sold costumes
        [Fact]
        public async Task GetAllSoldProducts_getList()
        {
            //Arrange
            IEnumerable<Product> repoProducts = _dataSeeder.GetProducts();

            List<SoldProduct> repoSoldProducts = new List<SoldProduct>();

            List<ProductResponse> expected_to_compare = new List<ProductResponse>();

            foreach (var repoProduct in repoProducts)
            {
                expected_to_compare.Add(repoProduct.ToProductResponse());
            }

            foreach (Product request in repoProducts)
            {
                repoSoldProducts.Add(request.ToSoldProduct());
            }

            _soldProductRepo.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(repoSoldProducts);

            //Act 
            List<ProductResponse> responses_from_get = await _productsService.GetAllSoldProducts();

            //Assert
            Assert.Equal(expected_to_compare, responses_from_get);
        }
        #endregion

        //SoldProductByProductID
        #region SoldProductByProductID
        //should Throw exceptuion if ID if null
        [Fact]
        public async Task SoldProductByProductID_NullID()
        {
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>( async() =>
            {
                //Act
                await _productsService.SoldProductByProductID(null);
            });
        }

        //should be false if id does not exist
        [Fact]
        public async Task SoldProductByProductID_NoneExistantID()
        {
            //Arrange
            Guid id = Guid.NewGuid();

            //Act
            bool response = await _productsService.SoldProductByProductID(id);

            //Assert
            Assert.False(response);
        }

        // should throw true if properly removed valid product
        [Fact]
        public async Task SoldProductByProductID_ProperSoldOfProduct()
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

            ProductResponse response_from_add = await _productsService.AddProduct(product_request);

            _productRepo.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(product_request.ToProduct());

            //Act
            bool response_from_soldProduct = await _productsService.SoldProductByProductID(response_from_add.ProductID);

            //Assert
            Assert.True(response_from_soldProduct);
        }

        #endregion

        //DeleteProduct
        #region DeleteProduct
        //should get exception if null ID
        [Fact]
        public async Task DeleteProduct_NullID()
        {
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>( async () =>
            {
                //Act
                await _productsService.DeleteProduct(null);
            });
        }

        //Should throw false if ID does not exist
        [Fact]
        public async Task DeleteProduct_NoneExistingID()
        {
            //Arrange
            Guid guid = Guid.NewGuid();

            //Act
            bool response = await _productsService.DeleteProduct(guid);

            //Assert
            Assert.False(response);

        }

        //Proper deletion of product
        [Fact]
        public async Task DeleteProduct_ExistingID()
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

            Product product = request.ToProduct();
            product.ProductID = Guid.NewGuid();
            
            _productRepo.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(product);


            //Act
            bool response_bool_from_deletion = await _productsService.DeleteProduct(product.ProductID);

            //Assert
            Assert.True(response_bool_from_deletion);
        }

        #endregion

        //GetFilteredProduct
        #region GetFilteredProducts

        //should properly filter products
        [Fact]
        public async Task GetFilteredProducts_properFiltered()
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
            List<Product> repoReturn = new List<Product>();

            foreach (ProductAddRequest request in request_list)
            {
                response_from_add.Add(request.ToProduct().ToProductResponse());
            }
            foreach (ProductAddRequest request in request_list)
            {
                repoReturn.Add(request.ToProduct());
            }


            List<ProductResponse> expected_filter = response_from_add.Where(temp => (!string.IsNullOrEmpty(temp.ProductType)) ? temp.ProductType.Contains("Bo", StringComparison.OrdinalIgnoreCase) : true).ToList();

            _testOutputHelper.WriteLine("Expected");
            foreach (ProductResponse response in expected_filter)
            {
                _testOutputHelper.WriteLine(response.ToString());
            }

            _productRepo.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(repoReturn);

            //Act
            List<ProductResponse> response_from_Getfiltered = await _productsService.GetFilteredProduct(nameof(Product.ProductType), "bo");

            _testOutputHelper.WriteLine("Actual Return: ");

            foreach (ProductResponse response in response_from_Getfiltered)
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
        public async Task GetFilteredProducts_getAllWithEmptySearchString()
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
            List<Product> repoResponse = new List<Product>();


            foreach (ProductAddRequest request in request_list)
            {
                response_from_add.Add(request.ToProduct().ToProductResponse());
            }
            foreach (ProductAddRequest request in request_list)
            {
                repoResponse.Add(request.ToProduct());
            }

            _productRepo.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(repoResponse);


            //Act
            List<ProductResponse> response_from_Getfiltered = await  _productsService.GetFilteredProduct(nameof(Product.ProductType), "");

            //Assert
            foreach (ProductResponse response in response_from_add)
            {
                Assert.Contains(response, response_from_Getfiltered);
            }
        }
        #endregion

        //GetSortedProducts
        #region
        //correctlty returns a sorted List of products
        [Fact]
        public async Task GetSortedProducts_PropertSorting()
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
            List<Product> repoResponse = new List<Product>();


            foreach (ProductAddRequest request in request_list)
            {
                response_from_add.Add(request.ToProduct().ToProductResponse());
            }
            foreach (ProductAddRequest request in request_list)
            {
                repoResponse.Add(request.ToProduct());
            }

            

            List<ProductResponse> productResponses_ordered = response_from_add.OrderBy(temp => temp.Color).ToList();

            //Act
            List<ProductResponse> productResponses_ordered_from_get = await _productsService.GetSortedProducts(response_from_add, nameof(Product.Color), SortOrderOptions.ASC);

            //Assert
            for (int i = 0; i < productResponses_ordered.Count; i++)
            {
                Assert.Equal(productResponses_ordered[i], productResponses_ordered_from_get[i]);
            }
        }
        #endregion

        //UpdateProduct
        #region UpdateProduct
        //should throw error from if update object is null
        [Fact]
        public async Task UpdateProduct_NullUpdateObject()
        {
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>( async () =>
            {
                //Act
                await _productsService.UpdateProduct(null);
            });
        }

        //properly update a producty
        [Fact]
        public async Task UpdateProduct_UpdateProduct()
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

            Product repoProduct = request.ToProduct();
            repoProduct.ProductID = Guid.NewGuid();

            ProductResponse response_from_add = request.ToProduct().ToProductResponse();

            _testOutputHelper.WriteLine(response_from_add.ToString());

            ProductUpdateRequest productUpdateRequest = response_from_add.ToProductUpdateRequest();

            productUpdateRequest.Color = "Red";
            productUpdateRequest.ProductDescription = "";
            _testOutputHelper.WriteLine(productUpdateRequest.ToString());

            _productRepo.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(repoProduct);

            //Act
            ProductResponse product_response_from_update = await _productsService.UpdateProduct(productUpdateRequest);

            //Assert
            _testOutputHelper.WriteLine($"{product_response_from_update.Color}");

            Assert.Equal("Red", product_response_from_update.Color);
            Assert.Equal("", product_response_from_update.ProductDescription);
        }
        #endregion
    }
}
