using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Products.InCookies;
using Assert = Xunit.Assert;
namespace WebStore.Service.Tests.Products
{
    [TestClass]
    public class CartServiceTests
    {
        private Cart _cart;
        private Mock<IProductData> _productDataMock;
        private Mock<ICartStore> _cartStoreMock;
        
        /// <summary>Тестируемый сервис</summary>
        private ICartService _cartService;


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
                .Returns(new PageProductsDTO
                {
                    Products = new List<ProductDTO>
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
                    },
                    TotalCount = 2
                });

            _cartStoreMock = new Mock<ICartStore>();
            _cartStoreMock.Setup(c => c.Cart).Returns(_cart);
            _cartService = new CartService(_productDataMock.Object, _cartStoreMock.Object);
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

        [TestMethod]
        public void CartServiceAddToCartWorkCorrect()
        {
            _cart.Items.Clear();

            const int expectedId = 5;


            _cartService.AddToCart(expectedId);


            Assert.Equal(1, _cart.ItemsCount);
            Assert.Single(_cart.Items);
            Assert.Equal(expectedId, _cart.Items.First().ProductId);
        }

        [TestMethod]
        public void CartServiceRemoveFromCartRemoveCorrectCount()
        {
            const int itemId = 1;

            _cartService.RemoveFromCart(itemId);
            Assert.Single(_cart.Items);
            Assert.Equal(2, _cart.Items.Single().ProductId);
        }

        [TestMethod]
        public void CartServiceClearClearCart()
        {
            _cartService.Clear();

            Assert.Empty(_cart.Items);
        }

        [TestMethod]
        public void CartServiceDecrementCorrect()
        {
            const int itemId = 2;

            _cartService.DecrementFromCart(itemId);

            Assert.Equal(3, _cart.ItemsCount);
            Assert.Equal(2, _cart.Items.Count);
            Assert.Equal(itemId, _cart.Items.Skip(1).First().ProductId);
            Assert.Equal(2, _cart.Items.Skip(1).First().Quantity);
        }

        [TestMethod]
        public void CartServiceRemoveItemWhenDecrementTo0()
        {
            const int itemId = 1;

            _cartService.DecrementFromCart(itemId);

            Assert.Equal(3, _cart.ItemsCount);

            Assert.Single(_cart.Items);
        }

        [TestMethod]
        public void CartServiceTransformToCartWorkCorrect()
        {
            var result = _cartService.TransformFromCart();

            Assert.Equal(4, result.ItemsCount);
            Assert.Equal(1.1M, result.Items.First().Product.Price);
        }
    }
}
