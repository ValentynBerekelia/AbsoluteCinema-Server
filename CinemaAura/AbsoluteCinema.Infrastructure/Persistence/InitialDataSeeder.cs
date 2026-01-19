using System;
using System.Collections.Generic;
using System.Linq;
using CinemaAura.Infrastructure.Persistence;
using AbsoluteCinema.Domain.Entities;
using CinemaAura.Domain.Enums;
using CinemaAura.Domain.ValueObjects;

namespace CinemaAura.Infrastructure.Persistence;

public static class InitialDataSeeder
{
    public static void Seed(CinemaDbContext context)
    {
        // щоб не дублювати сидінг
        if (context.Movies.Any() || context.Halls.Any() || context.Seats.Any())
            return;

        // --------------------
        // Genres (для фільмів)
        // --------------------
        var action = Genre.Create("Action");
        var drama  = Genre.Create("Drama");
        var comedy = Genre.Create("Comedy");
        var sciFi  = Genre.Create("Sci-Fi");
        var thriller = Genre.Create("Thriller");

        context.Genres.AddRange(action, drama, comedy, sciFi, thriller);

        // --------------------
        // Seat types
        // --------------------
        var standardSeat = SeatType.Create("Standard");
        var vipSeat      = SeatType.Create("VIP");
        var comfortSeat  = SeatType.Create("Comfort");

        context.SeatTypes.AddRange(standardSeat, vipSeat, comfortSeat);

        // --------------------
        // Halls (2-3 штуки)
        // --------------------
        var hall1 = Hall.Create("Hall #1", verticalSize: 8, horizontalSize: 12);
        var hall2 = Hall.Create("Hall #2", verticalSize: 7, horizontalSize: 10);
        var hall3 = Hall.Create("Hall #3", verticalSize: 6, horizontalSize: 15);

        context.Halls.AddRange(hall1, hall2, hall3);

        // --------------------
        // Seats: 6-8 рядів, 10-15 місць для кожного залу
        // --------------------
        var seats = new List<Seat>();

        void AddSeatsForHall(Hall hall, short rows, short seatsPerRow)
        {
            for (short row = 1; row <= rows; row++)
            {
                for (short num = 1; num <= seatsPerRow; num++)
                {
                    // VIP перший ряд, Comfort другий, решта Standard
                    var seatTypeId =
                        row == 1 ? vipSeat.Id :
                        row == 2 ? comfortSeat.Id :
                        standardSeat.Id;

                    var seat = Seat.Create(hall.Id, row, num, seatTypeId);
                    seats.Add(seat);
                    hall.AddSeat(seat.Id);
                }
            }
        }

        AddSeatsForHall(hall1, rows: 8, seatsPerRow: 12);
        AddSeatsForHall(hall2, rows: 7, seatsPerRow: 10);
        AddSeatsForHall(hall3, rows: 6, seatsPerRow: 15);

        context.Seats.AddRange(seats);

        // --------------------
        // Media (постери/трейлери для фільмів)
        // --------------------
        var posters = new[]
        {
            Media.Create(MediaType.Image, "https://i.ibb.co/jvDScBnd/449231935-859617122721322-2155950919009721165-n.jpg"),
            Media.Create(MediaType.Image, "https://i.ibb.co/SwvWqL9W/photo-2024-09-06-14-59-30.jpg"),
            Media.Create(MediaType.Image, "https://i.ibb.co/svznCKVv/2025-02-22-202217.jpg"),
            Media.Create(MediaType.Image, "https://i.ibb.co/WNfxpfLz/Screenshot-20250901-142509-Instagram.png"),
            Media.Create(MediaType.Image, "https://i.ibb.co/TBjk3Ttt/images.jpg"),
        };

        var trailers = new[]
        {
            Media.Create(MediaType.Video, "https://www.youtube.com/watch?v=xvFZjo5PgG0&pp=ygUIcmlja3JvbGzSBwkJTwoBhyohjO8%3D"),
            Media.Create(MediaType.Video, "https://www.youtube.com/watch?v=cIwRQwAS_YY&pp=ygUPZ2xvYmdsb2dhYmdhbGFi"),
            Media.Create(MediaType.Video, "https://www.youtube.com/watch?v=0kV2zInSIjg&pp=ygUSc3Bpbm5pbmcgY29ja3JvYWNo"),
            Media.Create(MediaType.Video, "https://www.youtube.com/watch?v=WIRK_pGdIdA&list=PLgmgQVDTGKAZgbn-317z7F_nXMBDMfKKV"),
            Media.Create(MediaType.Video, "https://www.youtube.com/watch?v=btB2h-MQzQ0"),
        };

        context.Medias.AddRange(posters);
        context.Medias.AddRange(trailers);

        // --------------------
        // Persons (мінімально для зв’язку movie_persons)
        // --------------------
        var persons = new[]
        {
            Person.Create("Christopher Nolan", "Director", UtcDate(1970, 7, 30), PersonRole.Director),
            Person.Create("Leonardo DiCaprio", "Actor", UtcDate(1974, 11, 11), PersonRole.Actor),
            Person.Create("Denis Villeneuve", "Director", UtcDate(1967, 10, 3), PersonRole.Director),
            Person.Create("Ryan Gosling", "Actor", UtcDate(1980, 11, 12), PersonRole.Actor),
            Person.Create("Greta Gerwig", "Director", UtcDate(1983, 8, 4), PersonRole.Director),
            Person.Create("Margot Robbie", "Actor", UtcDate(1990, 7, 2), PersonRole.Actor),
        };

        context.Persons.AddRange(persons);

        // --------------------
        // Movies (5 штук) + зв’язки (жанри/медіа/персони)
        // --------------------
        var m1 = Movie.Create("Inception", "Dreams within dreams", 8.8m, 16, TimeSpan.FromMinutes(148), "USA", "Warner Bros.", "English");
        m1.AddGenre(sciFi.Id); m1.AddGenre(thriller.Id);
        m1.AddMedia(posters[0].Id); m1.AddMedia(trailers[0].Id);
        m1.AddActor(persons[1].Id);

        var m2 = Movie.Create("Blade Runner 2049", "Neo-noir sci-fi", 8.0m, 16, TimeSpan.FromMinutes(164), "USA", "Alcon", "English");
        m2.AddGenre(sciFi.Id); m2.AddGenre(drama.Id);
        m2.AddMedia(posters[1].Id); m2.AddMedia(trailers[1].Id);
        m2.AddActor(persons[3].Id);

        var m3 = Movie.Create("The Dark Knight", "Batman vs Joker", 9.0m, 16, TimeSpan.FromMinutes(152), "USA", "Warner Bros.", "English");
        m3.AddGenre(action.Id); m3.AddGenre(thriller.Id);
        m3.AddMedia(posters[2].Id); m3.AddMedia(trailers[2].Id);

        var m4 = Movie.Create("Barbie", "Comedy/fantasy", 7.0m, 12, TimeSpan.FromMinutes(114), "USA", "Warner Bros.", "English");
        m4.AddGenre(comedy.Id);
        m4.AddMedia(posters[3].Id); m4.AddMedia(trailers[3].Id);
        m4.AddActor(persons[5].Id);

        var m5 = Movie.Create("Interstellar", "Space exploration", 8.6m, 12, TimeSpan.FromMinutes(169), "USA", "Paramount", "English");
        m5.AddGenre(sciFi.Id); m5.AddGenre(drama.Id);
        m5.AddMedia(posters[4].Id); m5.AddMedia(trailers[4].Id);

        context.Movies.AddRange(m1, m2, m3, m4, m5);

        // --------------------
        // Permissions + Roles (без юзерів)
        // --------------------
        var pManageMovies = Permission.Create(PermissionCode.Create("ManageMovies"));
        var pReadMovies   = Permission.Create(PermissionCode.Create("ReadMovies"));
        var pReadSessions = Permission.Create(PermissionCode.Create("ReadSessions"));
        var pManageHalls  = Permission.Create(PermissionCode.Create("ManageHalls"));

        context.Permissions.AddRange(pManageMovies, pReadMovies, pReadSessions, pManageHalls);

        var adminRole = Role.Create("Admin");
        adminRole.Grant(pManageMovies.Id);
        adminRole.Grant(pManageHalls.Id);

        var managerRole = Role.Create("Manager");
        managerRole.Grant(pManageMovies.Id);

        var viewerRole = Role.Create("Viewer");
        viewerRole.Grant(pReadMovies.Id);
        viewerRole.Grant(pReadSessions.Id);

        context.Roles.AddRange(adminRole, managerRole, viewerRole);

        context.SaveChanges();
    }
    
    private static DateTime UtcDate(int y, int m, int d)
        => DateTime.SpecifyKind(new DateTime(y, m, d), DateTimeKind.Utc);
}
