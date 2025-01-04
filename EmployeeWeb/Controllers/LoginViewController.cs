using EmployeeWeb.Models;
using Microsoft.AspNetCore.Mvc;
namespace EmployeeWeb.Controllers
{
    public class LoginViewController : Controller
    {
        private readonly EmployeeService _employeeService;
        private readonly HttpClient _httpClient;

        public LoginViewController(EmployeeService employeeService, HttpClient httpClient)
        {
            _employeeService = employeeService;
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7123/api/Login", model);
            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                _employeeService.SetToken(token);
                return RedirectToAction("Index", "EmployeeView");
            }

            ModelState.AddModelError("", "Invalid login attempt");
            return View(model);
        }
    }
}
