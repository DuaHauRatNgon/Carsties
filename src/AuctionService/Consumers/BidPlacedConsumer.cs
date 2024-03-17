using AuctionService.Data;
using Contracts;
using MassTransit;

namespace AuctionService;

//là một consumer trong MassTransit được sử dụng để xử lý các tin nhắn BidPlaced
// thực hiện các thao tác như cập nhật giá đấu cao nhất của đấu giá trong cơ sở dữ liệu.
public class BidPlacedConsumer : IConsumer<BidPlaced> {
    private readonly AuctionDbContext _dbContext;

    public BidPlacedConsumer(AuctionDbContext dbContext) {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<BidPlaced> context) {
        Console.WriteLine("--> Consuming bid placed");

        var auction = await _dbContext.Auctions.FindAsync(context.Message.AuctionId);

        if (auction.CurrentHighBid == null 
            || context.Message.BidStatus.Contains("Accepted") 
            && context.Message.Amount > auction.CurrentHighBid)
        {
            //Cập nhật giá đấu cao nhất của đấu giá.
            auction.CurrentHighBid = context.Message.Amount;
            await _dbContext.SaveChangesAsync();
        }
    }
}