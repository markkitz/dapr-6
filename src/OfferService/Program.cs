

using OfferService.Models;
using OfferService.Repositories;
using Onboarding.Models.Offer.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IOfferRepository, DaprOfferRepository>();

// Add services to the container.
builder.Services.AddAutoMapper(typeof(Offer), typeof(OfferCreated));
builder.Services.AddDaprClient();
builder.Services.AddControllers().AddDapr();
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
