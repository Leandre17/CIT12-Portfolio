using DataLayer;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebServer.Models;

namespace WebServer.Controllers
{
    [Route("api/users/{userId}/searches")]
    [ApiController]
    public class SearchHistoryController : ControllerBase
    {
        private readonly IDataService _dataService;
        private readonly LinkGenerator _linkGenerator;

        public SearchHistoryController(IDataService dataService, LinkGenerator linkGenerator)
        {
            _dataService = dataService;
            _linkGenerator = linkGenerator;
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
            var searchHistoryDTO = searchHistory.Adapt<IEnumerable<SearchHistoryDTO>>();
            foreach (var search in searchHistoryDTO)
            {
                search.Link = _linkGenerator.GetUriByAction(HttpContext, nameof(GetSearchHistory), values: new { userId });
            }
            return Ok(searchHistoryDTO);
        }
    }
}