using DataLayer;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebServer.Models;

namespace WebServer.Controllers
{
    [Route("api/users/{userId}/ratings")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IDataService _dataService;
        private readonly LinkGenerator _linkGenerator;
        public RatingController(IDataService dataService, LinkGenerator linkGenerator)
        {
            _dataService = dataService;
            _linkGenerator = linkGenerator;
        }

        // GET: api/users/{userId}/ratings
        [HttpGet]
        public IActionResult GetUserRatings(int userId)
        {
            var ratings = _dataService.GetRatingsByUser(userId);
            if (ratings == null)
            {
                return NotFound();
            }
            var ratingDtos = ratings.Adapt<IEnumerable<RatingDTO>>();
            foreach (var rating in ratingDtos)
            {
                rating.Link = _linkGenerator.GetUriByAction(HttpContext, nameof(GetUserRatings), values: new { ratingId = rating.RatingId });
            }
            return Ok(ratingDtos);
        }

        // POST: api/users/{userId}/ratings
        [HttpPost]
        public IActionResult CreateRating(int userId, [FromBody] UserRatingDTO rating)
        {
            if (rating == null || rating.MovieId == null)
            {
                return BadRequest();
            }
            var newRating = _dataService.AddUserRating(userId, rating.MovieId, rating.Rating);
            return CreatedAtAction(nameof(GetRatingById), new { ratingId = newRating.RatingId }, rating);
        }

        // PUT: api/users/{userId}/ratings/{ratingId}
        [HttpPut("{ratingId}")]
        public IActionResult UpdateRating(int userId, int ratingId, [FromBody] UserRatingDTO rating)
        {
            if (rating == null || rating.MovieId == null)
            {
                return BadRequest();
            }

            var existingRating = _dataService.GetRatingById(ratingId);
            if (existingRating == null || existingRating.UserId != userId)
            {
                return NotFound();
            }

            existingRating.Rating = rating.Rating;
            _dataService.UpdateUserRating(existingRating);
            return Ok();
        }

        // DELETE: api/users/{userId}/ratings/{ratingId}
        [HttpDelete("{ratingId}")]
        public IActionResult DeleteRating(int userId, int ratingId)
        {
            var existingRating = _dataService.GetRatingById(ratingId);
            if (existingRating == null || existingRating.UserId != userId)
            {
                return NotFound();
            }
            _dataService.DeleteUserRating(ratingId);
            return Ok();
        }

        // GET: api/users/{userId}/ratings/{ratingId}
        [HttpGet("{ratingId}")]
        public IActionResult GetRatingById(int userId, int ratingId)
        {
            var rating = _dataService.GetRatingById(ratingId);
            if (rating == null || rating.UserId != userId)
            {
                return NotFound();
            }
            var ratingDto = rating.Adapt<RatingDTO>();
            ratingDto.Link = _linkGenerator.GetUriByAction(HttpContext, nameof(GetRatingById), values: new { ratingId = rating.RatingId });
            return Ok(ratingDto);
        }
    }
}