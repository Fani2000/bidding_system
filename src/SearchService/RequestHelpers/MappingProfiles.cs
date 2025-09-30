using SearchService.Entities;

namespace SearchService.RequestHelpers;

public class MappingProfiles : AutoMapper.Profile
{
    public MappingProfiles()
    {
        CreateMap<Contracts.Events.AuctionCreated, Item>();
        CreateMap<Contracts.Events.AuctionUpdated, Item>();
        CreateMap<Contracts.Events.AuctionDeleted, Item>();
    }
}
