using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;

// Đây là một consumer MassTransit được sử dụng để xử lý message khi một event "AuctionCreated" được gửi đến. 
// Consumer này thực hiện ánh xạ massgage thành object Item, kiểm tra một điều kiện và sau đó lưu đối tượng vào cơ sở dữ liệu.
// Đây là một consumer cụ thể được triển khai để xử lý thông điệp của loại AuctionCreated
public class AuctionCreatedConsumer : IConsumer<AuctionCreated> {
    private readonly IMapper _mapper;

    public AuctionCreatedConsumer(IMapper mapper) {
        _mapper = mapper;
    }

    // Phương thức này được gọi khi consumer nhận được một thông điệp AuctionCreated.
    public async Task Consume(ConsumeContext<AuctionCreated> context) {
        Console.WriteLine("--> Consuming auction created: " + context.Message.Id);

        var item = _mapper.Map<Item>(context.Message);

        if (item.Model == "Mot hang xe nao do ...") throw new ArgumentException("khong the ban nhung xe co model la ...");

        await item.SaveAsync();
    }
}