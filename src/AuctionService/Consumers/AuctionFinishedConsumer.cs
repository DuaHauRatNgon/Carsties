using AuctionService.Data;
using AuctionService.Entities;
using Contracts;
using MassTransit;

namespace AuctionService;

//là một consumer trong MassTransit được sử dụng để xử lý các tin nhắn AuctionFinished 
//thực hiện các thao tác như cập nhật trạng thái và thông tin của đấu giá trong db.
public class AuctionFinishedConsumer : IConsumer<AuctionFinished> {
    private readonly AuctionDbContext _dbContext;

    public AuctionFinishedConsumer(AuctionDbContext dbContext) {
        _dbContext = dbContext;
    }

    //Phương thức này được gọi khi một tin nhắn AuctionFinished được nhận
    //Nó nhận một đối tượng ConsumeContext chứa thông tin về tin nhắn.
    public async Task Consume(ConsumeContext<AuctionFinished> context) {
        Console.WriteLine("--> Consume auction đã kết thúc");

        var auction = await _dbContext.Auctions.FindAsync(context.Message.AuctionId);

        // /Nếu đã bán, cập nhật thông tin về người chiến thắng và giá bán.
        if (context.Message.ItemSold){
            auction.Winner = context.Message.Winner;
            auction.SoldAmount = context.Message.Amount;
        }

        //Cập nhật trạng thái của đấu giá dựa trên giá bán và giá giữ chỗ
        auction.Status = auction.SoldAmount > auction.ReservePrice
            ? Status.Finished : Status.ReserveNotMet;

        await _dbContext.SaveChangesAsync();
    }
}