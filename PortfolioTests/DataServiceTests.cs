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
}