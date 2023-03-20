using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WasteFoodDistributionSystem.Models.EF
{
    public class Restaurant
    {
        [Key]
        public int RestaurantId { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        [Required, MaxLength(255)]
        public string Address { get; set; }

        [Required, MaxLength(50)]
        public string ContactNumber { get; set; }

        [Required, MaxLength(255), EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(255)]
        public string Password { get; set; }
        public string Image { get; set; }
    }
}