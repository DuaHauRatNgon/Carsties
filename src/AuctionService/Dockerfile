#base image , Từ khóa as build được sử dụng để đặt tên cho stage này là build, nó cho phép sử dụng nó trong các stage khác sau này.
FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
#thư mục làm việc mặc định. Tất cả các lệnh tiếp theo trong Dockerfile sẽ thực hiện ở đây.
WORKDIR /app
#container sẽ mở cổng 80, nhưng không tự động mở nó. Nó chỉ là một chỉ dẫn cho người dùng để biết cổng mà ứng dụng này cần mở.
EXPOSE 80

# copy all .csproj files and restore as distinct layers.   Use of the same COPY command
# for every dockerfile in the project to take advantage of docker caching
COPY Carsties.sln Carsties.sln
COPY src/AuctionService/AuctionService.csproj src/AuctionService/AuctionService.csproj
COPY src/SearchService/SearchService.csproj src/SearchService/SearchService.csproj
COPY src/GatewayService/GatewayService.csproj src/GatewayService/GatewayService.csproj
COPY src/Contracts/Contracts.csproj src/Contracts/Contracts.csproj
COPY src/IdentityService/IdentityService.csproj src/IdentityService/IdentityService.csproj

# Restore package deps
RUN dotnet restore Carsties.sln

# Copy the app folders over
COPY src/AuctionService src/AuctionService
COPY src/Contracts src/Contracts
WORKDIR /app/src/AuctionService
RUN dotnet publish -c Release -o /app/src/out

# Build runtime image
#Bắt đầu một stage mới với image .NET Asp.Net runtime.
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/src/out .
ENTRYPOINT [ "dotnet", "AuctionService.dll" ]