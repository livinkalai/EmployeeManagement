using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.DAL.Models
{
    public class APIResponse
    {
        public int ResponseCode { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}
