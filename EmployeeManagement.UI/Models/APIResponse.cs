namespace EmployeeManagement.UI.Models
{
    public class APIResponse
    {
        public int ResponseCode { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}
