using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebStore.Controllers;
using WebStore.Domain.DTO.Orders;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class CartControllerTests
    {
        [TestMethod]
        public async Task CheckOutModelStateInvalidReturnsViewModel()
        {
            var cartserviceMock = new Mock<ICartService>();
            var orderServiceMock = new Mock<IOrderService>();

            var controller = new CartController(cartserviceMock.Object);
            controller.ModelState.AddModelError("error", "InvalidModel");

            const string expectedModelName = "Test order";

            var orderViewModel = new OrderViewModel() { Name = expectedModelName };
            var result = await controller.CheckOut(orderViewModel, orderServiceMock.Object);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CartOrderViewModel>(viewResult.Model);

            Assert.Equal(expectedModelName, model.Order.Name);
        }

        [TestMethod]
        public async Task CheckoutCallsServiceAndReturnsRedirect()
        {
            var cartServiceMock = new Mock<ICartService>();

            cartServiceMock
                .Setup(s => s.TransformFromCart())
                .Returns(() => new CartViewModel
                {
                    Items = new[] { (new ProductViewModel { Name = "Product" }, 1) }
                });

            var orderServiceMock = new Mock<IOrderService>();
            const int expectedOrderid = 1;
            orderServiceMock
               .Setup(c => c.CreateOrder(It.IsAny<string>(), It.IsAny<CreateOrderModel>()))
               .ReturnsAsync(new OrderDTO
               {
                   Id = expectedOrderid
               });

            var controller = new CartController(cartServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(
                            new ClaimsIdentity(new[]
                            {
                                new Claim(ClaimTypes.NameIdentifier, "TestUser")
                            }))
                    }
                }
            };

            var orderViewModel = new OrderViewModel
            {
                Name = "Test order",
                Address = "Test address",
                Phone = "+1(234)567-89-00"
            };

            var result = await controller.CheckOut(orderViewModel, orderServiceMock.Object);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Null(redirectResult.ControllerName);
            Assert.Equal(nameof(controller.OrderConfirmed), redirectResult.ActionName);

            Assert.Equal(expectedOrderid, redirectResult.RouteValues["id"]);
        }
    }
}
