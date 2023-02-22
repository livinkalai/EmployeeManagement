using EmployeeManagement.DAL.Interfaces;
using EmployeeManagement.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepo;
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepo = employeeRepository;
        }

        [HttpGet]
        [Route("GetEmployees")]
        [Authorize(Roles = "Api.Read")]
        public async Task<List<Employee>> GetEmployees()
        {
            var employees = await _employeeRepo.GetEmployees();
            return employees;
        }

        [HttpGet]
        [Route("GetEmployeeById/{id}")]
        [Authorize(Roles = "Api.Read")]
        public async Task<Employee> GetEmployeeById(int id)
        {
            var employee = await _employeeRepo.GetEmployeeById(id);
            return employee;
        }

        [HttpPost]
        [Route("AddEmployee")]
        [Authorize(Roles = "Api.Write")]
        public async Task<APIResponse> AddEmployee(Employee employee)
        {
            APIResponse response = await _employeeRepo.AddEmployee(employee);
            return response;
        }

        [HttpPut]
        [Route("UpdateEmployee")]
        [Authorize(Roles = "Api.Write")]
        public async Task<APIResponse> UpdateEmployee(Employee employee)
        {
            APIResponse response = await _employeeRepo.UpdateEmployee(employee);
            return response;
        }

        [HttpDelete]
        [Route("DeleteEmployee/{id}")]
        [Authorize(Roles = "Api.Write")]
        public async Task<APIResponse> DeleteEmployee(int id)
        {
            APIResponse response = await _employeeRepo.DeleteEmployee(id);
            return response;
        }

        [HttpGet]
        [Route("SearchEmployees")]
        [Authorize(Roles = "Api.Read")]
        public async Task<List<Employee>> SearchEmployees(string keyword)
        {
            var employees = await _employeeRepo.SearchEmployees(keyword);
            return employees;
        }
    }
}
