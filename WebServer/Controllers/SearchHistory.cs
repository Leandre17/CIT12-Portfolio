using DataLayer;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace WebServer.Controllers
{
    [Route("api/users/{userId}/searches")]
    [ApiController]
    public class SearchHistoryController : ControllerBase
    {
        private readonly IDataService _dataService;

        public SearchHistoryController(IDataService dataService)
        {
            _dataService = dataService;
        }

        // GET: api/users/{userId}/searches
        [HttpGet]
        public IActionResult GetSearchHistory(int userId)
        {
            var searchHistory = _dataService.GetSearchHistoryByUser(userId);
            if (searchHistory == null)
            {
                return NotFound();
            }
            return Ok(searchHistory);
        }
    }
}