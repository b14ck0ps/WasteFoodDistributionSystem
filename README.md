# Food Distribution System

The Food Distribution System is a project designed to help an NGO collect excess food from restaurants and distribute it to deprived people and children in Dhaka city. The system automates and tracks the entire workflow, from the restaurant opening a collect request to the distribution of food by the assigned employee.

## Features

- Restaurant, employee, and administrator accounts with authentication and authorization
- Dashboard for restaurants to open collect requests
- Dashboard for administrators to view, accept, and assign requests to employees
- Dashboard for employees to view their assigned requests and mark them as completed after distribution
- Tracking and reporting of food distributions
- Database to store and manage all data in the system

## Technologies

- .NET Framework 4.8.1
- Entity Framework 6.4.4 (Code First)
- Microsoft SQL Server
- ASP.NET MVC (for web application)

## Getting Started

1. Install .NET Framework 4.8.1 SDK and runtime from [Microsoft's official website](https://dotnet.microsoft.com/download/dotnet-framework/net48).
2. Clone this repository to your local machine.
3. Open the solution file (`FoodDistributionSystem.sln`) in Visual Studio.
4. Ensure that the Entity Framework package is installed. If not, install it via the NuGet Package Manager.
5. Set up a Microsoft SQL Server instance and update the connection string in the `Web.config` file.
6. Run the `Update-Database` command in the Package Manager Console to create the database schema using Entity Framework Code First migrations.
7. Run the project in Visual Studio, and the web application will be hosted on your local machine.

## Usage

1. Register accounts for restaurants, employees, and administrators.
2. Log in as a restaurant and open a collect request.
3. Log in as an administrator and view, accept, and assign requests to employees.
4. Log in as an employee and view assigned requests. Mark them as completed after distributing the food.
5. Administrators can track and generate reports on food distributions.

## Schema
![Schema](https://i.ibb.co/w0qpKfH/fwms-db.png)
