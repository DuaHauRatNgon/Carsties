using Duende.IdentityServer;
using IdentityService.Data;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityService;

internal static class HostingExtensions
{
    //Các phương thức này được sử dụng để cấu hình và thiết lập ứng dụng Identity Service trong một ứng dụng ASP.NET Core
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                
                //Nếu ứng dụng đang chạy trong môi trường Docker, thiết lập IssuerUri của Identity Server thành "identity-svc"
                //thay vì "http://localhost:5000"
                // /IssuerUri là URI của Identity Server, nơi mà các ứng dụng khác có thể truy cập để xác thực người dùng 
                //và nhận thông tin về các tài nguyên được bảo vệ.
                if (builder.Environment.IsEnvironment("Docker")){
                    options.IssuerUri = "identity-svc";
                }

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                // options.EmitStaticAudienceClaim = true;
            })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            .AddAspNetIdentity<ApplicationUser>()
            .AddProfileService<CustomProfileService>();     //muốn thêm các claim (vd fullname,..) đính kèm trong jwt trả về
        
        //Đặt SameSite thành Lax giúp bảo vệ ứng dụng khỏi các cuộc tấn công CSRF (Cross-Site Request Forgery) bằng cách ngăn chặn việc gửi cookie trong các yêu cầu từ các trang không tin cậy.
        builder.Services.ConfigureApplicationCookie(options => {
            options.Cookie.SameSite = SameSiteMode.Lax;
        });
        
        builder.Services.AddAuthentication();

        return builder.Build();
    }
    
    public static WebApplication ConfigurePipeline(this WebApplication app)
    { 
        app.UseSerilogRequestLogging();
    
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthorization();
        
        app.MapRazorPages()
            .RequireAuthorization();

        return app;
    }
}