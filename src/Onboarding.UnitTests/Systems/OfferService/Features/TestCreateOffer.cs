using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using OfferService.Features.CreateOffer;
using OfferService.Repositories;
using Onboarding.Core.Offer.Events;
using Onboarding.EventPubs;

namespace Onboarding.UnitTests;

public class TestCreateOffer
{
    [Fact]
    public async Task NewOffer_OnSuccess_ReturnsStatusCode200()
    {
        var mockLogger = new Mock<ILogger<CreateOfferController>>();
        var mockMapper = new Mock<IMapper>();
        var mockRepo = new Mock<IOfferRepository>();
        var mockEventPub = new Mock<IEventPub<OfferCreated>>();
        var validator = new NewOfferValidator();
        var sut = new CreateOfferController(mockLogger.Object, mockMapper.Object, mockRepo.Object,mockEventPub.Object, validator);
        var newOffer = new NewOffer(){
            candidateId="1",
            firstName="John",
            lastName="Doe",
            personalEmail="test@test.ca",
            competitionId="1",
            level="1",
            manager="1",
            positionName="Software Developer",
            salary=100000,
            step=1
        };
        var result = (OkObjectResult) await sut.NewOffer(newOffer);

        result.StatusCode.Should().Be(200);
    }
    [Fact]
    public void Test()
    {
        true.Should().BeTrue();
    }
}