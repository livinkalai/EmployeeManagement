using EmployeeManagement.DAL.DBContext;
using EmployeeManagement.DAL.Interfaces;
using EmployeeManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.DAL.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public async Task<List<Employee>> GetEmployees()
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                using (var context = new EmployeeDbContext())
                {
                    employees = await context.Employees.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return employees;
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            Employee employee = new Employee();
            try
            {
                using (var context = new EmployeeDbContext())
                {
                    employee = await context.Employees.Where(x => x.Id == id).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return employee;
        }

        public async Task<List<Employee>> SearchEmployees(string keyword)
        {
            List<Employee> employees = new List<Employee>();


            if (keyword != null && keyword.Trim() != "")
            {
                try
                {
                    using (var context = new EmployeeDbContext())
                    {
                        keyword = keyword.ToLower();

                        employees = await context.Employees.Where(x => x.Name.ToLower().Contains(keyword) ||
                            x.DOB.ToString("MM/dd/yyyy").ToLower().Contains(keyword) ||
                            x.Department.ToLower().Contains(keyword) ||
                            x.Email.ToLower().Contains(keyword)).ToListAsync();

                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return employees;
        }


        public async Task<APIResponse> AddEmployee(Employee employee)
        {
            APIResponse resp = new APIResponse();
            try
            {
                using (var context = new EmployeeDbContext())
                {
                    Employee existEmployee = await context.Employees.Where(x => x.Email.ToLower() == employee.Email.ToLower()).FirstOrDefaultAsync();

                    if (existEmployee != null)
                    {
                        resp.ResponseCode = 400;
                        resp.Message = "An employee already exists with this email !";
                    }
                    else if (employee.Id == 0)
                    {
                        context.Employees.Add(employee);
                        await context.SaveChangesAsync();
                        resp.ResponseCode = 200;
                        resp.Message = "Employee added successfully";
                        resp.Data = employee;
                    }
                    else
                    {
                        resp.ResponseCode = 400;
                        resp.Message = "Failed to add the employee!";
                    }
                }
            }
            catch (Exception ex)
            {
                resp.ResponseCode = 400;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public async Task<APIResponse> UpdateEmployee(Employee employee)
        {
            APIResponse resp = new APIResponse();

            try
            {
                using (var context = new EmployeeDbContext())
                {
                    Employee employeeToUpdate = await GetEmployeeById(employee.Id);
                    if (employeeToUpdate != null)
                    {
                        context.Employees.Attach(employee);
                        context.Entry(employee).State = EntityState.Modified;
                        //context.Employees.Update(employeeToUpdate);
                        await context.SaveChangesAsync();
                        resp.Message = "Employee updated successfully";
                        resp.ResponseCode = 200;
                        resp.Data = employee;
                        return resp;
                    }
                    else
                    {
                        resp.Message = "Employee id is not found!";
                        resp.ResponseCode = 400;
                    }
                }
            }
            catch (Exception ex)
            {
                resp.Message = ex.Message;
                resp.ResponseCode = 400;
            }
            return resp;
        }

        public async Task<APIResponse> DeleteEmployee(int employeeId)
        {
            APIResponse resp = new APIResponse();

            try
            {
                using (var context = new EmployeeDbContext())
                {
                    Employee employee = await GetEmployeeById(employeeId);
                    if (employee != null)
                    {
                        context.Attach(employee);
                        context.Employees.Remove(employee);
                        await context.SaveChangesAsync();
                        resp.Message = "Employee deleted successfully";
                        resp.ResponseCode = 200;
                    }
                    else
                    {
                        resp.Message = "Employee id is not found!";
                        resp.ResponseCode = 400;
                    }

                }
            }
            catch (Exception ex)
            {
                resp.ResponseCode = 400;
                resp.Message = ex.Message;
            }
            return resp;
        }
    }
}
