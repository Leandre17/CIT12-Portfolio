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
}