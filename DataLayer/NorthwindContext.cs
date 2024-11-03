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
    public DbSet<Actor> Actors { get; set; }
    public DbSet<SearchHistory> SearchHistories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Env.Load();
        string host = Env.GetString("DB_HOST");
        string db = Env.GetString("DB_DATABASE");
        string uid = Env.GetString("DB_USERNAME");
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
        modelBuilder.Entity<User>().Property(u => u.Password).HasColumnName("password");

        modelBuilder.Entity<Movie>().ToTable("title_basics");
        modelBuilder.Entity<Movie>().HasKey(m => m.Id);
        modelBuilder.Entity<Movie>().Property(m => m.Id).HasColumnName("tconst");
        // modelBuilder.Entity<Movie>().Property(m => m.TitleType).HasColumnName("titletype");
        modelBuilder.Entity<Movie>().Property(m => m.Title).HasColumnName("primarytitle");
        modelBuilder.Entity<Movie>().Property(m => m.Year).HasColumnName("startyear");
        modelBuilder.Entity<Movie>().Property(m => m.Genre).HasColumnName("genres");
        modelBuilder.HasDbFunction(
             methodInfo: typeof(NorthwindContext).GetMethod(nameof(string_search)),
             builderAction: builder =>
             {
                 builder.HasName("string_search");
             });

        modelBuilder.Entity<Bookmark>().ToTable("bookmarks");
        modelBuilder.Entity<Bookmark>().HasKey(b => b.Id);
        modelBuilder.Entity<Bookmark>().Property(b => b.Id).HasColumnName("bookmark_id");
        modelBuilder.Entity<Bookmark>().Property(b => b.UserId).HasColumnName("user_id");
        modelBuilder.Entity<Bookmark>().Property(b => b.ItemId).HasColumnName("item_id");

        modelBuilder.Entity<UserRating>().ToTable("user_ratings");
        modelBuilder.Entity<UserRating>().HasKey(ur => ur.RatingId);
        modelBuilder.Entity<UserRating>().Property(ur => ur.RatingId).HasColumnName("rating_id");
        modelBuilder.Entity<UserRating>().Property(ur => ur.UserId).HasColumnName("user_id");
        modelBuilder.Entity<UserRating>().Property(ur => ur.MovieId).HasColumnName("tconst");
        modelBuilder.Entity<UserRating>().Property(ur => ur.Rating).HasColumnName("rating");

        modelBuilder.Entity<Actor>().ToTable("name_basics");
        modelBuilder.Entity<Actor>().HasKey(a => a.NConst);
        modelBuilder.Entity<Actor>().Property(a => a.NConst).HasColumnName("nconst");
        modelBuilder.Entity<Actor>().Property(a => a.PrimaryName).HasColumnName("primaryname");
        modelBuilder.Entity<Actor>().Property(a => a.BirthYear).HasColumnName("birthyear");
        modelBuilder.Entity<Actor>().Property(a => a.DeathYear).HasColumnName("deathyear");
        modelBuilder.Entity<Actor>().Property(a => a.PrimaryProfession).HasColumnName("primaryprofession");
        modelBuilder.Entity<Actor>().Property(a => a.KnownForTitles).HasColumnName("knownfortitles");

        modelBuilder.Entity<SearchHistory>().ToTable("search_history");
        modelBuilder.Entity<SearchHistory>().HasKey(sh => sh.Id);
        modelBuilder.Entity<SearchHistory>().Property(sh => sh.Id).HasColumnName("search_id");
        modelBuilder.Entity<SearchHistory>().Property(sh => sh.UserId).HasColumnName("user_id");
        modelBuilder.Entity<SearchHistory>().Property(sh => sh.SearchTerm).HasColumnName("search_term");
    }
    public IQueryable<Movie> string_search(string searchTitle, int userId)
        => FromExpression(() => string_search(searchTitle, userId));
}