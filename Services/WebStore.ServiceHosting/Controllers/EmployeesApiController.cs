using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    //[Route("api/[controller]")] // http://localhost:5001/api/employeesapi
    [Route("api/employees")]      // http://localhost:5001/api/employees
    [ApiController]
    [Produces("application/json")]
    public class EmployeesApiController : ControllerBase, IEmployeesData
    {
        private readonly IEmployeesData _employeesData;
        public EmployeesApiController(IEmployeesData employeesData)
        {
            _employeesData = employeesData;
        }

        [HttpGet] // GET http://localhost:5001/api/employees
        //[HttpGet("all")] // GET http://localhost:5001/api/employees/all
        public IEnumerable<Employee> Get()
        {
            return _employeesData.Get();
        }

        [HttpGet("{id}")]
        public Employee GetById(int id)
        {
            return _employeesData.GetById(id);
        }

        [HttpPost]
        public int Add(Employee employee)
        {
            var result = _employeesData.Add(employee);
            SaveChanges();
            return result;
        }

        [HttpPut]
        public void Edit(Employee employee)
        {
            _employeesData.Edit(employee);
            SaveChanges();
        }

        [HttpDelete("{id}")] // DELETE http://localhost:5001/api/employees/all/5
        [HttpDelete("delete/{id}")] // DELETE http://localhost:5001/api/employees/all/delete/5
        public bool Delete(int id)
        {
            var result = _employeesData.Delete(id);
            SaveChanges();
            return result;
        }

        //[NonAction]
        public void SaveChanges()
        {
            _employeesData.SaveChanges();
        }
    }
}
