using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PortfolioTests;

public class WebServiceTests
{
    private readonly HttpClient _client = new HttpClient();
    private readonly string _url = "http://localhost:5001/api";

    [Fact]
    public async Task TestGetUser()
    {
        var (data, statusCode) = await GetObject($"{_url}/users/1");

        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.Equal("noq@ruc.dk", data.Value("email"));
    }

    // [Fact]
    // public async Task TestGetUserBookmark()
    // {
    //     var (data, statusCode) = await GetObject($"{_url}/users/1/bookmarks/1");

    //     Assert.Equal(HttpStatusCode.OK, statusCode);
    //     Assert.Equal("tt26693752", data.Value("itemId"));
    // }

    [Fact]
    public async Task TestGetActorsInMovie()
    {
        var (data, statusCode) = await GetArray($"{_url}/movies/tt26693752/crew");

        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.Equal(10, data?.Count);

        var actor = data?.First();
        Assert.Equal("nm12354776", actor?.Value("nConst"));
        Assert.Equal("Jang Jeong-do", actor?.Value("primaryName"));

        actor = data?.Last();
        Assert.Equal("nm4811251 ", actor?.Value("nConst"));
        Assert.Equal("Kim Kang-min", actor?.Value("primaryName"));
    }



    // Helpers

    async Task<(JsonArray?, HttpStatusCode)> GetArray(string url)
    {
        var client = new HttpClient();
        var response = client.GetAsync(url).Result;
        var data = await response.Content.ReadAsStringAsync();
        return (JsonSerializer.Deserialize<JsonArray>(data), response.StatusCode);
    }

    async Task<(JsonObject?, HttpStatusCode)> GetObject(string url)
    {
        var client = new HttpClient();
        var response = client.GetAsync(url).Result;
        var data = await response.Content.ReadAsStringAsync();
        return (JsonSerializer.Deserialize<JsonObject>(data), response.StatusCode);
    }

    async Task<(JsonObject?, HttpStatusCode)> PostData(string url, object content)
    {
        var client = new HttpClient();
        var requestContent = new StringContent(
            JsonSerializer.Serialize(content),
            Encoding.UTF8,
            "application/json");
        var response = await client.PostAsync(url, requestContent);
        var data = await response.Content.ReadAsStringAsync();
        return (JsonSerializer.Deserialize<JsonObject>(data), response.StatusCode);
    }

    async Task<HttpStatusCode> PutData(string url, object content)
    {
        var client = new HttpClient();
        var response = await client.PutAsync(
            url,
            new StringContent(
                JsonSerializer.Serialize(content),
                Encoding.UTF8,
                "application/json"));
        return response.StatusCode;
    }

    async Task<HttpStatusCode> DeleteData(string url)
    {
        var client = new HttpClient();
        var response = await client.DeleteAsync(url);
        return response.StatusCode;
    }

}

static class HelperExt
{
    public static string? Value(this JsonNode node, string name)
    {
        var value = node[name];
        return value?.ToString();
    }

    public static string? FirstElement(this JsonArray node, string name)
    {
        return node.First()?.Value(name);
    }

    public static string? LastElement(this JsonArray node, string name)
    {
        return node.Last()?.Value(name);
    }
}