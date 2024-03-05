using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace SearchService;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Item>>> SearchItems(string SearchTerm, int pageNumber = 1, int pageSize = 4){
        var query = DB.PagedSearch<Item>();        

        query.Sort(x => x.Ascending(a => a.Make));

        if (!string.IsNullOrEmpty(SearchTerm)) {
            query.Match(Search.Full, SearchTerm).SortByTextScore();
        }
        
        query.PageNumber(pageNumber);
        query.PageSize(pageSize);

        var result = await query.ExecuteAsync();

        return Ok(new{result = result.Results, pgaeCount = result.PageCount, totalCount = result.TotalCount});
    }
}