namespace WasteFoodDistributionSystem.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WasteFoodDistributionSystem.Models.EF;
    using Faker;


    internal sealed class Configuration : DbMigrationsConfiguration<WasteFoodDistributionSystem.Models.FoodDistributionDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }


        protected override void Seed(WasteFoodDistributionSystem.Models.FoodDistributionDbContext context)
        {
            var rand = new Random();

            // Seed Restaurants
            for (var i = 1; i <= 15; i++)
            {
                context.Restaurants.AddOrUpdate(r => r.Name, new Restaurant
                {
                    RestaurantId = i,
                    Name = $"Restaurant {i + 1}",
                    Address = Faker.Address.StreetAddress(),
                    ContactNumber = Faker.Phone.Number(),
                    Email = Faker.Internet.Email(),
                    Password = "Pa$$w0rd!", // Generate a password with a random number
                    Image = "https://wallup.net/wp-content/uploads/2019/09/761345-restaurant-food-architecture-interior-design-room.jpg" // Replace this with the path of your restaurant images
                });
            }

            // Seed Employees
            for (var i = 1; i <= 20; i++)
            {
                context.Employees.AddOrUpdate(e => e.Name, new Employee
                {
                    EmployeeId = i,
                    Name = Faker.Name.FullName(),
                    ContactNumber = Faker.Phone.Number(),
                    Email = Faker.Internet.Email(),
                    AssignedArea = Faker.Address.City(),
                    Password = "Pa$$w0rd!", // Generate a password with a random number
                    Image = "https://cdn2.iconfinder.com/data/icons/colored-simple-circle-volume-01/128/circle-flat-general-51851bd79-1024.png" // Replace this with the path of your employee images
                });
            }

            // Seed CollectRequests
            for (var i = 1; i <= 100; i++)
            {
                context.CollectRequests.AddOrUpdate(cr => cr.Name, new CollectRequest
                {
                    RequestId = i,
                    RestaurantId = rand.Next(1, 11), // Randomly select a restaurant ID between 1 and 10
                    Name = $"Food {i + 1}",
                    Image = "https://wallpaperaccess.com/full/3606575.jpg", // Replace this with the path of your collect request images
                    Amount = rand.Next(5, 50), // Randomly select an amount between 5 and 50
                    EmployeeId = rand.Next(1, 11), // Randomly select an employee ID between 1 and 10
                    CreatedAt = DateTime.Now.AddDays(rand.Next(-30, 0)),
                    Status = rand.Next(0, 2) == 0 ? "Pending" : "Completed", // Randomly select "Pending" or "Completed" status
                    MaximumPreservationTime = DateTime.Now.AddHours(rand.Next(1, 24 * 7)) // Randomly select a time between 1 hour and 1 week from now
                });
            }

            // Seed FoodDistributions
            for (var i = 1; i <= 100; i++)
            {
                context.FoodDistributions.AddOrUpdate(fd => fd.RequestId, new FoodDistribution
                {
                    DistributionId = i,
                    RequestId = i, // Use the same ID as the CollectRequest ID for simplicity in this example
                    DistributedAt = DateTime.Now.AddDays(rand.Next(-30, 0)), // Randomly select a date between 30 days ago and today
                    RecipientCount = rand.Next(1, 11), // Randomly select a recipient count between 1 and 10
                    Location = Faker.Address.StreetAddress()
                });
            }

            // Save the changes to the database
            context.SaveChanges();
        }

    }
}
