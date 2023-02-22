using EmployeeManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.DAL.Interfaces
{
    public interface IEmployeeRepository
    {
        public Task<List<Employee>> GetEmployees();
        public Task<Employee> GetEmployeeById(int id);
        public Task<List<Employee>> SearchEmployees(string keyword);

        public Task<APIResponse> AddEmployee(Employee employee);

        public Task<APIResponse> UpdateEmployee(Employee employee);

        public Task<APIResponse> DeleteEmployee(int employeeId);


    }
}
