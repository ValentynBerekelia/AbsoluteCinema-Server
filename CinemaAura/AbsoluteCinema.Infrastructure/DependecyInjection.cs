using AbsoluteCinema.Application.Abstractions;
using AbsoluteCinema.Application.DTOs.Hall;
using AbsoluteCinema.Application.Features.Auth;
using AbsoluteCinema.Application.Features.Auth.Command.LoginUser;
using AbsoluteCinema.Application.Features.Auth.Command.Logout;
using AbsoluteCinema.Application.Features.Auth.Command.RefreshToken;
using AbsoluteCinema.Application.Features.Auth.Command.RevokeAllRefreshTokens;
using AbsoluteCinema.Application.Features.Auth.Queries.GetCurrentUser;
using AbsoluteCinema.Application.Features.Genres.Queries;
using AbsoluteCinema.Application.Features.Halls.Queries;
using AbsoluteCinema.Application.Features.Movies.Command;
using AbsoluteCinema.Application.Features.Movies.Queries;
using AbsoluteCinema.Application.Features.SeatTypes.Queries;
using AbsoluteCinema.Application.Features.Sessions.Queries;
using AbsoluteCinema.Application.Features.Tickets.Queries;
using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Infrastructure.Authentication;
using AbsoluteCinema.Infrastructure.EFQueries;
using AbsoluteCinema.Infrastructure.Persistence;
using AbsoluteCinema.Infrastructure.Repositories;
using AbsoluteCinema.Infrastructure.Security;
using AbsoluteCinema.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RefreshTokenCommandHandlerInfra = AbsoluteCinema.Infrastructure.EFQueries.RefreshTokenCommand;
using RevokeAllRefreshTokensCommandHandlerInfra = AbsoluteCinema.Infrastructure.EFQueries.RevokeAllRefreshTokensCommandHandler;

namespace AbsoluteCinema.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    ) =>
        services
            .AddDbContext(configuration)
            .AddAuthenticationInternal()
            .AddTokenProvider()
            .AddRepositories()
            .AddStorages()
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

    private static IServiceCollection AddTokenProvider(this IServiceCollection services)
    {
        services.AddScoped<ITokenProvider, TokenProvider>();
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
        services.AddScoped<IStatisticsRepository, StatisticsRepository>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork<CinemaDbContext>>();
        return services;
    }

    private static IServiceCollection AddQueries(this IServiceCollection services)
    {
        services.AddScoped<IGetMoviesDtoQuery, GetMoviesDtoQuery>();
        services.AddScoped<IGetMovieDetailsQuery, GetMovieDetailsQuery>();
        services.AddScoped<IGetAdminMoviesStatsQuery, GetAdminMoviesStatsQuery>();
        services.AddScoped<ICreateGenreCommend, CreateGenreCommand>();
        services.AddScoped<IGetHallsDtoQuery, GetHallsDtoQuery>();
        services.AddScoped<IGetHallQueryHandler, GetHallDetailsQuery>();
        services.AddScoped<IGetFeaturedMoviesDtoQuery, GetFeaturedMoviesDtoQuery>();
        services.AddScoped<IGetTicketQueryHandler, GetTicketDetailsQuery>();
        services.AddScoped<IGetGenresQuery, GetGenreDtoQuery>();
        services.AddScoped<IGetSeatTypesQuery, GetSeatTypeDtoQuery>();
        services.AddScoped<ICreateUserCommand, RegisterUserCommand>();
        services.AddScoped<ILoginUserCommand, LogInUserCommand>();
        services.AddScoped<IRefreshTokenCommand, RefreshTokenCommandHandlerInfra>();
        services.AddScoped<ILogoutCommand, EFQueries.LogoutCommand>();
        services.AddScoped<IRevokeAllRefreshTokensCommand, RevokeAllRefreshTokensCommandHandlerInfra>();
        services.AddScoped<IGetCurrentUserQuery, GetCurrentUserQueryInfra>();

        services.AddScoped<IGetTicketsFromSessionDtoQuery, GetTicketsFromSessionDtoQuery>();
        services.AddScoped<IGetTicketShortQuery, GetTicketShortDtoQuery>();
        return services;
    }

    private static IServiceCollection AddStorages(this IServiceCollection services)
    {
        services.AddScoped<IStorageService, SupabaseStorageService>();

        return services;
    }
}