using System.Net;
using Polly;
using Polly.Extensions.Http;
using SearchService;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//đăng ký một dịch vụ HttpClient trong container dịch vụ của ASP.NET Core và áp dụng một chính sách thử lại (retry policy) cho HttpClient đó. 
//cái này ở commit trước (cách làm cũ)
builder.Services.AddHttpClient<AuctionSvcHttpClient>().AddPolicyHandler(GetPolicy());

//đăng ký AutoMapper vào container dịch vụ và cấu hình nó để tìm kiếm các profile (cấu hình ánh xạ) trong tất cả các assembly hiện tại của ứng dụng. 
//Mỗi profile định nghĩa cách ánh xạ giữa các types (loại) khác nhau, giúp tự động ánh xạ dữ liệu giữa chúng mà không cần phải viết mã ánh xạ chi tiết.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//đăng ký và cấu hình MassTransit để sử dụng RabbitMQ
//thêm các consumer từ namespace chứa AuctionCreatedConsumer
//cấu hình tên endpoint, thiết lập chính sách tái thử lại
//và cấu hình consumer cụ thể cho một endpoint nhận thông điệp "search-auction-created".
builder.Services.AddMassTransit(x => 
{
    //Tự động tìm và đăng ký tất cả các consumer trong namespace chứa AuctionCreatedConsumer. Điều này giúp MassTransit biết được các consumer nào cần được sử dụng để xử lý các loại mesage cụ thể.
    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
    // Đặt định dạng tên endpoint cho các endpoint được cấu hình trong MassTransit.
    // Sử dụng định dạng Kebab Case (viết thường và ngăn cách bằng -) cho tên endpoint và thêm tiền tố "search". false để chỉ định k viết hoa chữ cái đầu tiên 
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));
    //Cấu hình MassTransit để sử dụng RabbitMQ làm bộ truyền tin.
    x.UsingRabbitMq((context, cfg) => 
    {
        //Cấu hình một endpoint nhận các thông điệp trên RabbitMQ, trong trường hợp này là "search-auction-created".
        cfg.ReceiveEndpoint("search-auction-created", e => 
        {
            //MESSAGE RETRY
            //Thiết lập chính sách thử lại cho endpoint, ở đây là thử lại sau mỗi 5 giây, và giữa các lần thử lại có thời gian đợi là 5 giây.
            e.UseMessageRetry(r => r.Interval(5, 5));
            //Cấu hình consumer cụ thể (trong trường hợp này là AuctionCreatedConsumer) để được sử dụng trong endpoint này.
            e.ConfigureConsumer<AuctionCreatedConsumer>(context);
        });
        //Cấu hình tất cả các endpoints dựa trên ngữ cảnh cấu hình.
        cfg.ConfigureEndpoints(context);
    });
});

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