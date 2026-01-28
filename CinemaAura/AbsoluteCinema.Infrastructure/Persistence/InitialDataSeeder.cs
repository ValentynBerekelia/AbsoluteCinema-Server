using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Enums;
using AbsoluteCinema.Domain.Primitives;
using Microsoft.EntityFrameworkCore;

namespace AbsoluteCinema.Infrastructure.Persistence;

public static class InitialDataSeeder
{
    public static void Seed(CinemaDbContext context)
    {
        if (context.Movies.Any())
        {
            return;
        }

        // ==========================================
        // 2. GENRES
        // ==========================================
        var action = Genre.Create("Action");
        var drama = Genre.Create("Drama");
        var scifi = Genre.Create("Sci-Fi");
        var adventure = Genre.Create("Adventure");

        context.Genres.AddRange(action, drama, scifi, adventure);

        // ==========================================
        // 3. SeatType
        // ==========================================
        var standardType = SeatType.Create("Standard");
        var vipType = SeatType.Create("VIP");

        context.SeatTypes.AddRange(standardType, vipType);

        context.SaveChanges();

        // ==========================================
        // 4. HALLS AND VENUES
        // ==========================================
        var redHall = CreateHallWithSeats(context, "Red Hall", rows: 10, seatsPerRow: 12, standardType, vipType);
        var blueHall = CreateHallWithSeats(context, "Blue Hall", rows: 8, seatsPerRow: 10, standardType, vipType);

        context.SaveChanges();

        // ==========================================
        // 5. PEOPLE (Actors / Directors)
        // ==========================================

        // Christopher Nolan
        var nolan = Person.Create("Christopher Nolan", "Director...", new DateTime(1970, 7, 30, 0, 0, 0, DateTimeKind.Utc), PersonRole.Director);
        nolan.ChangeMedia(Media.Create(MediaType.Image, "https://upload.wikimedia.org/wikipedia/commons/9/95/Christopher_Nolan_Cannes_2018.jpg"));

        // Leonardo DiCaprio
        var dicaprio = Person.Create("Leonardo DiCaprio", "Actor...", new DateTime(1974, 11, 11, 0, 0, 0, DateTimeKind.Utc), PersonRole.Actor);
        dicaprio.ChangeMedia(Media.Create(MediaType.Image, "https://upload.wikimedia.org/wikipedia/commons/4/46/Leonardo_Dicaprio_Cannes_2019.jpg"));

        // Matthew McConaughey
        var mcconaughey = Person.Create("Matthew McConaughey", "Actor...", new DateTime(1969, 11, 4, 0, 0, 0, DateTimeKind.Utc), PersonRole.Actor);
        mcconaughey.ChangeMedia(Media.Create(MediaType.Image, "https://upload.wikimedia.org/wikipedia/commons/b/bf/Matthew_McConaughey_2011.jpg"));

        context.Persons.AddRange(nolan, dicaprio, mcconaughey);

        // ==========================================
        // 6. Movies
        // ==========================================

        // --- Inception ---
        var inception = Movie.Create(
            "Inception",
            "A thief who steals corporate secrets through the use of dream-sharing technology...",
            8.8m, 13, TimeSpan.FromMinutes(148), "USA", "Warner Bros.", "English"
        );
        inception.AddGenre(action);
        inception.AddGenre(scifi);
        inception.AddPerson(nolan);
        inception.AddPerson(dicaprio);

        inception.AddMedia(Media.Create(MediaType.Image, "https://m.media-amazon.com/images/M/MV5BMjAxMzY3NjcxNF5BMl5BanBnXkFtZTcwNTI5OTM0Mw@@._V1_.jpg")); // Poster
        inception.AddMedia(Media.Create(MediaType.BannerImage, "https://images.alphacoders.com/233/233682.jpg")); // Banner

        // --- Interstellar ---
        var interstellar = Movie.Create(
            "Interstellar",
            "A team of explorers travel through a wormhole in space...",
            8.7m, 12, TimeSpan.FromMinutes(169), "USA", "Paramount Pictures", "English"
        );
        interstellar.AddGenre(adventure);
        interstellar.AddGenre(scifi);
        interstellar.AddPerson(nolan);
        interstellar.AddPerson(mcconaughey);

        interstellar.AddMedia(Media.Create(MediaType.Image, "https://m.media-amazon.com/images/M/MV5BZjdkOTU3MDktN2IxOS00OGEyLWFmMjktY2FiMmZkNWIyODZiXkEyXkFqcGdeQXVyMTMxODk2OTU@._V1_.jpg")); // Poster
        interstellar.AddMedia(Media.Create(MediaType.BannerImage, "https://images.alphacoders.com/554/554060.jpg")); // Banner

        context.Movies.AddRange(inception, interstellar);
        context.SaveChanges(); 

        // ==========================================
        // 7.SESSIONS
        // ==========================================
        
        var tomorrow = DateTime.UtcNow.Date.AddDays(1);

        var session1 = Session.Create(inception.Id, redHall.Id, tomorrow.AddHours(10));

        var session2 = Session.Create(interstellar.Id, blueHall.Id, tomorrow.AddHours(14));

        context.Sessions.AddRange(session1, session2);
        context.SaveChanges();

        // ==========================================
        // 8. TYPE PRICES
        // ==========================================

        var prices = new List<TypePrice>();

        // prices for session1 (Inception)
        prices.Add(TypePrice.Create(session1.Id, standardType.Id, 150.00m)); // Standard: 150 грн
        prices.Add(TypePrice.Create(session1.Id, vipType.Id, 300.00m));      // VIP: 300 грн

        // prices for session2 (Interstellar)
        prices.Add(TypePrice.Create(session2.Id, standardType.Id, 120.00m)); // Standard: 120 грн
        prices.Add(TypePrice.Create(session2.Id, vipType.Id, 250.00m));      // VIP: 250 грн

        context.TypePrices.AddRange(prices);

        // ==========================================
        // 9. FINAL SAVE
        // ==========================================
        context.SaveChanges();
    }

    private static Hall CreateHallWithSeats(
        CinemaDbContext context,
        string name,
        int rows,
        int seatsPerRow,
        SeatType standardType,
        SeatType vipType)
    {
        // Create Hall
        var hall = Hall.Create(name, rows, seatsPerRow);
        context.Halls.Add(hall);

        var seats = new List<Seat>();

        for (short r = 1; r <= rows; r++)
        {
            for (short s = 1; s <= seatsPerRow; s++)
            {
                // last 2 rows - VIP
                var currentType = (r > rows - 2) ? vipType : standardType;

                var seat = Seat.Create(hall.Id, r, s, currentType.Id);
                seats.Add(seat);
            }
        }

        context.Seats.AddRange(seats);
        return hall;
    }
}