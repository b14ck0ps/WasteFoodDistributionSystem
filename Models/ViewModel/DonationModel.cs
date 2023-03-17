using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WasteFoodDistributionSystem.Models.ViewModel
{
    public class DonationModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string imgUrl { get; set; }
        [Required]
        public int PreservTime { get; set; }
        [Required]
        public int Amount { get; set; }
    }
}