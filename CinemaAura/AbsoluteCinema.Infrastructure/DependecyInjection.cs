using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Infrastructure.Persistence;
using AbsoluteCinema.Infrastructure.Repositories;
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
            .AddRepositories();

    private static IServiceCollection AddAuthenticationInternal(this IServiceCollection services)
    {
        //services.AddSingleton<IPasswordService, PasswordService>();
        
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
        
        services.AddScoped<IUnitOfWork, EfUnitOfWork<CinemaDbContext>>();

        return services;
    }
}