using Contracts;
using MassTransit;

namespace AuctionService;

//triển khai interface IConsumer của MassTransit để xử lý các thông điệp lỗi (Fault<AuctionCreated>)
public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>> {
    //Phương thức này được gọi khi một fail message được nhận
    //context: Ngữ cảnh của việc consume, cung cấp thông tin về message và info liên quan
    public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context){
        Console.WriteLine("--> Consuming faulty creation");

        //Lấy ra exception đầu tiên từ danh sách các exceptions trong Fault<AuctionCreated>.
        var exception = context.Message.Exceptions.First();

        if (exception.ExceptionType == "System.ArgumentException"){
             //Nếu exception là System.ArgumentException, thay đổi giá trị của thuộc tính Model trong message gốc (AuctionCreated) thành "ThaCo Trường Hải".
            context.Message.Message.Model = "ThaCo Trường Hải";
            //Sau đó, gửi message gốc (đã được chỉnh sửa) đi, có thể thông qua một chanel truyền tin khác.
            await context.Publish(context.Message.Message);
        }
        else {
            Console.WriteLine("exception không phải là System.ArgumentException, có thể lỗi đâu đó trong hệ thống :v");
        }
    }
}

//sơ đồ: message gốc (AuctionService) --> Mass transit/ rabbitmq/ bus/ fault queue --x-> Search sẻvice
//               trả lại về cho AuctionCreatedFaultConsumer để xử lí lỗi <---
//                ---> xử lí message gốc (đã được chỉnh sửa) ---> tiếp tục đẩy cho bus ...                           