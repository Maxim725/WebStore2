using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using WebStore.Controllers;
using WebStore.Interfaces.TestApi;
using Assert = Xunit.Assert;
namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class WebApiTestControllerTest
    {
        //class TestValueService : IValueService
        //{
        //    public HttpStatusCode Delete(int id)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public IEnumerable<string> Get()
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public string Get(int id)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public Uri Post(string value)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public HttpStatusCode Update(int id, string value)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
        [TestMethod]
        public void IndexReturnsViewWithValues()
        {
            // 1-й способ
            // Можно взять сервис из реализации IValueService (но тэо уже не модульный тест)

            // 2-й способ
            // Написание своего тестового сервиса с реализацией интерфейса IValueService
            // var service = new TestValueService();
            // var controller = new WebApiTestController(service);

            // 3-й способ
            // Использование Moq объектов - заглушек

            var expectedValues = new[] { "1", "2", "3" };
            var valueServiceMock = new Mock<IValueService>();

            valueServiceMock
                .Setup(service => service.Get())
                .Returns(expectedValues);

            var controller = new WebApiTestController(valueServiceMock.Object);

            var result = controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<IEnumerable<string>>(viewResult.Model);

            Assert.Equal(expectedValues.Length, model.Count());

            // если объект просто притворяется интерфейсом, то это "Стаб" (Stab)

            valueServiceMock.Verify(service => service.Get());
            valueServiceMock.VerifyNoOtherCalls();

            // Если выполняется последующая проверка состояния, то это "Мок" Moq
        }
    }
}
