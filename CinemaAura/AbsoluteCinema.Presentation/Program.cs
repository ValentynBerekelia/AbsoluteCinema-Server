using AbsoluteCinema.Application;
using AbsoluteCinema.Application.Features.Movies.Queries;
using AbsoluteCinema.Infrastructure;
using AbsoluteCinema.Infrastructure.EFQueries;
using AbsoluteCinema.Infrastructure.Persistence;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;
using AbsoluteCinema;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddApplication()
    .AddInfrastucture(builder.Configuration)
    .AddPresentation(builder.Configuration);


var config = TypeAdapterConfig.GlobalSettings;

config.Scan(Assembly.GetExecutingAssembly());
builder.Services.AddMapster();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5174")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        // add converter
    });

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


