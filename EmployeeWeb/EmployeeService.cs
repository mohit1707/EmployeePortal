using EmployeeWeb.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace EmployeeWeb
{
    public class EmployeeService
    {
        private readonly HttpClient _httpClient;
        private string _token;

        public EmployeeService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("https://localhost:7123/api/") // Adjust based on your API URL
            };
        }

        // Set the JWT token
        public void SetToken(string token)
        {
            // Deserialize the response to extract the token
            var tokenResponse = JsonSerializer.Deserialize<Dictionary<string, string>>(token);

            // Extract the token value
            string plainToken = tokenResponse["token"];
            _token = plainToken;
        }

        // Attach the token to requests
        public void AddAuthorizationHeader()
        {
            if (!string.IsNullOrEmpty(_token))
            {
                if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _httpClient.DefaultRequestHeaders.Remove("Authorization");
                }
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                Console.WriteLine($"Authorization Header: Bearer {_token}");

                // Log the full headers for debugging
                Console.WriteLine("Request Headers:");
                foreach (var header in _httpClient.DefaultRequestHeaders)
                {
                    Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
                }
            }
            else
            {
                Console.WriteLine("Token is null or empty");
            }
        }

        public async Task<IEnumerable<EmployeeView>> GetEmployeesAsync()
        {
            AddAuthorizationHeader();
            var response = await _httpClient.GetAsync("employee");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<EmployeeView>>();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {response.StatusCode}, Content: {errorContent}");
                throw new HttpRequestException($"Error fetching employees: {response.StatusCode}");
            }
        }

        public async Task<EmployeeView> GetEmployeeAsync(int id)
        {
            AddAuthorizationHeader();
            return await _httpClient.GetFromJsonAsync<EmployeeView>($"employee/{id}");
        }

        public async Task CreateEmployeeAsync(EmployeeView employee)
        {
            AddAuthorizationHeader();
            await _httpClient.PostAsJsonAsync("employee", employee);
        }

        public async Task UpdateEmployeeAsync(EmployeeView employee)
        {
            AddAuthorizationHeader();
            await _httpClient.PutAsJsonAsync($"employee/{employee.Id}", employee);
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            AddAuthorizationHeader();
            await _httpClient.DeleteAsync($"employee/{id}");
        }
    }
}
