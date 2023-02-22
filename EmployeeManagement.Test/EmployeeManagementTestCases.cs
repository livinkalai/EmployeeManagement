using EmployeeManagement.API.Controllers;
using EmployeeManagement.DAL.Interfaces;
using EmployeeManagement.DAL.Models;
using EmployeeManagement.DAL.Repository;

namespace EmployeeManagement.Test
{
    [TestFixture]
    public class Tests
    {
        private IEmployeeRepository employeeRepository;
        [SetUp]
        public void Setup()
        {
            employeeRepository = new EmployeeRepository();
        }

        [Test]
        public async Task TestAddEmployee()
        {
            var employeeController = new EmployeeController(employeeRepository);
            var employee = new Employee
            {
                Name = "Test case user 1",
                Department = "Department 1",
                Email = "email1@emp.com",
                DOB = new DateTime(1990, 04, 14)
            };
            APIResponse resp = await employeeController.AddEmployee(employee);
            if (resp != null && resp.ResponseCode == 200)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public async Task TestAddEmployeeWithDuplicateId()
        {
            var employeeController = new EmployeeController(employeeRepository);
            List<Employee> employees = await employeeController.GetEmployees();
            int empId;
            if (employees.Count == 0)
            {
                var employee = new Employee
                {
                    Name = "Test case user 1",
                    Department = "Department 1",
                    Email = "email1@emp.com",
                    DOB = new DateTime(1990, 04, 14)
                };
                await employeeController.AddEmployee(employee);
                empId = employee.Id;
            }
            else
            {
                empId = employees[0].Id;
            }

            Employee duplicateEmp = new Employee()
            {
                Id = empId,
                Name = "Test case user 1",
                Department = "Department 1",
                Email = "duplicate_eid@emp.com",
                DOB = new DateTime(1990, 04, 14)
            };
            APIResponse resp = await employeeController.AddEmployee(duplicateEmp);
            if (resp != null && resp.ResponseCode == 400)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public async Task TestUpdateEmployee()
        {
            var employeeController = new EmployeeController(employeeRepository);

            List<Employee> employees = await employeeController.GetEmployees();
            int empId;
            if (employees.Count == 0)
            {
                var employee = new Employee
                {
                    Name = "Test case user 1",
                    Department = "Department 1",
                    Email = "email1@emp.com",
                    DOB = new DateTime(1990, 04, 14)
                };
                await employeeController.AddEmployee(employee);
                empId = employee.Id;
            }
            else
            {
                empId = employees[0].Id;
            }

            var employee1 = new Employee
            {
                Id = empId,
                Name = "Test case user updated",
                Department = "Department updated",
                Email = "email1@emp.com",
                DOB = new DateTime(1990, 04, 14)
            };
            APIResponse resp = await employeeController.UpdateEmployee(employee1);
            if (resp != null && resp.ResponseCode == 200)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }

        }

        [Test]
        public async Task TestDeleteEmployee()
        {
            var employeeController = new EmployeeController(employeeRepository);

            List<Employee> employees = await employeeController.GetEmployees();
            int empId = 0;
            if (employees.Count == 0)
            {
                var employee = new Employee
                {
                    Name = "Test case user 1",
                    Department = "Department 1",
                    Email = "email1@emp.com",
                    DOB = new DateTime(1990, 04, 14)
                };
                await employeeController.AddEmployee(employee);
                empId = employee.Id;
            }
            else
            {
                empId = employees[0].Id;
            }
            APIResponse resp = await employeeController.DeleteEmployee(empId);
            if (resp != null && resp.ResponseCode == 200)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }

        }

        [Test]
        public async Task TestGetEmployees()
        {
            var employeeController = new EmployeeController(employeeRepository);
            List<Employee> employees = await employeeController.GetEmployees();
            int count = employees.Count();
            var employee = new Employee
            {
                Name = "Test case user 1",
                Department = "Department 1",
                Email = "getemployeeidtest@emp.com",
                DOB = new DateTime(1990, 04, 14)
            };
            APIResponse resp = await employeeController.AddEmployee(employee);
            if (resp != null && resp.ResponseCode == 200)
            {
                employees = await employeeController.GetEmployees();
                if (employees.Count() > count)
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }
            }
            else
            {
                Assert.Fail("Failed while adding employee");
            }
        }

        [Test]
        public async Task TestGetEmployeeById()
        {
            var employeeController = new EmployeeController(employeeRepository);
            List<Employee> employees = await employeeController.GetEmployees();
            int empId = 0;
            if (employees.Count() == 0)
            {
                var employee = new Employee
                {
                    Name = "Test case user 1",
                    Department = "Department 1",
                    Email = "email1@emp.com",
                    DOB = new DateTime(1990, 04, 14)
                };
                APIResponse resp = await employeeController.AddEmployee(employee);
                empId = employee.Id;
            }
            else
            {
                empId = employees[0].Id;
            }

            Employee employee1 = await employeeController.GetEmployeeById(empId);
            if (employee1 != null)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }

        }

        [Test]
        public async Task TestSearchEmployees()
        {
            var employeeController = new EmployeeController(employeeRepository);
            var employee = new Employee
            {
                Name = "Test case user 1",
                Department = "Department 1",
                Email = "uniquetest@emp.com",
                DOB = new DateTime(1990, 04, 14)
            };
            APIResponse resp = await employeeController.AddEmployee(employee);
            if (resp != null && resp.ResponseCode == 200)
            {
                List<Employee> employees = await employeeController.SearchEmployees("unique");
                if (employees.Count == 1)
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }
            }
            else
            {
                Assert.Fail("Failed while adding employee");
            }
        }
    }
}