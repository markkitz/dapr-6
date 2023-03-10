

using OfferService.Features.CreateOffer;
using OfferService.Features.UpdateOffer;
using OfferService.Models;
using OfferService.Repositories;
using Onboarding.EventPubs;
using Onboarding.Core.Offer.Events;
using FluentValidation;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry;

var builder = WebApplication.CreateBuilder(args);


var serviceName = "OfferService";



builder.Services.AddSingleton<IOfferRepository, DaprOfferRepository>();
builder.Services.AddSingleton<IEventPub<OfferCreated>, CreateOfferEventPub>();
builder.Services.AddSingleton<IEventPub<OfferUpdated>, UpdateOfferEventPub>();
builder.Services.AddValidatorsFromAssemblyContaining<NewOfferValidator>();
// Add services to the container.
builder.Services.AddAutoMapper(typeof(Offer), typeof(OfferCreated));
builder.Services.AddDaprClient();
builder.Services.AddControllers();
// (options =>
// {
//     options.Filters.Add(new TestFilter());
// });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenTelemetry()
        .WithTracing((builder) => builder
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
            .AddAspNetCoreInstrumentation()
            .AddZipkinExporter())
            .WithMetrics()
        .StartWithHost();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapControllers();

app.Run();
