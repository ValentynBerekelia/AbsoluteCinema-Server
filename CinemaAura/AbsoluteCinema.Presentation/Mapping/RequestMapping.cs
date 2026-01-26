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
    }

    
}