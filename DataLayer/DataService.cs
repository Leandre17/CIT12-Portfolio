using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer;
public class DataService : IDataService
{
    public User CreateUser(User user)
    {
        var db = new NorthwindContext();
        db.Users.Add(user);
        db.SaveChanges();
        return user;
    }

    public bool DeleteUser(int userId)
    {
        var db = new NorthwindContext();
        var user = db.Users.Find(userId);
        if (user == null) return false;

        db.Users.Remove(user);
        db.SaveChanges();
        return true;
    }

    public bool LogoutUser(int userId)
    {
        return true;
        // Implement logout logic here if needed
    }

    public IEnumerable<Movie> GetMoviesByYear(string year)
    {
        var db = new NorthwindContext();
        return db.Movies.Where(m => m.Year == year).ToList();
    }

    public IEnumerable<Movie> GetMoviesByTitle(string title)
    {
        var db = new NorthwindContext();
        return db.string_search(title, 1).ToList();
    }

    public Movie CreateMovie(Movie movie)
    {
        var db = new NorthwindContext();
        db.Movies.Add(movie);
        db.SaveChanges();
        return movie;
    }

    public bool UpdateMovie(string movieId, Movie updatedMovie)
    {
        var db = new NorthwindContext();
        var movie = db.Movies.Find(movieId);
        if (movie == null) return false;

        movie.Title = updatedMovie.Title;
        movie.Genre = updatedMovie.Genre;
        movie.Year = updatedMovie.Year;
        db.SaveChanges();
        return true;
    }

    public bool DeleteMovie(string movieId)
    {
        var db = new NorthwindContext();
        var movie = db.Movies.Find(movieId);
        if (movie == null) return false;

        db.Movies.Remove(movie);
        db.SaveChanges();
        return true;
    }

    public Bookmark? GetUserBookmark(int userId, int bookmarkId)
    {
        var db = new NorthwindContext();
        return db.Bookmarks.FirstOrDefault(b => b.UserId == userId && b.Id == bookmarkId);
    }
    public User? AuthenticateUser(string username, string password)
    {
        var db = new NorthwindContext();
        User? user = db.Users.FirstOrDefault(u => u.Name == username);
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

    public User? GetUserById(int userId)
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

    public Movie? GetMovieById(string movieId)
    {
        var db = new NorthwindContext();
        return db.Movies.Find(movieId);
    }

    public IEnumerable<Movie> GetAllMovies()
    {
        var db = new NorthwindContext();
        try
        {
            return db.Movies
                 .AsNoTracking()
                 .Where(m => !string.IsNullOrWhiteSpace(m.Title))
                 .Select(m => new Movie
                 {
                     Id = m.Id,
                     Title = m.Title.Trim(),
                     Genre = !string.IsNullOrWhiteSpace(m.Genre) ? m.Genre.Trim() : null,
                     Year = m.Year
                 })
                 .ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAllMovies: {ex}");
            throw;
        }
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

    public UserRating? GetRatingById(int ratingId)
    {
        var db = new NorthwindContext();
        return db.UserRatings.Find(ratingId);
    }

    public bool UpdateUserRating(UserRating rating)
    {
        var db = new NorthwindContext();
        var existingRating = db.UserRatings.Find(rating.UserId, rating.MovieId);
        if (existingRating == null) return false;

        existingRating.Rating = rating.Rating;
        db.SaveChanges();
        return true;
    }

    public bool DeleteUserRating(int ratingId)
    {
        var db = new NorthwindContext();
        var rating = db.UserRatings.Find(ratingId);
        if (rating == null) return false;

        db.UserRatings.Remove(rating);
        db.SaveChanges();
        return true;
    }

    public double GetMovieRatings(string movieId)
    {
        var db = new NorthwindContext();
        return db.UserRatings.Where(r => r.MovieId == movieId).Average(r => r.Rating);
    }

    public IEnumerable<Actor> GetActorsInMovie(string movieId)
    {
        var db = new NorthwindContext();
        return db.Actors.Where(a => a.KnownForTitles.Contains(movieId)).ToList();
    }
}

