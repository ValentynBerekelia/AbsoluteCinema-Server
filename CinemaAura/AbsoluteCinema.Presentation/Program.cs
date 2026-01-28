using System.Reflection;
using AbsoluteCinema.Application;
using AbsoluteCinema.Application.Features.Movies.Queries;
using AbsoluteCinema.Infrastructure;
using AbsoluteCinema.Infrastructure.EFQueries;
using AbsoluteCinema.Infrastructure.Persistence;
using CinemaAura.Infrastructure.Persistence;
using Mapster;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddApplication()
    .AddInfrastucture(builder.Configuration);


var config = TypeAdapterConfig.GlobalSettings;

config.Scan(Assembly.GetExecutingAssembly());
builder.Services.AddMapster();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "AbsoluteCinema API",
        Version = "v1",
        Description = "Cinema booking system API"
    });
});

builder.Services.AddTransient<IGetMoviesDtoQuery, GetMoviesDtoQuery>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();
    InitialDataSeeder.Seed(db);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "AbsoluteCinema API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.MapControllers();


app.Run();
