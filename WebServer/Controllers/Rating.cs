using DataLayer;
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
        public RatingController(IDataService dataService)
        {
            _dataService = dataService;
        }

        // GET: api/users/{userId}/ratings
        [HttpGet]
        public IActionResult GetUserRatings(int userId)
        {
            var ratings = _dataService.GetRatingsByUser(userId);
            return Ok(ratings);
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
            return CreatedAtAction(nameof(GetUserRatings), new { userId = userId }, rating);
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
            return NoContent();
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
            return NoContent();
        }
    }
}