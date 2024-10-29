using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetEnv;

namespace DataLayer;

internal class NorthwindContext : DbContext
{
    // public DbSet<Category>? Categories { get; set; }
    // public DbSet<Product>? Products { get; set; }
    // public DbSet<OrderDetails>? OrderDetails { get; set; }
    // public DbSet<Order>? Orders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Env.Load();
        string host = Env.GetString("DB_HOST");
        string db = Env.GetString("DB_NAME");
        string uid = Env.GetString("DB_USER");
        string pwd = Env.GetString("DB_PASSWORD");
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        optionsBuilder.UseNpgsql($"host={host};db={db};uid={uid};pwd={pwd}");
    }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     MapCategories(modelBuilder);
    //     MapProducts(modelBuilder);
    //     MapOrder(modelBuilder);
    //     MapOrderDetails(modelBuilder);
    // }

    // private static void MapCategories(ModelBuilder modelBuilder)
    // {
    //     // Categories
    //     modelBuilder.Entity<Category>().ToTable("categories");
    //     modelBuilder.Entity<Category>().Property(x => x.Id).HasColumnName("categoryid");
    //     modelBuilder.Entity<Category>().Property(x => x.Name).HasColumnName("categoryname");
    //     modelBuilder.Entity<Category>().Property(x => x.Description).HasColumnName("description");

    // }
    // private static void MapProducts(ModelBuilder modelBuilder)
    // {

    //     // Products
    //     modelBuilder.Entity<Product>().ToTable("products");
    //     modelBuilder.Entity<Product>().Property(x => x.Id).HasColumnName("productid");
    //     modelBuilder.Entity<Product>().Property(x => x.Name).HasColumnName("productname");
    //     modelBuilder.Entity<Product>().Property(x => x.CategoryId).HasColumnName("categoryid");
    //     modelBuilder.Entity<Product>().Property(x => x.UnitPrice).HasColumnName("unitprice");
    //     modelBuilder.Entity<Product>().Property(x => x.UnitsInStock).HasColumnName("unitsinstock");
    //     modelBuilder.Entity<Product>().Property(x => x.QuantityPerUnit).HasColumnName("quantityperunit");
    //     modelBuilder.Entity<Product>().HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId);
    // }

    // private static void MapOrder(ModelBuilder modelBuilder)
    // {
    //     // Orders
    //     modelBuilder.Entity<Order>().ToTable("orders");
    //     modelBuilder.Entity<Order>().Property(x => x.Id).HasColumnName("orderid");
    //     modelBuilder.Entity<Order>().Property(x => x.Date).HasColumnName("orderdate");
    //     modelBuilder.Entity<Order>().Property(x => x.CustomerId).HasColumnName("customerid");
    //     modelBuilder.Entity<Order>().Property(x => x.EmployeeId).HasColumnName("employeeid");
    //     modelBuilder.Entity<Order>().Property(x => x.Required).HasColumnName("requireddate");
    //     modelBuilder.Entity<Order>().Property(x => x.ShippedDate).HasColumnName("shippeddate");
    //     modelBuilder.Entity<Order>().Property(x => x.Freight).HasColumnName("freight");
    //     modelBuilder.Entity<Order>().Property(x => x.ShipName).HasColumnName("shipname");
    //     modelBuilder.Entity<Order>().Property(x => x.ShipAddress).HasColumnName("shipaddress");
    //     modelBuilder.Entity<Order>().Property(x => x.ShipCity).HasColumnName("shipcity");
    // }

    // private static void MapOrderDetails(ModelBuilder modelBuilder)
    // {
    //     // OrderDetails
    //     modelBuilder.Entity<OrderDetails>().HasKey(od => new { od.OrderId, od.ProductId });
    //     modelBuilder.Entity<OrderDetails>().ToTable("orderdetails");
    //     modelBuilder.Entity<OrderDetails>().Property(x => x.OrderId).HasColumnName("orderid");
    //     modelBuilder.Entity<OrderDetails>().Property(x => x.ProductId).HasColumnName("productid");
    //     modelBuilder.Entity<OrderDetails>().Property(x => x.UnitPrice).HasColumnName("unitprice");
    //     modelBuilder.Entity<OrderDetails>().Property(x => x.Quantity).HasColumnName("quantity");
    //     modelBuilder.Entity<OrderDetails>().Property(x => x.Discount).HasColumnName("discount");
    // }
}