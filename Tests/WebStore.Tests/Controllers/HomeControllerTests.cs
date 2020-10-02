using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Bson;
using System;
using System.Data;
using WebStore.Controllers;

using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void IndexReturnsView()
        {
            var controller = new HomeController();

            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void BlogsReturnsView()
        {
            var controller = new HomeController();

            var result = controller.Blogs();

            Assert.IsType<ViewResult>(result);
        }
        [TestMethod]
        public void BlogSingleReturnsView()
        {
            var controller = new HomeController();

            var result = controller.BlogSingle();

            Assert.IsType<ViewResult>(result);
        }
        [TestMethod]
        public void ContactUsReturnsView()
        {
            var controller = new HomeController();

            var result = controller.ContactUs();

            Assert.IsType<ViewResult>(result);
        }
        [TestMethod]
        public void Error404ReturnsView()
        {
            var controller = new HomeController();

            var result = controller.Error404();

            Assert.IsType<ViewResult>(result);
        }
        [TestMethod, ExpectedException(typeof(ApplicationException))]
        public void ThrowThrownApplicationException()
        {
            // Можно с помощью атрибутов настраивать вывод успешного теста 
            // при определённом исключении

            var controller = new HomeController();

            const string id = "Test Value";
            var result = controller.Throw("");


            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void ThrowThrownApplicationException_2()
        {
            // Можно с помощью атрибутов настраивать вывод успешного теста 
            // при определённом исключении

            var controller = new HomeController();

            const string id = "Test Value";

            var expectedMessage = $"Исключение: {id}";

            var ex = Assert.Throws<ApplicationException>(() => controller.Throw(id));

            var actualMessage = ex.Message;

            Assert.Equal(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void ErrorStatus404RedirectToError404()
        {
            var controller = new HomeController();
            const string statusCode404 = "404";

            var result = controller.ErrorStatus(statusCode404);

            //Assert.NotNull(result);

            var redirectToAction = Assert.IsType<RedirectToActionResult>(result);

            const string expectedMethodName = nameof(HomeController.Error404);
            Assert.Equal(expectedMethodName, redirectToAction.ActionName);
            Assert.Null(redirectToAction.ControllerName);
        }
    }
}
