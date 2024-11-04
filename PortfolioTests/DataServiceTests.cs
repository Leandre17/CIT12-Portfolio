using DataLayer;
namespace Assignment4.Tests;

public class DataServiceTests
{
    private readonly IDataService _dataService = new DataService();
    [Fact]
    public void TestGetUser()
    {
        var user = _dataService.GetUserById(1);
        Assert.Equal("nikolai", user.Name);
    }

    [Fact]
    public void TestGetUserBookmark()
    {
        var bookmark = _dataService.GetUserBookmark(1, 1);
        Assert.Equal("tt26693752", bookmark.ItemId);
    }

    [Fact]
    public void TestGetActorsInMovie()
    {
        var actors = _dataService.GetActorsInMovie("tt26693752");
        Assert.Equal(10, actors.Count());

        var actor = actors.First();
        Assert.Equal("nm12354776", actor.NConst);
        Assert.Equal("Jang Jeong-do", actor.PrimaryName);

        actor = actors.Last();
        Assert.Equal("nm4811251 ", actor.NConst);
        Assert.Equal("Kim Kang-min", actor.PrimaryName);

    }

    [Fact]
    public void TestGetActorById()
    {
        var actor = _dataService.GetActorById("nm12354776");
        Assert.Equal("Jang Jeong-do", actor.PrimaryName);
    }

    [Fact]
    public void TestGetActors()
    {
        var actors = _dataService.GetActors(1, 10);
        Assert.Equal(10, actors.Count());
    }

    [Fact]
    public void TestSearchActors()
    {
        var actors = _dataService.SearchActors("Jang Jeong-do");
        Assert.Equal(1, actors.Count());
    }

    [Fact]
    public void TestCreateActor()
    {
        var actor = new Actor
        {
            NConst = "nm12345678",
            PrimaryName = "Test Actor",
            BirthYear = "1990",
            DeathYear = "2000",
            PrimaryProfession = "Actor",
            KnownForTitles = "tt12345678"
        };
        var newActor = _dataService.AddActor(actor);
        Assert.Equal("Test Actor", newActor.PrimaryName);

        var createdActor = _dataService.GetActorById("nm12345678");
        Assert.Equal("Test Actor", createdActor.PrimaryName);

        var isdeleted = _dataService.DeleteActor("nm12345678");
        Assert.True(isdeleted);
    }

    [Fact]
    public void TestUpdateActor()
    {
        var actor = new Actor
        {
            NConst = "nm12345678",
            PrimaryName = "Test Actor",
            BirthYear = "1990",
            DeathYear = "2000",
            PrimaryProfession = "Actor",
            KnownForTitles = "tt12345678"
        };
        var newActor = _dataService.AddActor(actor);
        Assert.Equal("Test Actor", newActor.PrimaryName);

        newActor.PrimaryName = "Updated Actor";
        var updatedActorBool = _dataService.UpdateActor("nm12345678", newActor);
        Assert.True(updatedActorBool);
        var updatedActor = _dataService.GetActorById("nm12345678");
        Assert.Equal("Updated Actor", updatedActor.PrimaryName);

        var isdeleted = _dataService.DeleteActor("nm12345678");
        Assert.True(isdeleted);
    }

    [Fact]
    public void TestCreateMovie()
    {
        var movie = new Movie
        {
            Id = "tt12345678",
            Title = "Test Movie",
            Year = "2021",
            Genre = "Action"
        };
        var newMovie = _dataService.CreateMovie(movie);
        Assert.Equal("Test Movie", newMovie.Title);

        var createdMovie = _dataService.GetMovieById("tt12345678");
        Assert.Equal("Test Movie", createdMovie.Title);

        var isdeleted = _dataService.DeleteMovie("tt12345678");
        Assert.True(isdeleted);
    }

    [Fact]
    public void TestUpdateMovie()
    {
        var movie = new Movie
        {
            Id = "tt12345678",
            Title = "Test Movie",
            Year = "2021",
            Genre = "Action"
        };
        var newMovie = _dataService.CreateMovie(movie);
        Assert.Equal("Test Movie", newMovie.Title);

        newMovie.Title = "Updated Movie";
        var updatedMovieBool = _dataService.UpdateMovie("tt12345678", newMovie);
        Assert.True(updatedMovieBool);
        var updatedMovie = _dataService.GetMovieById("tt12345678");
        Assert.Equal("Updated Movie", updatedMovie.Title);

        var isdeleted = _dataService.DeleteMovie("tt12345678");
        Assert.True(isdeleted);
    }

    [Fact]
    public void TestGetMoviesByGenre()
    {
        var movies = _dataService.GetMoviesByGenre("Action");
        Assert.Equal(699, movies.Count());
    }

    [Fact]
    public void TestGetMoviesByYear()
    {
        var movies = _dataService.GetMoviesByYear("2021");
        Assert.Equal(13725, movies.Count());
    }

    [Fact]
    public void TestCreateUser()
    {
        var user = new User
        {
            Name = "Test User",
            Email = "t.t@t.com",
            Password = "password",
            Id = 100
        };
        var newUser = _dataService.CreateUser(user);
        Assert.Equal("Test User", newUser.Name);

        var createdUser = _dataService.GetUserById(newUser.Id);
        Assert.Equal("Test User", createdUser.Name);

        var isdeleted = _dataService.DeleteUser(newUser.Id);
        Assert.True(isdeleted);
    }

    [Fact]
    public void TestCreateBookmark()
    {
        var bookmark = new Bookmark
        {
            Id = 100,
            UserId = 1,
            ItemId = "tt12345678"
        };
        var newBookmark = _dataService.AddBookmark(1, "tt12345678");
        Assert.Equal("tt12345678", newBookmark.ItemId);

        var createdBookmark = _dataService.GetUserBookmark(1, newBookmark.Id);
        Assert.Equal("tt12345678", createdBookmark.ItemId);

        var isdeleted = _dataService.DeleteBookmark(newBookmark.Id, 1);
        Assert.True(isdeleted);
    }

    // [Fact]
    // public void TestCreateRating()
    // {
    //     var rating = new UserRating
    //     {
    //         RatingId = 100,
    //         UserId = 1,
    //         MovieId = "tt12345678",
    //         Rating = 1
    //     };
    //     var newRating = _dataService.AddUserRating(1, "tt12345678", 1);
    //     Assert.Equal(1, newRating.Rating);

    //     var createdRating = _dataService.GetRatingById(newRating.RatingId);
    //     Assert.Equal(1, createdRating.Rating);

    //     var isdeleted = _dataService.DeleteUserRating(newRating.RatingId);
    //     Assert.True(isdeleted);
    // }
}