using System.ComponentModel.DataAnnotations;

namespace EmployeeWeb.Models
{
    public class AddEmployee
    {

        [Required(ErrorMessage = "Name is mandatory.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Gender is mandatory.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Mobile is mandatory.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be 10 digits.")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Email is mandatory.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is mandatory.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Date of Birth is mandatory.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
    }

}
