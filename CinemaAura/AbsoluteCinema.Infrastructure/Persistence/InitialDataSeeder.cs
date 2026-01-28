using System;
using System.Collections.Generic;
using System.Linq;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Enums;
using AbsoluteCinema.Domain.ValueObjects;
using AbsoluteCinema.Infrastructure.Persistence;

namespace CinemaAura.Infrastructure.Persistence;

public static class InitialDataSeeder
{
    public static void Seed(CinemaDbContext context)
    {
        // Перевірка, щоб не дублювати дані
        if (context.Movies.Any() || context.Halls.Any() || context.Seats.Any())
            return;
    // Genres
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
        // Halls
        // --------------------
        var hall1 = Hall.Create("Hall #1", verticalSize: 8, horizontalSize: 12);
        var hall2 = Hall.Create("Hall #2", verticalSize: 7, horizontalSize: 10);
        var hall3 = Hall.Create("Hall #3", verticalSize: 6, horizontalSize: 15);

        context.Halls.AddRange(hall1, hall2, hall3);

        // --------------------
        // Seats
        // --------------------
        var seats = new List<Seat>();

        void AddSeatsForHall(Hall hall, short rows, short seatsPerRow)
        {
            for (short row = 1; row <= rows; row++)
            {
                for (short num = 1; num <= seatsPerRow; num++)
                {
                    // Логіка: 1-й ряд VIP, 2-й Comfort, решта Standard
                    var seatTypeId =
                        row == 1 ? vipSeat.Id :
                        row == 2 ? comfortSeat.Id :
                        standardSeat.Id;

                    // Створення місця
                    var seat = Seat.Create(hall.Id, row, num, seatTypeId);
                    seats.Add(seat);
                    
                    // Додавання ID місця в сутність Hall (відповідно до вашої логіки Hall.cs)
                    hall.AddSeat(seat.Id);
                }
            }
        }

        AddSeatsForHall(hall1, rows: 8, seatsPerRow: 12);
        AddSeatsForHall(hall2, rows: 7, seatsPerRow: 10);
        AddSeatsForHall(hall3, rows: 6, seatsPerRow: 15);

        context.Seats.AddRange(seats);

        // --------------------
        // Media
        // --------------------
        var posters = new[]
        {
            Media.Create(MediaType.BannerImage, "https://i.ibb.co/jvDScBnd/449231935-859617122721322-2155950919009721165-n.jpg"),
            Media.Create(MediaType.BannerImage, "https://i.ibb.co/SwvWqL9W/photo-2024-09-06-14-59-30.jpg"),
            Media.Create(MediaType.BannerImage, "https://i.ibb.co/svznCKVv/2025-02-22-202217.jpg"),
            Media.Create(MediaType.BannerImage, "https://i.ibb.co/WNfxpfLz/Screenshot-20250901-142509-Instagram.png"),
            Media.Create(MediaType.BannerImage, "https://i.ibb.co/TBjk3Ttt/images.jpg"),
        };

        var trailers = new[]
        {
            Media.Create(MediaType.Video, "https://www.youtube.com/watch?v=xvFZjo5PgG0"),
            Media.Create(MediaType.Video, "https://www.youtube.com/watch?v=cIwRQwAS_YY"),
            Media.Create(MediaType.Video, "https://www.youtube.com/watch?v=0kV2zInSIjg"),
            Media.Create(MediaType.Video, "https://www.youtube.com/watch?v=WIRK_pGdIdA"),
            Media.Create(MediaType.Video, "https://www.youtube.com/watch?v=btB2h-MQzQ0"),
        };

        context.Medias.AddRange(posters);
        context.Medias.AddRange(trailers);

        // --------------------
        // Persons
        // Другий параметр тепер 'bio', четвертий - PersonRole
        // --------------------
        var persons = new[]
        {
            Person.Create("Christopher Nolan", "Known for complex storytelling.", UtcDate(1970, 7, 30), PersonRole.Director),
            Person.Create("Leonardo DiCaprio", "Oscar-winning actor.", UtcDate(1974, 11, 11), PersonRole.Actor),
            Person.Create("Denis Villeneuve", "French-Canadian filmmaker.", UtcDate(1967, 10, 3), PersonRole.Director),
            Person.Create("Ryan Gosling", "Known for La La Land and Drive.", UtcDate(1980, 11, 12), PersonRole.Actor),
            Person.Create("Greta Gerwig", "Director and screenwriter.", UtcDate(1983, 8, 4), PersonRole.Director),
            Person.Create("Margot Robbie", "Australian actress and producer.", UtcDate(1990, 7, 2), PersonRole.Actor),
        };

        context.Persons.AddRange(persons);

        // --------------------
        // Movies
        // Зверніть увагу: AddGenre, AddMedia, AddPerson тепер приймають об'єкти, а не ID
        // --------------------
        
        // Movie 1: Inception
        var m1 = Movie.Create("Inception", "Dreams within dreams", 8.8m, 16, TimeSpan.FromMinutes(148), "USA", "Warner Bros.", "English");
        m1.AddGenre(sciFi); 
        m1.AddGenre(thriller);
        m1.AddMedia(posters[0]); 
        m1.AddMedia(trailers[0]);
        m1.AddPerson(persons[1]); // DiCaprio (Actor)

        // Movie 2: Blade Runner 2049
        var m2 = Movie.Create("Blade Runner 2049", "Neo-noir sci-fi", 8.0m, 16, TimeSpan.FromMinutes(164), "USA", "Alcon", "English");
        m2.AddGenre(sciFi); 
        m2.AddGenre(drama);
        m2.AddMedia(posters[1]); 
        m2.AddMedia(trailers[1]);
        m2.AddPerson(persons[3]); // Ryan Gosling (Actor)

        // Movie 3: The Dark Knight
        var m3 = Movie.Create("The Dark Knight", "Batman vs Joker", 9.0m, 16, TimeSpan.FromMinutes(152), "USA", "Warner Bros.", "English");
        m3.AddGenre(action); 
        m3.AddGenre(thriller);
        m3.AddMedia(posters[2]); 
        m3.AddMedia(trailers[2]);
        m3.AddPerson(persons[0]); // Nolan (Director) - приклад додавання режисера

        // Movie 4: Barbie
        var m4 = Movie.Create("Barbie", "Comedy/fantasy", 7.0m, 12, TimeSpan.FromMinutes(114), "USA", "Warner Bros.", "English");
        m4.AddGenre(comedy);
        m4.AddMedia(posters[3]); 
        m4.AddMedia(trailers[3]);
        m4.AddPerson(persons[5]); // Margot Robbie (Actor)
        m4.AddPerson(persons[4]); // Greta Gerwig (Director)

        // Movie 5: Interstellar
        var m5 = Movie.Create("Interstellar", "Space exploration", 8.6m, 12, TimeSpan.FromMinutes(169), "USA", "Paramount", "English");
        m5.AddGenre(sciFi); 
        m5.AddGenre(drama);
        m5.AddMedia(posters[4]); 
        m5.AddMedia(trailers[4]);
        m5.AddPerson(persons[0]); // Nolan (Director)

        context.Movies.AddRange(m1, m2, m3, m4, m5);

        // --------------------
        // Permissions + Roles
        // Grant тепер приймає об'єкт Permission
        // --------------------
        var pCreateMovies = Permission.Create(PermissionCode.Create("movies.create"));
        var pReadMovies   = Permission.Create(PermissionCode.Create("movies.read"));
        var pUpdateMovies = Permission.Create(PermissionCode.Create("movies.update"));
        var pDeleteMovies = Permission.Create(PermissionCode.Create("movies.delete"));

        var pCreateSessions = Permission.Create(PermissionCode.Create("sessions.create"));
        var pReadSessions   = Permission.Create(PermissionCode.Create("sessions.read"));
        var pUpdateSessions = Permission.Create(PermissionCode.Create("sessions.update"));
        var pDeleteSessions = Permission.Create(PermissionCode.Create("sessions.delete"));

        var pCreateHalls = Permission.Create(PermissionCode.Create("halls.create"));
        var pReadHalls   = Permission.Create(PermissionCode.Create("halls.read"));
        var pUpdateHalls = Permission.Create(PermissionCode.Create("halls.update"));
        var pDeleteHalls = Permission.Create(PermissionCode.Create("halls.delete"));

        context.Permissions.AddRange(pCreateMovies, pReadMovies, pUpdateMovies, pDeleteMovies,  pCreateSessions, pReadSessions, pUpdateSessions, pDeleteSessions, pCreateHalls, pReadHalls, pUpdateHalls, pDeleteHalls);
        var adminRole = Role.Create("Admin");
        adminRole.Grant(pCreateMovies);
        adminRole.Grant(pReadMovies);
        adminRole.Grant(pUpdateMovies);
        adminRole.Grant(pDeleteMovies);
        adminRole.Grant(pCreateSessions);
        adminRole.Grant(pReadSessions);
        adminRole.Grant(pUpdateSessions);
        adminRole.Grant(pDeleteSessions);
        adminRole.Grant(pCreateHalls);
        adminRole.Grant(pReadHalls);
        adminRole.Grant(pUpdateHalls);
        adminRole.Grant(pDeleteHalls);
        

        var managerRole = Role.Create("Manager");
        managerRole.Grant(pCreateMovies);
        managerRole.Grant(pReadMovies);
        managerRole.Grant(pUpdateMovies);
        managerRole.Grant(pDeleteMovies);
        managerRole.Grant(pCreateSessions);
        managerRole.Grant(pReadSessions);
        managerRole.Grant(pUpdateSessions);
        managerRole.Grant(pDeleteSessions);

        var viewerRole = Role.Create("Viewer");
        viewerRole.Grant(pReadMovies);
        viewerRole.Grant(pReadSessions);

        context.Roles.AddRange(adminRole, managerRole, viewerRole);

        context.SaveChanges();
    }
    
    private static DateTime UtcDate(int y, int m, int d)
        => DateTime.SpecifyKind(new DateTime(y, m, d), DateTimeKind.Utc);
}
