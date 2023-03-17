using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WasteFoodDistributionSystem.Models.EF
{
    public class CollectRequest
    {
        [Key]
        public int RequestId { get; set; }

        [Required]
        public int RestaurantId { get; set; }
        [Required]
        public string Name { get; set; }

        public string Image { get; set; }
        public int Amount { get; set; }

        public int? EmployeeId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; }

        [Required]
        public int MaximumPreservationTime { get; set; }

        [ForeignKey("RestaurantId")]
        public virtual Restaurant Restaurant { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
    }
}