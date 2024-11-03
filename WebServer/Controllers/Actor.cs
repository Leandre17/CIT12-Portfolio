using DataLayer;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.Controllers;
using WebServer.Models;

namespace WebServer.Controllers
{
    [Route("api/actors")]
    [ApiController]
    public class ActorController : ControllerBase
    {
        private readonly IDataService _dataService;
        private readonly LinkGenerator _linkGenerator;

        public ActorController(IDataService dataService, LinkGenerator linkGenerator)
        {
            _dataService = dataService;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public IActionResult GetActors([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var actors = _dataService.GetActors(page, pageSize);
            var actorDtos = actors.Adapt<IEnumerable<ActorDto>>();
            foreach (var actor in actorDtos)
            {
                actor.Link = _linkGenerator.GetUriByAction(HttpContext, nameof(GetActor), values: new { id = actor.NConst });
            }
            return Ok(actorDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetActor(string id)
        {
            var actor = _dataService.GetActorById(id);
            if (actor == null)
            {
                return NotFound();
            }
            var actorDto = actor.Adapt<ActorDto>();
            actorDto.Link = _linkGenerator.GetUriByAction(HttpContext, nameof(GetActor), values: new { id });
            return Ok(actorDto);
        }

        [HttpGet("search")]
        public IActionResult SearchActors([FromQuery] string q)
        {
            var actors = _dataService.SearchActors(q);
            var actorDtos = actors.Adapt<IEnumerable<ActorDto>>();
            foreach (var actor in actorDtos)
            {
                actor.Link = _linkGenerator.GetUriByAction(HttpContext, nameof(GetActor), values: new { id = actor.NConst });
            }
            return Ok(actorDtos);
        }

        [HttpGet("{id}/movies")]
        public IActionResult GetActorMovies(string id)
        {
            var movies = _dataService.GetActorMovies(id);
            var movieDtos = movies.Adapt<IEnumerable<MovieDto>>();
            foreach (var movie in movieDtos)
            {
                movie.Link = _linkGenerator.GetUriByAction(HttpContext, nameof(MoviesController.GetMovieByIdWSL), values: new { id = movie.Id });
            }
            return Ok(movieDtos);
        }

        [HttpPost]
        public IActionResult AddActor([FromBody] Actor actor)
        {
            var createdActor = _dataService.AddActor(actor);
            return CreatedAtAction(nameof(GetActor), new { id = createdActor.NConst }, createdActor);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateActor(string id, [FromBody] Actor actor)
        {
            var updatedActor = _dataService.UpdateActor(id, actor);
            if (!updatedActor)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteActor(string id)
        {
            var deleted = _dataService.DeleteActor(id);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}