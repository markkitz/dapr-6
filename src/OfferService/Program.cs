

using OfferService.Features.CreateOffer;
using OfferService.Features.UpdateOffer;
using OfferService.Models;
using OfferService.Repositories;
using Onboarding.EventPubs;
using Onboarding.Core.Offer.Events;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IOfferRepository, DaprOfferRepository>();
builder.Services.AddSingleton<IEventPub<OfferCreated>, CreateOfferEventPub>();
builder.Services.AddSingleton<IEventPub<OfferUpdated>, UpdateOfferEventPub>();
builder.Services.AddValidatorsFromAssemblyContaining<NewOfferValidator>();
// Add services to the container.
builder.Services.AddAutoMapper(typeof(Offer), typeof(OfferCreated));
builder.Services.AddDaprClient();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapControllers();

app.Run();
