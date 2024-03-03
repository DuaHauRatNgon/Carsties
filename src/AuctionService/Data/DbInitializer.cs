using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data;

public class DbInitializer
{
    public static void InitDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        SeedData(scope.ServiceProvider.GetService<AuctionDbContext>());
    }

    private static void SeedData(AuctionDbContext context)
    {
        context.Database.Migrate();

        if (context.Auctions.Any())
        {
            Console.WriteLine("Da co data- khong can seed");
            return;
        }

        var auctions = new List<Auction>()
        {
            // 1 Toyota Camry 2.4G 2003
            new Auction
            {
                Id = Guid.Parse("afbee524-5972-4075-8800-7d1f9d7b0a0c"),
                Status = Status.Live,
                ReservePrice = 20000,
                Seller = "Long Việt Auto 2 ",
                Winner = "",
                AuctionEnd = DateTime.UtcNow.AddDays(10),
                Item = new Item
                {
                    Make = "Toyota",
                    Model = "Camry",
                    Color = "White",
                    Mileage = 50000,
                    Year = 2003,
                    ImageUrl = "https://img1.oto.com.vn/crop/640x480/2022/11/09/20221109100520-10a8_wm.jpg"
                }
            }
            ,
            // 2 Kia Morning EX 1.25 MT 2017
            new Auction
            {
                Id = Guid.Parse("c8c3ec17-01bf-49db-82aa-1ef80b833a9f"),
                Status = Status.Live,
                ReservePrice = 90000,
                Seller = "Auto Luyến Hằng",
                Winner = "",
                AuctionEnd = DateTime.UtcNow.AddDays(60),
                Item = new Item
                {
                    Make = "Kia",
                    Model = "Morning",
                    Color = "Black",
                    Mileage = 15035,
                    Year = 2018,
                    ImageUrl = "https://img1.oto.com.vn/crop/640x480/2024/02/28/1hnnn9kgf4hfdk2-2851_wm.webp"
                }
            },
            // 3 Ford Everest 2.5 MT 2008
            new Auction
            {
                Id = Guid.Parse("bbab4d5a-8565-48b1-9450-5ac2a5c4a654"),
                Status = Status.Live,
                Seller = "Vạn Phát Auto",
                Winner = "",
                AuctionEnd = DateTime.UtcNow.AddDays(4),
                Item = new Item
                {
                    Make = "Ford",
                    Model = "Everest",
                    Color = "Gold",
                    Mileage = 65125,
                    Year = 2023,
                    ImageUrl = "https://img1.oto.com.vn/crop/640x480/2024/02/21/z5178865799059b63f681e5d37822e2d929bcb78cffc80-2a9c_wm.webp"
                }
            },
            // 4 Kia Soluto AT Deluxe 2021
            new Auction
            {
                Id = Guid.Parse("155225c1-4448-4066-9886-6786536e05ea"),
                Status = Status.ReserveNotMet,
                ReservePrice = 50000,
                Seller = "Gia Nguyên Auto",
                Winner = "",
                AuctionEnd = DateTime.UtcNow.AddDays(-10),
                Item = new Item
                {
                    Make = "Kia",
                    Model = "Soluto",
                    Color = "Blue",
                    Mileage = 15001,
                    Year = 2020,
                    ImageUrl = "https://img1.oto.com.vn/crop/640x480/2024/02/27/73bfe450bda340a3bd12bfd8710b5497-3414_wm.webp"
                }
            },
            // 5 Mazda 3 2017
            new Auction
            {
                Id = Guid.Parse("466e4744-4dc5-4987-aae0-b621acfc5e39"),
                Status = Status.Live,
                ReservePrice = 20000,
                Seller = "Long Việt Auto 2",
                Winner = "",
                AuctionEnd = DateTime.UtcNow.AddDays(30),
                Item = new Item
                {
                    Make = "Mazda",
                    Model = "Mazda 3 2017",
                    Color = "Red",
                    Mileage = 90000,
                    Year = 2017,
                    ImageUrl = "https://img1.oto.com.vn/crop/640x480/2024/02/20/z51766614666126288cce0d7dbee81338c31677121543e-e265_wm.webp"
                }
            },
            // 6 Mercedes-Benz GLC 200 4Matic 2022
            new Auction
            {
                Id = Guid.Parse("dc1e4071-d19d-459b-b848-b5c3cd3d151f"),
                Status = Status.Live,
                ReservePrice = 20000,
                Seller = "DNZ – PHỐ XE LƯỚT",
                Winner = "",
                AuctionEnd = DateTime.UtcNow.AddDays(45),
                Item = new Item
                {
                    Make = "Mercedes-Benz",
                    Model = "GLC 200 4Matic",
                    Color = "Silver",
                    Mileage = 50000,
                    Year = 2022,
                    ImageUrl = "https://img1.oto.com.vn/crop/640x480/2024/02/26/20240226153245-628d_wm.webp"
                }
            },
            // 7 VinFast Fadil 2019
            new Auction
            {
                Id = Guid.Parse("47111973-d176-4feb-848d-0ea22641c31a"),
                Status = Status.Live,
                ReservePrice = 150000,
                Seller = "Vạn Phát Auto",
                Winner = "",
                AuctionEnd = DateTime.UtcNow.AddDays(13),
                Item = new Item
                {
                    Make = "VinFast",
                    Model = "Fadil",
                    Color = "Red",
                    Mileage = 5000,
                    Year = 2022,
                    ImageUrl = "https://img1.oto.com.vn/crop/640x480/2024/02/29/z52030113668740a54b02cf7eb3e1c3de17fa4660a6e3c-c7fc_wm.webp"
                }
            },
            // 8 Mazda 6 2.0L Premium 2018
            new Auction
            {
                Id = Guid.Parse("6a5011a1-fe1f-47df-9a32-b5346b289391"),
                Status = Status.Live,
                Seller = "Vạn Phát Auto",
                Winner = "",
                AuctionEnd = DateTime.UtcNow.AddDays(19),
                Item = new Item
                {
                    Make = "Mazda",
                    Model = "Premium",
                    Color = "White",
                    Mileage = 10050,
                    Year = 2018,
                    ImageUrl = "https://img1.oto.com.vn/crop/640x480/2024/02/01/z51254005754005324838e6826ae81486cfea73d049833-731d_wm.webp"
                }
            },
            // 9 Mitsubishi Jolie 2.0 2004
            new Auction
            {
                Id = Guid.Parse("40490065-dac7-46b6-acc4-df507e0d6570"),
                Status = Status.Live,
                ReservePrice = 20000,
                Seller = "Lâm Xuân Mai ",
                Winner = "",
                AuctionEnd = DateTime.UtcNow.AddDays(20),
                Item = new Item
                {
                    Make = "Mitsubishi",
                    Model = "Jolie",
                    Color = "Black",
                    Mileage = 25400,
                    Year = 2004,
                    ImageUrl = "https://img1.oto.com.vn/crop/640x480/2024/02/29/7d288bb1f33a5e64072b20-c533_wm.webp"
                }
            },
            // 10 Ford Ranger XLS 2.2 4x2 AT 2017
            new Auction
            {
                Id = Guid.Parse("3659ac24-29dd-407a-81f5-ecfe6f924b9b"),
                Status = Status.Live,
                ReservePrice = 20000,
                Seller = "Nguyễn Mạnh Hưởng",
                Winner = "",
                AuctionEnd = DateTime.UtcNow.AddDays(48),
                Item = new Item
                {
                    Make = "Ford",
                    Model = "Ranger",
                    Color = "Black",
                    Mileage = 150150,
                    Year = 2017,
                    ImageUrl = "https://img1.oto.com.vn/crop/640x480/2024/01/31/z5122868195304a544bc9568fc596275e936d7c50f28e1-65ed_wm.webp"
                }
            }
        };

        context.AddRange(auctions);

        context.SaveChanges();
    }
}