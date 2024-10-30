using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer;
public class DataService : IDataService
{
    public User AuthenticateUser(string username, string password)
    {
        var db = new NorthwindContext();
        User user = db.Users.FirstOrDefault(u => u.Name == username);
        if (user == null) return null;

        if (user.Password == password)
        {
            return user;
        }
        return null;
    }

    public User RegisterUser(string username, string email, string password)
    {
        var db = new NorthwindContext();
        if (db.Users.Any(u => u.Name == username || u.Email == email))
        {
            throw new ArgumentException("Username or email already exists.");
        }

        User newUser = new User
        {
            Name = username,
            Email = email,
            Password = password
        };
        db.Users.Add(newUser);
        db.SaveChanges();

        return newUser;
    }

    public User GetUserById(int userId)
    {
        var db = new NorthwindContext();
        return db.Users.Find(userId);
    }

    public bool UpdateUser(User user)
    {
        var db = new NorthwindContext();
        var existingUser = db.Users.Find(user.Id);
        if (existingUser == null) return false;

        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        existingUser.Password = user.Password;
        db.SaveChanges();
        return true;
    }

    public Movie GetMovieById(string movieId)
    {
        var db = new NorthwindContext();
        return db.Movies.Find(movieId);
    }

    public IEnumerable<Movie> GetAllMovies()
    {
        var db = new NorthwindContext();
        return db.Movies.ToList();
    }

    public IEnumerable<Movie> GetMoviesByGenre(string genre)
    {
        var db = new NorthwindContext();
        return db.Movies.Where(m => m.Genre == genre).ToList();
    }

    public IEnumerable<Bookmark> GetBookmarksByUser(int userId)
    {
        var db = new NorthwindContext();
        return db.Bookmarks.Where(b => b.UserId == userId).ToList();
    }

    public Bookmark AddBookmark(int userId, string movieId)
    {
        var db = new NorthwindContext();
        if (db.Bookmarks.Any(b => b.UserId == userId && b.ItemId == movieId))
        {
            throw new ArgumentException("Bookmark already exists.");
        }

        Bookmark newBookmark = new Bookmark
        {
            UserId = userId,
            ItemId = movieId
        };
        db.Bookmarks.Add(newBookmark);
        db.SaveChanges();

        return newBookmark;
    }

    public bool DeleteBookmark(int bookmarkId, int userId)
    {
        var db = new NorthwindContext();
        var bookmark = db.Bookmarks.Find(bookmarkId);
        if (bookmark == null || bookmark.UserId != userId) return false;

        db.Bookmarks.Remove(bookmark);
        db.SaveChanges();
        return true;
    }

    public UserRating AddUserRating(int userId, string movieId, int rating)
    {
        var db = new NorthwindContext();
        if (db.UserRatings.Any(r => r.UserId == userId && r.MovieId == movieId))
        {
            throw new ArgumentException("Rating already exists.");
        }

        UserRating newUserRating = new UserRating
        {
            UserId = userId,
            MovieId = movieId,
            Rating = rating
        };
        db.UserRatings.Add(newUserRating);
        db.SaveChanges();

        return newUserRating;
    }

    public IEnumerable<UserRating> GetRatingsByUser(int userId)
    {
        var db = new NorthwindContext();
        return db.UserRatings.Where(r => r.UserId == userId).ToList();
    }

    public float GetAverageRatingForMovie(string movieId)
    {
        var db = new NorthwindContext();
        return (float)db.UserRatings.Where(r => r.MovieId == movieId).Average(r => r.Rating);
    }

}

