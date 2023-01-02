using AutoMapper;
using OfferService.Features.CreateOffer;
using OfferService.Models;
using Onboarding.Models.Offer.Events;

namespace OfferService.Helpers;

public class OfferMapper : Profile
{
    public OfferMapper()
    {
        CreateMap<Offer, OfferCreated>().ReverseMap();
        CreateMap<Offer, OfferUpdated>().ReverseMap();
        CreateMap<NewOffer, Offer>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));
    }
}