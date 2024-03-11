using AuctionService;
using AuctionService.Entities;
using AutoMapper;
using AuctionService.DTOs;
using Contracts;

namespace AuctionService.RequestHelpers {
    public class MappingProfiles : Profile {
        public MappingProfiles() {
            CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item);
            CreateMap<Item, AuctionDto>();
            CreateMap<CreateAuctionDto, Auction>()
                .ForMember(d => d.Item, o => o.MapFrom(s => s));
            CreateMap<CreateAuctionDto, Item>();

            //search service k thể hiểu auction DTO, vì vậy chúng ta cần 1 thứ ở giữa , đó chính là contract , which is the auction created
            CreateMap<AuctionDto, AuctionCreated>();
        }
    }
}
