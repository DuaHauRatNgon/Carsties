
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