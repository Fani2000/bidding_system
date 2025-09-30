using AuctionService.Dtos;
using AuctionService.Entities;
using AutoMapper;
using Contracts.Events;

namespace AuctionService.RequestHandlers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item);
        CreateMap<Item, AuctionDto>();
        CreateMap<CreateAuctionDto, Auction>().ForMember(d => d.Item, o => o.MapFrom(s => s));
        CreateMap<CreateAuctionDto, Item>();
        CreateMap<UpdateAuctionDto, Auction>().ForMember(d => d.Item, o => o.MapFrom(s => s));
        // CreateMap<UpdateAuctionDto, Item>();
        CreateMap<Auction, AuctionCreated>();
        CreateMap<Auction, AuctionUpdated>();
        CreateMap<Auction, AuctionDeleted>();
    }
}
