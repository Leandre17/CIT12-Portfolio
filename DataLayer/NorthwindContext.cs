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
    public DbSet<User> Users { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Bookmark> Bookmarks { get; set; }
    public DbSet<UserRating> UserRatings { get; set; }

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<User>().Property(u => u.Id).HasColumnName("user_id");
        modelBuilder.Entity<User>().Property(u => u.Name).HasColumnName("username");
        modelBuilder.Entity<User>().Property(u => u.Email).HasColumnName("email");

        modelBuilder.Entity<Movie>().ToTable("title_basics");
        modelBuilder.Entity<Movie>().HasKey(m => m.Id);
        modelBuilder.Entity<Movie>().Property(m => m.Id).HasColumnName("tconst");
        // modelBuilder.Entity<Movie>().Property(m => m.TitleType).HasColumnName("titletype");
        modelBuilder.Entity<Movie>().Property(m => m.Title).HasColumnName("primarytitle");
        modelBuilder.Entity<Movie>().Property(m => m.Year).HasColumnName("startyear");
        modelBuilder.Entity<Movie>().Property(m => m.Genre).HasColumnName("genres");
        modelBuilder.Entity<Movie>().Property(m => m.Rating).HasColumnName("averagerating");

        modelBuilder.Entity<Bookmark>().ToTable("bookmarks");
        modelBuilder.Entity<Bookmark>().HasKey(b => b.Id);
        modelBuilder.Entity<Bookmark>().Property(b => b.Id).HasColumnName("bookmark_id");
        modelBuilder.Entity<Bookmark>().Property(b => b.UserId).HasColumnName("user_id");
        modelBuilder.Entity<Bookmark>().Property(b => b.ItemId).HasColumnName("item_id");

        modelBuilder.Entity<UserRating>().ToTable("user_ratings");
        modelBuilder.Entity<UserRating>().HasKey(ur => new { ur.UserId, ur.MovieId });
        modelBuilder.Entity<UserRating>().Property(ur => ur.UserId).HasColumnName("user_id");
        modelBuilder.Entity<UserRating>().Property(ur => ur.MovieId).HasColumnName("tconst");
        modelBuilder.Entity<UserRating>().Property(ur => ur.Rating).HasColumnName("rating");
    }
}