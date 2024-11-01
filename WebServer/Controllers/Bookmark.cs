using DataLayer;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models;

namespace WebApi.Controllers;
[ApiController]
[Route("api/")]
public class BookmarkController : ControllerBase
{
    private readonly IDataService _dataService;

    public BookmarkController(IDataService dataService)
    {
        _dataService = dataService;
    }

    [HttpGet("users/{id}/bookmarks")]
    public IActionResult GetUserBookmarks(int id)
    {
        var bookmarks = _dataService.GetBookmarksByUser(id);
        if (bookmarks == null)
        {
            return NotFound();
        }
        var bookmarkDtos = bookmarks.Adapt<IEnumerable<BookmarkDto>>();
        return Ok(bookmarkDtos);
    }

    [HttpPost("users/{id}/bookmarks")]
    public IActionResult CreateUserBookmark(int id, [FromBody] CreateBookmarkDto createBookmarkDto)
    {
        if (createBookmarkDto == null)
        {
            return BadRequest();
        }

        var bookmark = createBookmarkDto.Adapt<Bookmark>();
        var createdBookmark = _dataService.AddBookmark(id, bookmark.ItemId);

        if (createdBookmark == null)
        {
            return StatusCode(500, "A problem happened while handling your request.");
        }

        var bookmarkDto = createdBookmark.Adapt<BookmarkDto>();
        return CreatedAtAction(nameof(GetUserBookmarks), new { id = id }, bookmarkDto);
    }

    [HttpDelete("users/{userId}/bookmarks/{bookmarkId}")]
    public IActionResult DeleteUserBookmark(int userId, int bookmarkId)
    {
        var bookmark = _dataService.GetUserBookmark(userId, bookmarkId);
        if (bookmark == null)
        {
            return NotFound();
        }

        var result = _dataService.DeleteBookmark(userId, bookmarkId);
        if (!result)
        {
            return StatusCode(500, "A problem happened while handling your request.");
        }

        return NoContent();
    }
}