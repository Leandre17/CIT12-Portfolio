using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer;
public interface IDataService
{
    // user methods
    User? AuthenticateUser(string username, string password);
    User RegisterUser(string username, string email, string password);
    User? GetUserById(int userId);
    User CreateUser(User user);
    bool UpdateUser(User user);
    bool DeleteUser(int userId);
    bool LogoutUser(int userId);

    // movie methods
    Movie? GetMovieById(string movieId);
    IEnumerable<Movie> GetAllMovies();
    IEnumerable<Movie> GetMoviesByGenre(string genre);
    IEnumerable<Movie> GetMoviesByYear(string year);
    IEnumerable<Movie> GetMoviesByTitle(string title);
    Movie CreateMovie(Movie movie);
    bool UpdateMovie(string movieId, Movie movie);
    bool DeleteMovie(string movieId);
    double GetMovieRatings(string movieId);
    IEnumerable<Actor> GetActorsInMovie(string movieId);
    // bookmark methods
    IEnumerable<Bookmark> GetBookmarksByUser(int userId);
    Bookmark? GetUserBookmark(int userId, int bookmarkId);
    Bookmark AddBookmark(int userId, string movieId);
    bool DeleteBookmark(int bookmarkId, int userId);
    

    // rating methods
    UserRating AddUserRating(int userId, string movieId, int rating);
    UserRating? GetRatingById(int ratingId);
    bool UpdateUserRating(UserRating rating);
    bool DeleteUserRating(int ratingId);
    IEnumerable<UserRating> GetRatingsByUser(int userId);
    float GetAverageRatingForMovie(string movieId);

    // search history methods
    IEnumerable<SearchHistory> GetSearchHistoryByUser();
    IEnumerable<SearchHistory> GetSearchHistoryByUser(int userId);
}
