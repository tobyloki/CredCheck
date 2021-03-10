using ConsoleApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.Data
{
    public class ContosoPetsContext : DbContext
    {
        // disable this during migration and update database, but enable during web app
        public ContosoPetsContext(DbContextOptions<ContosoPetsContext> options) : base(options) { }

        // enable this during migration and update database, but disable during web app
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ContosoPets;Integrated Security=True");
        //}

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }        
    }
}
