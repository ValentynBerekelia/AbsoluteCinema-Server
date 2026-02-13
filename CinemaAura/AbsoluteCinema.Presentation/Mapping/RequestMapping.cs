using AbsoluteCinema.Domain.Entities;
using Mapster;

namespace AbsoluteCinema.Mapping;

public class RequestMapping
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<MovieId, Guid>.NewConfig()
            .MapWith(src => src.Id);

        TypeAdapterConfig<Guid, MovieId>.NewConfig()
            .MapWith(src => new MovieId(src));

        TypeAdapterConfig<SessionId, Guid>.NewConfig()
            .MapWith(src => src.Id);

        TypeAdapterConfig<Guid, SessionId>.NewConfig()
            .MapWith(src => new SessionId(src));

        TypeAdapterConfig<HallId, Guid>.NewConfig()
            .MapWith(src => src.Id);

        TypeAdapterConfig<Guid, HallId>.NewConfig()
            .MapWith(src => new HallId(src));

        TypeAdapterConfig<TicketId, Guid>.NewConfig()
            .MapWith(src => src.Id);

        TypeAdapterConfig<Guid, TicketId>.NewConfig()
            .MapWith(src => new TicketId(src));

        TypeAdapterConfig<UserId, Guid>.NewConfig()
            .MapWith(src => src.Id);

        TypeAdapterConfig<Guid, UserId>.NewConfig()
            .MapWith(src => new UserId(src));

    }


}