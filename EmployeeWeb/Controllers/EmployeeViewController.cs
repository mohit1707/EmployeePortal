using EmployeeWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeWeb.Controllers
{

    public class EmployeeViewController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly EmployeeService _employeeService;

        public EmployeeViewController(EmployeeService employeeService)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("https://localhost:7123/api/") // Adjust based on your API URL
            };
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetEmployeesAsync();
            return View(employees);
        }


        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeService.GetEmployeeAsync(id);
            return employee == null ? NotFound() : View(employee);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeView employee)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(employee);

                await _employeeService.CreateEmployeeAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View(employee); ;
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeService.GetEmployeeAsync(id);
            return employee == null ? NotFound() : View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeView employee)
        {
            if (!ModelState.IsValid)
                return View(employee);

            await _employeeService.UpdateEmployeeAsync(employee);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeService.GetEmployeeAsync(id);
            await _employeeService.DeleteEmployeeAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
