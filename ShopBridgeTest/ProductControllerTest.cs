using AutoMapper;
using ShopBridgeAPI.Controllers;
using ShopBridgeAPI.Models;
using System.Collections.Generic;
using Xunit;
using Assert = Xunit.Assert;
using Moq;
using ShopBridgeAPI.Repository.IRepository;
using System.Linq;
using ShopBridgeAPI.ShopBridgeMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ShopBridgeAPI.Models.Dtos;

namespace ShopBridgeUnitTest
{
    public class ProductControllerTest
    {
        private readonly IMapper _mapper;
        private readonly static int id = 0;

        public ProductControllerTest()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new ShopBridgeMappings());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }
        [Fact]
        public void Test_GET_AllProducts()
        {
            // Arrange
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(repo => repo.GetProducts()).Returns(Multiple());
            var controller = new ProductsController(mockRepo.Object, _mapper);

            // Act
            var result = controller.GetProducts() as ObjectResult;
            var actualResult = result.Value;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)result.StatusCode);
            Assert.Equal(3, (actualResult as ICollection<ProductDto>).Count());
        }

        private static ICollection<Product> Multiple()
        {
            var r = new List<Product>
            {
                new Product()
                {
                    Id = 1,
                    Name = "Test One",
                    Description = "Description1"
                },
                new Product()
                {
                    Id = 2,
                    Name = "Test Two",
                    Description = "Description2"
                },
                new Product()
                {
                    Id = 3,
                    Name = "Test Three",
                    Description = "Description3"
                }
            };
            return r;
        }

        [Fact]
        public void Test_GET_AProduct_NotFound()
        {
            // Arrange            
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(repo => repo.GetProduct(It.IsAny<int>())).Returns<int>((a) => Single(a));
            var controller = new ProductsController(mockRepo.Object, _mapper);

            // Act
            var result = controller.GetProduct(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Test_GET_AProduct_Ok()
        {
            // Arrange            
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(repo => repo.GetProduct(It.IsAny<int>())).Returns<int>((a) => Single(a));
            var controller = new ProductsController(mockRepo.Object, _mapper);

            // Act
            var result = controller.GetProduct(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        private static Product Single(int id)
        {
            IEnumerable<Product> products = Multiple();
            return products.Where(a => a.Id == id).FirstOrDefault();
        }

        [Fact]
        public void Test_POST_AddProduct()
        {
            // Arrange
            ProductDto product = new ProductDto()
            {
                Id = 4,
                Name = "Test Four",
                Description = "description4"
            };
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(repo => repo.CreateProduct(It.IsAny<Product>())).Returns(true);
            var controller = new ProductsController(mockRepo.Object, _mapper);

            // Act
            var result = controller.CreateProduct(product);

            // Assert
            Assert.IsType<CreatedAtRouteResult>(result);
        }

        [Fact]
        public void Test_POST_AddProduct_BadRequest()
        {
            // Arrange
            ProductDto product = new ProductDto();
            product = null;
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(repo => repo.CreateProduct(It.IsAny<Product>())).Returns(true);
            var controller = new ProductsController(mockRepo.Object, _mapper);

            // Act
            var result = controller.CreateProduct(product);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Test_PUT_UpdateProduct()
        {
            // Arrange
            ProductDto product = new ProductDto()
            {
                Id = 3,
                Name = "new name",
                Description = "description3"
            };
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(repo => repo.UpdateProduct(It.IsAny<Product>())).Returns(true);
            var controller = new ProductsController(mockRepo.Object, _mapper);

            // Act
            var result = controller.UpdateProduct(product.Id, product);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Test_PUT_UpdateProduct_BadRequest()
        {
            // Arrange
            ProductDto product = new ProductDto();
            product = null;
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(repo => repo.UpdateProduct(It.IsAny<Product>())).Returns(true);
            var controller = new ProductsController(mockRepo.Object, _mapper);

            // Act
            var result = controller.UpdateProduct(id, product);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Test_DELETE_Product_NotFound()
        {
            // Arrange
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(repo => repo.DeleteProduct(It.IsAny<Product>())).Returns(true);
            var controller = new ProductsController(mockRepo.Object, _mapper);

            // Act
            var result = controller.DeleteProduct(3);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
