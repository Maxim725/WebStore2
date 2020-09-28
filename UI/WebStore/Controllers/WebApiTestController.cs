using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.TestApi;

namespace WebStore.Controllers
{
    public class WebApiTestController : Controller
    {
        private readonly IValueService _valueService;
        public WebApiTestController(IValueService valueService)
        {
            _valueService = valueService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View(_valueService.Get());
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            return View("Details",_valueService.Get(id));
        }
    }
}
