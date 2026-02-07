namespace AbsoluteCinema.Infrastructure.Security;

public static class Permissions
{
    public const string SessionsRead = "sessions.read";
    public const string SessionsManage = "sessions.manage";

    public const string HallsRead = "halls.read";
    public const string HallsManage = "halls.manage";

    public const string MoviesRead = "movies.read";
    public const string MoviesManage = "movies.manage";

    public const string GenresRead = "genres.read";
    public const string GenresManage = "genres.manage";

    public const string TicketsCreate = "tickets.create";
    public const string TicketsReadAll = "tickets.read_all";
    public const string TicketsManage = "tickets.manage";

    public const string UsersManage = "users.manage";


    public static readonly string[] All =
    {
        SessionsRead, SessionsManage,
        HallsRead, HallsManage,
        MoviesRead, MoviesManage,
        GenresRead, GenresManage,
        TicketsCreate, TicketsReadAll,
        UsersManage
    };
}
