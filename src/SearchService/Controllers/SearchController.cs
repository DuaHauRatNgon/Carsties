using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace SearchService;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Item>>> SearchItems(string SearchTerm){
        var query = DB.Find<Item>();        

        if (!string.IsNullOrEmpty(SearchTerm)) {
            query.Match(Search.Full, SearchTerm).SortByTextScore();
        }

        var result = await query.ExecuteAsync();

        return result;
    }
}