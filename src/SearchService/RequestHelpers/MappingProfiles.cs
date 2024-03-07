using AutoMapper;
using Contracts;

namespace SearchService;

//lớp MappingProfiles kế thừa từ Profile trong AutoMapper, một thư viện giúp ánh xạ giữa các loại dữ liệu khác nhau trong .NET. 
public class MappingProfiles : Profile
{
    //định nghĩa 2 quy tắc ánh xạ cho AutoMapper. 
    // Mỗi quy tắc này cho biết cách ánh xạ dữ liệu từ một đối tượng AuctionCreated/ AuctionUpdated sang một đối tượng Item. 
    public MappingProfiles(){
        CreateMap<AuctionCreated, Item>();
        CreateMap<AuctionUpdated, Item>();
    }
    //Điều này giúp AutoMapper tự động ánh xạ các thuộc tính tương ứng giữa các loại này khi được sử dụng trong ứng dụng :v
}