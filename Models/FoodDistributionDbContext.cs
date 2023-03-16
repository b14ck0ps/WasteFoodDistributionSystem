using System.Data.Entity;
using WasteFoodDistributionSystem.Models.EF;

namespace WasteFoodDistributionSystem.Models
{
    public class FoodDistributionDbContext : DbContext
    {
        public FoodDistributionDbContext() : base("name=fwms_db") { }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<CollectRequest> CollectRequests { get; set; }
        public DbSet<FoodDistribution> FoodDistributions { get; set; }
    }
}