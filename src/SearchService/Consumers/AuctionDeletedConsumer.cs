using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;

public class AuctionDeletedConsumer : IConsumer<AuctionDeleted> {
    public async Task Consume(ConsumeContext<AuctionDeleted> context){
        Console.WriteLine("--> Consuming AuctionDeleted: " + context.Message.Id);

        var result = await DB.DeleteAsync<Item>(context.Message.Id);

        //nếu result trả về k được chấp nhận
        if (!result.IsAcknowledged) 
            throw new MessageException(typeof(AuctionDeleted), "Có vấn đề trong lúc xóa auction");
    }
}