using DataLayer;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebServer.Controllers
{
    [Route("api/actors")]
    [ApiController]
    public class ActorController : ControllerBase
    {
        private readonly IDataService _dataService;

        public ActorController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public IActionResult GetActors([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var actors = _dataService.GetActors(page, pageSize);
            return Ok(actors);
        }

        [HttpGet("{id}")]
        public IActionResult GetActor(string id)
        {
            var actor = _dataService.GetActorById(id);
            if (actor == null)
            {
                return NotFound();
            }
            return Ok(actor);
        }

        [HttpGet("search")]
        public IActionResult SearchActors([FromQuery] string q)
        {
            var actors = _dataService.SearchActors(q);
            return Ok(actors);
        }

        [HttpGet("{id}/movies")]
        public IActionResult GetActorMovies(string id)
        {
            var movies = _dataService.GetActorMovies(id);
            return Ok(movies);
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