using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using WebStore.Clients.Base;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Employees
{
    class EmployeesClient : BaseClient, IEmployeesData
    {
        public EmployeesClient(IConfiguration configuration) : base(configuration, "api/employees")
        { }


        public IEnumerable<Employee> Get() => Get<IEnumerable<Employee>>(_serviceAddress);

        public Employee GetById(int id) => Get<Employee>($"{_serviceAddress}/{id}");
        public int Add(Employee employee) => Post(_serviceAddress, employee).Content.ReadAsAsync<int>().Result;


        public void Edit(Employee employee) => Put(_serviceAddress, employee);
        public bool Delete(int id) => Delete($"{_serviceAddress}/{id}").IsSuccessStatusCode;

        public void SaveChanges()
        {
        }
    }
}
