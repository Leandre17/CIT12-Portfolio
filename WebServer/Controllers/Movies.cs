using DataLayer;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[Route("api/movies")]
public class MoviesController : ControllerBase
{
    private readonly IDataService _dataService;

    public MoviesController(IDataService dataService)
    {
        _dataService = dataService;
    }

    // GET: api/movies/{id}
    [HttpGet("{id}")]
    public IActionResult GetMovieByIdWSL(string id)
    {
        var movie = _dataService.GetMovieById(id);
        if (movie == null)
        {
            return NotFound();
        }
        return Ok(movie);
    }
    // POST: api/movies
    [HttpPost]
    public IActionResult CreateMovie([FromBody] Movie movieDto)
    {
        var movie = _dataService.CreateMovie(movieDto);
        return CreatedAtAction(nameof(GetMovieByIdWSL), new { id = movie.Id }, movie);
    }

    // PUT: api/movies/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateMovie(string id, [FromBody] Movie movieDto)
    {
        var updatedMovie = _dataService.UpdateMovie(id, movieDto);
        if (!updatedMovie)
        {
            return NotFound();
        }
        return Ok();
    }

    // DELETE: api/movies/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteMovie(string id)
    {
        var deleted = _dataService.DeleteMovie(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }

    public IActionResult GetMovies([FromQuery] string genre, [FromQuery] int? year, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var movies = _dataService.GetAllMovies();

        if (!string.IsNullOrEmpty(genre))
        {
            movies = movies.Where(m => m.Genre == genre).ToList();
        }

        if (year.HasValue)
        {
            movies = movies.Where(m => m.Year == year.Value).ToList();
        }

        var totalMovies = movies.Count();
        var pagedMovies = movies.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var result = new
        {
            TotalMovies = totalMovies,
            Page = page,
            PageSize = pageSize,
            Movies = pagedMovies,
            _links = new
            {
                self = new { href = $"/api/movies?page={page}&pageSize={pageSize}" },
                next = page * pageSize < totalMovies ? new { href = $"/api/movies?page={page + 1}&pageSize={pageSize}" } : null,
                previous = page > 1 ? new { href = $"/api/movies?page={page - 1}&pageSize={pageSize}" } : null
            }
        };

        return Ok(result);
    }
}