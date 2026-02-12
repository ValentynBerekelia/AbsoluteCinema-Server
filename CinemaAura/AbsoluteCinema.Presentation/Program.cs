using AbsoluteCinema;
using AbsoluteCinema.Application;
using AbsoluteCinema.Application.Abstractions;
using AbsoluteCinema.Application.Features.Movies.Queries;
using AbsoluteCinema.Application.Features.Persons.Queries;
using AbsoluteCinema.Infrastructure;
using AbsoluteCinema.Infrastructure.EFQueries;
using AbsoluteCinema.Infrastructure.Persistence;
using Mapster;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using QuestPDF.Infrastructure;

QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

var defaultCulture = new CultureInfo("en-US");
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(defaultCulture),
    SupportedCultures = new List<CultureInfo> {defaultCulture},
    SupportedUICultures = new List<CultureInfo> {defaultCulture}
};

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPresentation(builder.Configuration);


var config = TypeAdapterConfig.GlobalSettings;

config.Scan(Assembly.GetExecutingAssembly());
builder.Services.AddMapster();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5174")
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
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AbsoluteCinema API",
        Version = "v1",
        Description = "Cinema booking system API"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT like: Bearer {your_token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddTransient<IGetMoviesDtoQuery, GetMoviesDtoQuery>();
builder.Services.AddTransient<IGetPersonDtoQuery, GetPersonDtoQuery>();
builder.Services.AddTransient<IGetPersonsDtoQuery, GetPersonsDtoQuery>();

var app = builder.Build();

// using (var scope = app.Services.CreateScope())
// {
//     var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();
//     var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
    
//     await dbContext.Database.MigrateAsync();
//     InitialDataSeeder.Seed(dbContext, passwordHasher);
// }

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
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseCors("AllowViteFrontend");

app.UseRequestLocalization(localizationOptions);

app.MapControllers();


app.Run();
