using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer;
public interface IDataService
{
    User AuthenticateUser(string username, string password);
    User RegisterUser(string username, string email, string password);
    User GetUserById(int userId);
    bool UpdateUser(User user);
    Movie GetMovieById(string movieId);
    IEnumerable<Movie> GetAllMovies();
    IEnumerable<Movie> GetMoviesByGenre(string genre);
    IEnumerable<Bookmark> GetBookmarksByUser(int userId);
    Bookmark AddBookmark(int userId, string movieId);
    bool DeleteBookmark(int bookmarkId, int userId);
    UserRating AddUserRating(int userId, string movieId, int rating);
    IEnumerable<UserRating> GetRatingsByUser(int userId);
    float GetAverageRatingForMovie(string movieId);
}
