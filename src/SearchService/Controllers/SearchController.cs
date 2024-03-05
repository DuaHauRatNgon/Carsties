using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace SearchService;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Item>>> SearchItems([FromQuery] SearchParams searchParams){
        var query = DB.PagedSearch<Item, Item>();

        if (!string.IsNullOrEmpty(searchParams.SearchTerm)){
            query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
        }

        // Đoạn mã này sử dụng một câu lệnh switch case để kiểm tra giá trị của searchParams.OrderBy. 
        // Nếu giá trị này là "make", sẽ sắp xếp kết quả theo trường Make theo thứ tự tăng dần. 
        // Nếu là "new", sẽ sắp xếp kết quả theo thời gian tạo mới nhất (CreatedAt) theo thứ tự giảm dần. 
        // Trong trường hợp còn lại, nếu không khớp với bất kỳ giá trị nào, sẽ sắp xếp kết quả theo trường AuctionEnd theo thứ tự tăng dần.
        query = searchParams.OrderBy switch{
            "make" => query.Sort(x => x.Ascending(a => a.Make)),
            "new" => query.Sort(x => x.Descending(a => a.CreatedAt)),
            _ => query.Sort(x => x.Ascending(a => a.AuctionEnd))
        };

        // câu lệnh switch case này kiểm tra giá trị của searchParams.FilterBy. 
        // Nếu giá trị này là "finished", sẽ áp dụng một điều kiện tìm kiếm để lọc ra các mục mà thời gian đấu giá (AuctionEnd) đã kết thúc (nhỏ hơn thời gian hiện tại). Nếu giá trị là "endingSoon", sẽ áp dụng điều kiện tìm kiếm để lọc ra các mục mà thời gian đấu giá kết thúc trong 6 giờ tiếp theo và vẫn chưa kết thúc. 
        // Trong trường hợp còn lại, nếu không khớp với bất kỳ giá trị nào, sẽ áp dụng điều kiện tìm kiếm để lọc ra các mục mà thời gian đấu giá chưa kết thúc (lớn hơn thời gian hiện tại).
        query = searchParams.FilterBy switch{
            "finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
            "endingSoon" => query.Match(x => x.AuctionEnd < DateTime.UtcNow.AddHours(6) && x.AuctionEnd > DateTime.UtcNow),
            _ => query.Match(x => x.AuctionEnd > DateTime.UtcNow)
        };

        if (!string.IsNullOrEmpty(searchParams.Seller)){
            query.Match(x => x.Seller == searchParams.Seller);
        }

        if (!string.IsNullOrEmpty(searchParams.Winner)){
            query.Match(x => x.Winner == searchParams.Winner);
        }

        query.PageNumber(searchParams.PageNumber);
        query.PageSize(searchParams.PageSize);

        var result = await query.ExecuteAsync();

        return Ok(new{
            results = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });
    }
}