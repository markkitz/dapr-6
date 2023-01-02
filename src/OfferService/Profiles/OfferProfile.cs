using AutoMapper;
using OfferService.Models;
using Onboarding.Models.Offer.Events;

namespace OfferService.Profiles;

public class OfferProfile : Profile
{
    public OfferProfile()
    {
        CreateMap<Offer, OfferCreated>().ReverseMap();
        CreateMap<NewOffer, Offer>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));
    }
}