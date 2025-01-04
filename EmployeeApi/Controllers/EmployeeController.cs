using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeApi.Repositories;
using EmployeeApi.Models;

namespace EmployeeApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _employeeRepository.GetAll();
            return Ok(employees);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _employeeRepository.GetById(id);
            return employee == null ? NotFound() : Ok(employee);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _employeeRepository.Add(employee);
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee employee)
        {
            if (id != employee.Id)
                return BadRequest();

            var existingEmployee = await _employeeRepository.GetById(id);
            if (existingEmployee == null)
                return NotFound();

            // Update the existing employee properties with the new values
            existingEmployee.Name = employee.Name;
            existingEmployee.Gender = employee.Gender;
            existingEmployee.Mobile = employee.Mobile;
            existingEmployee.Email = employee.Email;
            existingEmployee.Password = employee.Password; // Hashing should be implemented here
            existingEmployee.DateOfBirth = employee.DateOfBirth;

            await _employeeRepository.Update(existingEmployee);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var existingEmployee = await _employeeRepository.GetById(id);
            if (existingEmployee == null)
                return NotFound();

            await _employeeRepository.Delete(id);
            return NoContent();
        }
    }
}
