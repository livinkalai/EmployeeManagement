using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.UI.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        [Required]
        public string Department { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
