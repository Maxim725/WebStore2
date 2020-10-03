using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;
namespace WebStore.Service.Tests.Products
{
    [TestClass]
    public class CartServiceTests
    {
        private Cart _cart;
        private Mock<IProductData> _productDataMock;
        //private Mock<ICartStore> _cartStoreMock;

        [TestInitialize]
        public void Initialize()
        {
            _cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem { ProductId = 1, Quantity = 1 },
                    new CartItem { ProductId = 2, Quantity = 3 }
                }
            };

            _productDataMock = new Mock<IProductData>();
            _productDataMock
               .Setup(c => c.GetProducts(It.IsAny<ProductFilter>()))
               .Returns(new List<ProductDTO>
                {
                    new ProductDTO
                    {
                        Id = 1,
                        Name = "Product 1",
                        Price = 1.1m,
                        Order = 0,
                        ImageUrl = "Product1.png",
                        Brand = new BrandDTO { Id = 1, Name = "Brand 1" },
                        Section = new SectionDTO { Id = 1, Name = "Section 1"}
                    },
                    new ProductDTO
                    {
                        Id = 2,
                        Name = "Product 2",
                        Price = 2.2m,
                        Order = 0,
                        ImageUrl = "Product2.png",
                        Brand = new BrandDTO { Id = 2, Name = "Brand 2" },
                        Section = new SectionDTO { Id = 2, Name = "Section 2"}
                    },
                });

            //_cartStoreMock = new Mock<ICartStore>();
            //_cartStoreMock.Setup(c => c.Cart).Returns(_cart);
            //_cartStoreMock = new CartService(_productDataMock.Object, _cartStoreMock.Object);
        }

        [TestMethod]
        public void CartClassItemsCountReturnsCorrectQuantity()
        {
            var cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem { ProductId = 1, Quantity = 1},
                    new CartItem { ProductId = 2, Quantity = 3}
                }
            };

            const int expectedCount = 4;

            var actualCount = cart.ItemsCount;

            Assert.Equal(expectedCount, actualCount);
        }

        [TestMethod]
        public void CartViewModelReturnsCorrectItemsCount()
        {
            var cartVM = new CartViewModel
            {
                Items = new[]
                {
                    ( new ProductViewModel { Id = 1, Name = "Product 1", Price = 0.3m}, 1),
                    ( new ProductViewModel { Id = 2, Name = "Product 2", Price = 0.3m}, 3)
                }
            };

            const int expectedCount = 4;

            var actualCount = cartVM.ItemsCount;

            Assert.Equal(expectedCount, actualCount);
        }
    }
}
