using DataLayer;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebServer.Controllers;
using WebServer.Models;

namespace WebApi.Controllers;
[ApiController]
[Route("api/movies")]
public class MoviesController : ControllerBase
{
    private readonly IDataService _dataService;
    private readonly LinkGenerator _linkGenerator;

    public MoviesController(IDataService dataService, LinkGenerator linkGenerator)
    {
        _dataService = dataService;
        _linkGenerator = linkGenerator;
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
        var movieDto = movie.Adapt<MovieDto>();
        movieDto.Link = _linkGenerator.GetUriByAction(HttpContext, nameof(GetMovieByIdWSL), values: new { id = id });
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
        return Ok();
    }

    // GET: api/movies
    [HttpGet]
    public IActionResult GetMovies([FromQuery] string? genre, [FromQuery] string? year, [FromQuery] string? title, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var movies = null as IQueryable<Movie>;
        try
        {
            if (title != null)
            {
                movies = _dataService.GetMoviesByTitle(title).AsQueryable();
            }
            else
            {
                movies = _dataService.GetAllMovies().AsQueryable();
            }
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            if (!string.IsNullOrEmpty(genre))
            {
                movies = movies.Where(m => m.Genre != null &&
                    m.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(year))
            {
                movies = movies.Where(m => m.Year == year);
            }

            var totalMovies = movies.Count();
            var pagedMovies = movies.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var baseUrl = "/api/movies?";
            var queryParams = new List<string>();

            if (!string.IsNullOrEmpty(genre))
                queryParams.Add($"genre={Uri.EscapeDataString(genre)}");

            if (!string.IsNullOrEmpty(year))
                queryParams.Add($"year={year}");

            var baseQueryString = string.Join("&", queryParams);
            if (!string.IsNullOrEmpty(baseQueryString))
                baseQueryString += "&";

            var result = new
            {
                TotalMovies = totalMovies,
                Page = page,
                PageSize = pageSize,
                Movies = pagedMovies,
                _links = new
                {
                    self = new
                    {
                        href = $"{baseUrl}{baseQueryString}page={page}&pageSize={pageSize}"
                    },
                    next = page * pageSize < totalMovies
                        ? new { href = $"{baseUrl}{baseQueryString}page={page + 1}&pageSize={pageSize}" }
                        : null,
                    previous = page > 1
                        ? new { href = $"{baseUrl}{baseQueryString}page={page - 1}&pageSize={pageSize}" }
                        : null
                }
            };

            return Ok(result);
        }
        catch (Exception)
        {
            // Log the error
            return StatusCode(500, new { error = "An error occurred while retrieving movies." });
        }
    }

    // GET: api/movies/search/{title}
    [HttpGet("search")]
    public IActionResult SearchMovies([FromQuery] string q)
    {
        var movies = _dataService.GetMoviesByTitle(q);
        var movieDtos = movies.Adapt<IEnumerable<MovieDto>>();
        foreach (var movieDto in movieDtos)
        {
            movieDto.Link = _linkGenerator.GetUriByAction(HttpContext, nameof(GetMovieByIdWSL), values: new { id = movieDto.Id });
        }
        return Ok(movieDtos);
    }

    // GET: api/movies/{id}/ratings
    [HttpGet("{id}/ratings")]
    public IActionResult GetMovieRatings(string id)
    {
        var ratings = _dataService.GetMovieRatings(id);
        return Ok(ratings);
    }

    // GET: api/movies/{id}/actors
    [HttpGet("{id}/crew")]
    public IActionResult GetActorsInMovie(string id)
    {
        var actors = _dataService.GetActorsInMovie(id);
        if (actors == null)
        {
            return NotFound();
        }
        var actorDtos = actors.Adapt<IEnumerable<ActorDto>>();
        foreach (var actorDto in actorDtos)
        {
            actorDto.Link = _linkGenerator.GetUriByAction(HttpContext, nameof(ActorController.GetActor), values: new { id = actorDto.NConst });
        }
        return Ok(actors);
    }
}