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
        public string NewPassword { get; set; }
        [Required]
        public string ContactNumber { get; set; }
        [Required]
        public string Address { get; set; }
        public string Image { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }

    }
}