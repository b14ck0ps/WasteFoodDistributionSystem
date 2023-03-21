using System.ComponentModel.DataAnnotations;
using System.Web;

namespace WasteFoodDistributionSystem.Models.ViewModel
{
    public class RestaurantSettingModel
    {
        public int RestaurantId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+])(?!.*\s).*$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one numeric digit, and one special character")]
        public string NewPassword { get; set; }
        [Required]
        public string ContactNumber { get; set; }
        [Required]
        public string Address { get; set; }
        public string Image { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }

    }
}