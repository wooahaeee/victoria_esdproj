using System.ComponentModel.DataAnnotations;

namespace victoria_esdproj.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "User Name is required.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Booking ID is required.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }
    }
}
