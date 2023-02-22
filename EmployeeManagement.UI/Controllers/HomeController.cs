using EmployeeManagement.UI.Models;
using EmployeeManagement.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace EmployeeManagement.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public IActionResult Index()
        {
            ViewBag.APIBaseUrl = _configuration.GetValue<string>("APIBaseUrl");
            return View();
        }

        public async Task<ActionResult> GetEmployees()
        {
            List<EmployeeViewModel> employees = new List<EmployeeViewModel>();
            string apiUrl = _configuration.GetValue<string>("APIBaseUrl") + "/Employee/GetEmployees";
            try
            {
                string token = await TokenHandler.GetOAuthToken(_configuration, _httpClient);

                using (var httpReq = new HttpRequestMessage(HttpMethod.Get, apiUrl))
                {
                    _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    var response = await _httpClient.SendAsync(httpReq);
                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();
                        employees = JsonConvert.DeserializeObject<List<EmployeeViewModel>>(data);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return PartialView("~/Views/Shared/_EmployeelList.cshtml", employees);
        }

        public async Task<ActionResult> SearchEmployees(string keyword)
        {
            List<EmployeeViewModel> employees = new List<EmployeeViewModel>();
            string apiUrl = _configuration.GetValue<string>("APIBaseUrl") + $"/Employee/SearchEmployees?keyword={keyword}";
            try
            {
                string token = await TokenHandler.GetOAuthToken(_configuration, _httpClient);

                using (var httpReq = new HttpRequestMessage(HttpMethod.Get, apiUrl))
                {
                    _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    var response = await _httpClient.SendAsync(httpReq);
                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();
                        employees = JsonConvert.DeserializeObject<List<EmployeeViewModel>>(data);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return PartialView("~/Views/Shared/_EmployeelList.cshtml", employees);

        }

        [HttpGet]
        public async Task<string> GetAccessToken()
        {
            string token = "";
            try
            {
                token = await TokenHandler.GetOAuthToken(_configuration, _httpClient);
            }
            catch (Exception ex)
            {
                throw;
            }
            return token;
        }
    }
}