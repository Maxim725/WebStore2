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
    /// <summary>
    /// API управления сотрудниками
    /// </summary>
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

        /// <summary>
        /// Получение всех доступных сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet] // GET http://localhost:5001/api/employees
        //[HttpGet("all")] // GET http://localhost:5001/api/employees/all
        public IEnumerable<Employee> Get()
        {
            return _employeesData.Get();
        }


        /// <summary>
        /// Получение сотрудника по Id
        /// </summary>
        /// <param name="id">Id сотрудника</param>
        /// <returns>Найденный сотрудник</returns>
        [HttpGet("{id}")]
        public Employee GetById(int id)
        {
            return _employeesData.GetById(id);
        }

        /// <summary>
        /// Добавление сотрудника
        /// </summary>
        /// <param name="employee">Новый сотрудник</param>
        /// <returns>Идентификатор добавленного сотрудника</returns>
        [HttpPost]
        public int Add(Employee employee)
        {
            var result = _employeesData.Add(employee);
            SaveChanges();
            return result;
        }


        /// <summary>
        /// Редактирование сотрудника
        /// </summary>
        /// <param name="employee">Сотрудник с изменёнными данными</param>
        [HttpPut]
        public void Edit(Employee employee)
        {
            _employeesData.Edit(employee);
            SaveChanges();
        }

        /// <summary>
        /// Удаление сотрудника
        /// </summary>
        /// <param name="id">Идентификатор сотрудника</param>
        /// <returns>Флаг успеха удаления сотрудника</returns>
        [HttpDelete("{id}")] // DELETE http://localhost:5001/api/employees/all/5
        [HttpDelete("delete/{id}")] // DELETE http://localhost:5001/api/employees/all/delete/5
        public bool Delete(int id)
        {
            var result = _employeesData.Delete(id);
            SaveChanges();
            return result;
        }

        // Если не указывать, то Swagger не будет работать, т. к. неоднозначный запрос к методу, вот
        [NonAction]
        public void SaveChanges()
        {
            _employeesData.SaveChanges();
        }
    }
}
