using System.Net;
using Polly;
using Polly.Extensions.Http;
using SearchService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient<AuctionSvcHttpClient>().AddPolicyHandler(GetPolicy());

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthorization();

app.MapControllers();

// Khi ứng dụng bắt đầu (ApplicationStarted), đăng ký một hàm async để khởi tạo cơ sở dữ liệu bằng cách gọi phương thức DbInitializer.InitDb().
app.Lifetime.ApplicationStarted.Register(async () =>
{
    try{
        await DbInitializer.InitDb(app);
    }
    catch (Exception e){
        Console.WriteLine(e);
    }
});

app.Run();

//trả về một chính sách thử lại (retry policy) sử dụng Polly.
// xử lý các lỗi HTTP tạm thời và các trạng thái lỗi "Not Found" (404), và sau đó thực hiện thử lại vô hạn sau mỗi 3 giây.
static IAsyncPolicy<HttpResponseMessage> GetPolicy()
    => HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));