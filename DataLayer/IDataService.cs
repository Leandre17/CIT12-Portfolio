using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer;
public interface IDataService
{
    // user methods
    User AuthenticateUser(string username, string password);
    User RegisterUser(string username, string email, string password);
    User GetUserById(int userId);
    User CreateUser(User user);
    bool UpdateUser(User user);
    bool DeleteUser(int userId);
    bool LogoutUser(int userId);

    // movie methods
    Movie GetMovieById(string movieId);
    IEnumerable<Movie> GetAllMovies();
    IEnumerable<Movie> GetMoviesByGenre(string genre);
    IEnumerable<Movie> GetMoviesByYear(int year);
    Movie CreateMovie(Movie movie);
    Movie UpdateMovie(string movieId, Movie movie);
    bool DeleteMovie(string movieId);

    // bookmark methods
    IEnumerable<Bookmark> GetBookmarksByUser(int userId);
    Bookmark GetUserBookmark(int userId, int bookmarkId);
    Bookmark AddBookmark(int userId, string movieId);
    bool DeleteBookmark(int bookmarkId, int userId);
    

    // rating methods
    UserRating AddUserRating(int userId, string movieId, int rating);
    IEnumerable<UserRating> GetRatingsByUser(int userId);
    float GetAverageRatingForMovie(string movieId);
}
