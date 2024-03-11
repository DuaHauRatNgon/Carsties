
using AuctionService.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace AuctionService {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<AuctionDbContext>(opt => {
                opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // cấu hình và đăng ký MassTransit, một thư viện giúp quản lý và triển khai các dịch vụ tương tác truyền tin trên hệ thống. 
            // Trong trường hợp này, nó được cấu hình để sử dụng RabbitMQ làm bộ truyền tin
            builder.Services.AddMassTransit(x => {
                //sử dụng Entity Framework Outbox để quản lý trạng thái gửi của các thông điệp
                //Thêm dịch vụ Entity Framework Outbox vào MassTransit, sử dụng context của cơ sở dữ liệu AuctionDbContext.
                x.AddEntityFrameworkOutbox<AuctionDbContext>(o =>{
                    //Thiết lập thời gian chờ giữa các lần kiểm tra trạng thái gửi của thông điệp trong Outbox. Cấu hình thời gian chờ là 10 giây.
                    o.QueryDelay = TimeSpan.FromSeconds(10);
                    //Chọn loại csdl được sử dụng cho lưu trữ Outbox là PostgreSQL để lưu trữ các thông điệp và trạng thái gửi.
                    o.UsePostgres();
                    //Kích hoạt tính năng Bus Outbox trong Entity Framework Outbox. 
                    //MassTransit sẽ sử dụng Entity Framework Outbox để theo dõi trạng thái gửi của các thông điệp trên Bus.
                    o.UseBusOutbox();
                });

                //Tự động tìm và đăng ký tất cả các consumer trong namespace chứa AuctionCreatedFaultConsumer.
                x.AddConsumersFromNamespaceContaining<AuctionCreatedFaultConsumer>();
                // Đặt định dạng tên endpoint cho các endpoint được cấu hình trong MassTransit.
                // Sử dụng định dạng Kebab Case (viết thường và ngăn cách bằng -) cho tên endpoint và thêm tiền tố "auction". false để chỉ định k viết hoa chữ cái đầu tiên 
                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("auction", false));

                x.UsingRabbitMq((context, cfg) => {
                    cfg.ConfigureEndpoints(context);
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            try {
                DbInitializer.InitDb(app);
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }

            app.Run();
        }
    }
}