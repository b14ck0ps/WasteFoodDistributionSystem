using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WasteFoodDistributionSystem.Models.EF
{
    public class FoodDistribution
    {
        [Key]
        public int DistributionId { get; set; }

        [Required]
        public int RequestId { get; set; }

        [Required]
        public DateTime DistributedAt { get; set; }

        [Required]
        public int RecipientCount { get; set; }

        [Required, MaxLength(255)]
        public string Location { get; set; }

        [ForeignKey("RequestId")]
        public virtual CollectRequest CollectRequest { get; set; }
    }
}