using AbsoluteCinema.Application.Abstractions;
using AbsoluteCinema.Application.DTOs.Hall;
using AbsoluteCinema.Application.Features.Genres.Queries;
using AbsoluteCinema.Application.Features.Halls.Queries;
using AbsoluteCinema.Application.Features.Movies.Command;
using AbsoluteCinema.Application.Features.Movies.Queries;
using AbsoluteCinema.Application.Features.SeatTypes.Queries;
using AbsoluteCinema.Application.Features.Sessions.Queries;
using AbsoluteCinema.Application.Features.Tickets.Queries;
using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.ValueObjects;
using AbsoluteCinema.Infrastructure.EFQueries;
using AbsoluteCinema.Infrastructure.Persistence;
using AbsoluteCinema.Infrastructure.Repositories;
using AbsoluteCinema.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AbsoluteCinema.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastucture(
        this IServiceCollection services,
        IConfiguration configuration
    ) =>
        services
            .AddDbContext(configuration)
            .AddAuthenticationInternal()
            .AddRepositories()
            .AddQueries();

    private static IServiceCollection AddAuthenticationInternal(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        
        return services;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<CinemaDbContext>(opt =>
        {
            opt.UseNpgsql(connectionString, builder =>
            {
                builder.MigrationsAssembly(typeof(CinemaDbContext).Assembly.FullName);
            });
        });    
        
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IHallRepository, HallRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IMediaRepository, MediaRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<ISeatRepository, SeatRepository>();
        services.AddScoped<ISeatTypeRepository, SeatTypeRepository>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork<CinemaDbContext>>();
        return services;
    }
    
    private static IServiceCollection AddQueries(this IServiceCollection services)
    {
        services.AddScoped<IGetMoviesDtoQuery, GetMoviesDtoQuery>();
        services.AddScoped<IGetMovieDetailsQuery, GetMovieDetailsQuery>();
        services.AddScoped<ICreateGenreCommend, CreateGenreCommand>();
        services.AddScoped<IGetHallsDtoQuery, GetHallsDtoQuery>();
        services.AddScoped<IGetHallQueryHandler, GetHallDetailsQuery>();
        services.AddScoped<IGetFeaturedMoviesDtoQuery, GetFeaturedMoviesDtoQuery>();
        services.AddScoped<IGetTicketQueryHandler, GetTicketDetailsQuery>();
        services.AddScoped<IGetGenresQuery,GetGenreDtoQuery>();
        services.AddScoped<IGetSeatTypesQuery, GetSeatTypeDtoQuery>();
        services.AddScoped<IGetTicketsFromSessionDtoQuery, GetTicketsFromSessionDtoQuery>();
        return services;
    }
}