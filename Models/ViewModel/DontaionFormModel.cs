using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WasteFoodDistributionSystem.Models.ViewModel
{
    public class DontaionFormModel
    {
        [Required, FileExtensions(Extensions = "jpg,png", ErrorMessage = "Please upload a JPG or PNG file.")]
        public string ImageUrl { get; set; }
        [Required, MaxLength(255)]
        public string Name { get; set; }
        [Required, MaxLength(255)]
        public int Amount { get; set; }
        [Required, MaxLength(255)]
        public int PreservationTime { get; set; }
    }
}