using AbsoluteCinema.Application.Abstractions;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Enums;
using AbsoluteCinema.Domain.Primitives;
using AbsoluteCinema.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AbsoluteCinema.Infrastructure.Persistence;

public static class InitialDataSeeder
{
    public static void Seed(CinemaDbContext context, IPasswordHasher passwordHasher)
    {
        // 1. Content: Genres, Halls, Persons, Movies (with Posters, Trailers, and Screenshots)
        SeedContent(context);

        // 2. Schedule: Sessions and Prices
        SeedSessionsAndPrices(context);

        // 3. Security: Permissions, Roles, and Users
        SeedSecurity(context, passwordHasher);
    }

    // ==========================================
    // PART 1: CONTENT
    // ==========================================
    private static void SeedContent(CinemaDbContext context)
    {
        // If movies exist, we assume the content is already seeded
        if (context.Movies.Any()) return;

        // --- GENRES ---
        var genreNames = new[]
        {
            "Action", "Adventure", "Animation", "Biography", "Comedy",
            "Crime", "Documentary", "Drama", "Family", "Fantasy",
            "Film Noir", "History", "Horror", "Music", "Musical",
            "Mystery", "Romance", "Sci-Fi", "Short", "Sport",
            "Superhero", "Thriller", "War", "Western", "Cyberpunk",
            "Steampunk", "Slice of Life", "Dark Fantasy", "Urban", "Political"
        };

        var genres = genreNames.Select(name => Genre.Create(name)).ToList();
        context.Genres.AddRange(genres);
        context.SaveChanges();

        var action = genres.First(g => g.Name == "Action");
        var drama = genres.First(g => g.Name == "Drama");
        var scifi = genres.First(g => g.Name == "Sci-Fi");
        var adventure = genres.First(g => g.Name == "Adventure");
        var comedy = genres.First(g => g.Name == "Comedy");
        var thriller = genres.First(g => g.Name == "Thriller");

        // --- SEAT TYPES ---
        var standardType = SeatType.Create("Standard");
        var vipType = SeatType.Create("VIP");
        context.SeatTypes.AddRange(standardType, vipType);

        context.SaveChanges(); // Save to generate IDs

        // --- HALLS ---
        var redHall = CreateHallWithSeats(context, "Red Hall", 10, 12, standardType, vipType);
        var blueHall = CreateHallWithSeats(context, "Blue Hall", 8, 10, standardType, vipType);
        context.SaveChanges();

        // --- PERSONS ---
        var nolan = Person.Create("Christopher Nolan", "Director...", new DateTime(1970, 7, 30, 0, 0, 0, DateTimeKind.Utc), PersonRole.Director);
        nolan.ChangeMedia(Media.Create(MediaType.PersonImage, "https://upload.wikimedia.org/wikipedia/commons/9/95/Christopher_Nolan_Cannes_2018.jpg"));

        var dicaprio = Person.Create("Leonardo DiCaprio", "Actor...", new DateTime(1974, 11, 11, 0, 0, 0, DateTimeKind.Utc), PersonRole.Actor);
        dicaprio.ChangeMedia(Media.Create(MediaType.PersonImage, "https://upload.wikimedia.org/wikipedia/commons/4/46/Leonardo_Dicaprio_Cannes_2019.jpg"));

        var mcconaughey = Person.Create("Matthew McConaughey", "Actor...", new DateTime(1969, 11, 4, 0, 0, 0, DateTimeKind.Utc), PersonRole.Actor);
        mcconaughey.ChangeMedia(Media.Create(MediaType.PersonImage, "https://upload.wikimedia.org/wikipedia/commons/b/bf/Matthew_McConaughey_2011.jpg"));

        var gosling = Person.Create("Ryan Gosling", "Actor...", new DateTime(1980, 11, 12, 0, 0, 0, DateTimeKind.Utc), PersonRole.Actor);
        gosling.ChangeMedia(Media.Create(MediaType.PersonImage, "https://upload.wikimedia.org/wikipedia/commons/f/f6/Ryan_Gosling_in_2018.jpg"));

        var robbie = Person.Create("Margot Robbie", "Actress...", new DateTime(1990, 7, 2, 0, 0, 0, DateTimeKind.Utc), PersonRole.Actor);
        robbie.ChangeMedia(Media.Create(MediaType.PersonImage, "https://upload.wikimedia.org/wikipedia/commons/0/0b/Margot_Robbie_2019.jpg"));

        var gerwig = Person.Create("Greta Gerwig", "Director...", new DateTime(1983, 8, 4, 0, 0, 0, DateTimeKind.Utc), PersonRole.Director);
        gerwig.ChangeMedia(Media.Create(MediaType.PersonImage, "https://upload.wikimedia.org/wikipedia/commons/8/82/Greta_Gerwig_2018.jpg"));

        context.Persons.AddRange(nolan, dicaprio, mcconaughey, gosling, robbie, gerwig);

        // --- MOVIES ---

        // 1. Inception
        var inception = Movie.Create("Inception", "A thief who steals corporate secrets through the use of dream-sharing technology...", 8.8m, 13, TimeSpan.FromMinutes(148), "USA", "Warner Bros.", "English");
        inception.AddGenre(action);
        inception.AddGenre(scifi);
        inception.AddPerson(nolan);
        inception.AddPerson(dicaprio);

        // Poster
        inception.AddMedia(Media.Create(MediaType.PosterImage, "https://upload.wikimedia.org/wikipedia/uk/e/e1/%D0%9F%D0%BE%D1%87%D0%B0%D1%82%D0%BE%D0%BA_%D1%84%D1%96%D0%BB%D1%8C%D0%BC%2C_2010.jpg"));
        // Trailer
        inception.AddMedia(Media.Create(MediaType.Video, "https://www.youtube.com/watch?v=YoHD9XEInc0"));
        // Screenshots (Movie Stills)
        inception.AddMedia(Media.Create(MediaType.Image, "https://drop.ndtv.com/albums/ENTERTAINMENT/inception-leonardo/2.jpg"));
        inception.AddMedia(Media.Create(MediaType.Image, "https://drop.ndtv.com/albums/ENTERTAINMENT/inception-leonardo/10.jpg"));
        inception.AddMedia(Media.Create(MediaType.Image, "https://drop.ndtv.com/albums/ENTERTAINMENT/inception-leonardo/21.jpg"));
        // Banner
        inception.AddMedia(Media.Create(MediaType.BannerImage, "https://images7.alphacoders.com/586/thumb-1920-586904.jpg"));

        context.Movies.Add(inception);

        // 2. Interstellar
        var interstellar = Movie.Create("Interstellar", "A team of explorers travel through a wormhole in space...", 8.7m, 12, TimeSpan.FromMinutes(169), "USA", "Paramount", "English");
        interstellar.AddGenre(adventure);
        interstellar.AddGenre(drama);
        interstellar.AddGenre(scifi);
        interstellar.AddPerson(nolan);
        interstellar.AddPerson(mcconaughey);

        // Poster
        interstellar.AddMedia(Media.Create(MediaType.PosterImage, "https://upload.wikimedia.org/wikipedia/en/b/bc/Interstellar_film_poster.jpg"));
        // Trailer
        interstellar.AddMedia(Media.Create(MediaType.Video, "https://www.youtube.com/watch?v=zSWdZVtXT7E"));
        // Screenshots
        interstellar.AddMedia(Media.Create(MediaType.Image, "https://film-grab.com/wp-content/uploads/2015/04/35-512.jpg"));
        interstellar.AddMedia(Media.Create(MediaType.Image, "https://film-grab.com/wp-content/uploads/photo-gallery/12%20(514).jpg?bwg=1547222787"));
        interstellar.AddMedia(Media.Create(MediaType.Image, "https://film-grab.com/wp-content/uploads/photo-gallery/43%20(504).jpg?bwg=1547222787"));
        // Banner
        interstellar.AddMedia(Media.Create(MediaType.BannerImage, "https://images3.alphacoders.com/551/thumb-1920-551456.jpg"));
        
        context.Movies.Add(interstellar);

        // 3. Barbie
        var barbie = Movie.Create("Barbie", "Barbie suffers a crisis that leads her to question her world and her existence.", 7.0m, 12, TimeSpan.FromMinutes(114), "USA", "Warner Bros.", "English");
        barbie.AddGenre(comedy);
        barbie.AddGenre(adventure);
        barbie.AddPerson(gerwig);
        barbie.AddPerson(robbie);
        barbie.AddPerson(gosling);

        // Poster
        barbie.AddMedia(Media.Create(MediaType.PosterImage, "https://upload.wikimedia.org/wikipedia/en/0/0b/Barbie_2023_poster.jpg"));
        // Trailer
        barbie.AddMedia(Media.Create(MediaType.Video, "https://www.youtube.com/watch?v=pBk4NYhWNMM"));
        // Screenshots
        barbie.AddMedia(Media.Create(MediaType.Image, "https://film-grab.com/wp-content/uploads/photo-gallery/thumb/Barbie_19.jpg?bwg=1699879305"));
        barbie.AddMedia(Media.Create(MediaType.Image, "https://film-grab.com/wp-content/uploads/photo-gallery/thumb/Barbie_59.jpg?bwg=1699879305"));
        barbie.AddMedia(Media.Create(MediaType.Image, "https://film-grab.com/wp-content/uploads/photo-gallery/Barbie_52.jpg?bwg=1699879305"));
        // Banner
        barbie.AddMedia(Media.Create(MediaType.BannerImage, "https://images2.alphacoders.com/132/1320904.jpg"));

        context.Movies.Add(barbie);

        // 4. The Dark Knight
        var darkKnight = Movie.Create("The Dark Knight", "When the menace known as the Joker wreaks havoc and chaos on the people of Gotham...", 9.0m, 16, TimeSpan.FromMinutes(152), "USA", "Warner Bros.", "English");
        darkKnight.AddGenre(action);
        darkKnight.AddGenre(thriller);
        darkKnight.AddPerson(nolan);

        // Poster
        darkKnight.AddMedia(Media.Create(MediaType.PosterImage, "https://upload.wikimedia.org/wikipedia/en/1/1c/The_Dark_Knight_%282008_film%29.jpg"));
        // Trailer
        darkKnight.AddMedia(Media.Create(MediaType.Video, "https://www.youtube.com/watch?v=EXeTwQWrcwY"));
        // Screenshots
        darkKnight.AddMedia(Media.Create(MediaType.Image, "https://film-grab.com/wp-content/uploads/photo-gallery/thumb/10%20(1051).jpg?bwg=1547463350"));
        darkKnight.AddMedia(Media.Create(MediaType.Image, "https://film-grab.com/wp-content/uploads/photo-gallery/thumb/46%20(1027).jpg?bwg=1547463350"));
        darkKnight.AddMedia(Media.Create(MediaType.Image, "https://film-grab.com/wp-content/uploads/photo-gallery/thumb/23%20(1051).jpg?bwg=1547463351"));
        // Banner
        darkKnight.AddMedia(Media.Create(MediaType.BannerImage, "https://wallpapercat.com/w/full/b/0/8/37797-3840x2160-desktop-4k-the-dark-knight-wallpaper-image.jpg"));

        context.Movies.Add(darkKnight);

        // 5. Blade Runner 2049
        var bladeRunner = Movie.Create("Blade Runner 2049", "Young Blade Runner K's discovery of a long-buried secret leads him to track down former Blade Runner Rick Deckard...", 8.0m, 16, TimeSpan.FromMinutes(164), "USA", "Alcon", "English");
        bladeRunner.AddGenre(scifi);
        bladeRunner.AddGenre(drama);
        bladeRunner.AddPerson(gosling);

        // Poster
        bladeRunner.AddMedia(Media.Create(MediaType.PosterImage, "https://upload.wikimedia.org/wikipedia/en/9/9b/Blade_Runner_2049_poster.png"));
        // Trailer
        bladeRunner.AddMedia(Media.Create(MediaType.Video, "https://www.youtube.com/watch?v=gCcx85zbxz4"));
        // Screenshots
        bladeRunner.AddMedia(Media.Create(MediaType.Image, "https://film-grab.com/wp-content/uploads/photo-gallery/thumb/bladerunner062.jpg?bwg=1551282394"));
        bladeRunner.AddMedia(Media.Create(MediaType.Image, "https://film-grab.com/wp-content/uploads/photo-gallery/thumb/bladerunner026.jpg?bwg=1551282394"));
        bladeRunner.AddMedia(Media.Create(MediaType.Image, "https://film-grab.com/wp-content/uploads/photo-gallery/thumb/bladerunner022.jpg?bwg=1551282394"));
        bladeRunner.AddMedia(Media.Create(MediaType.Image, "https://film-grab.com/wp-content/uploads/photo-gallery/thumb/bladerunner056.jpg?bwg=1551282394"));
        //Banner
        bladeRunner.AddMedia(Media.Create(MediaType.BannerImage, "https://images4.alphacoders.com/866/thumb-1920-866590.jpg"));

        context.Movies.Add(bladeRunner);

        context.SaveChanges();
    }

    // ==========================================
    // PART 2: SESSIONS & PRICES
    // ==========================================
    private static void SeedSessionsAndPrices(CinemaDbContext context)
    {
        if (context.Sessions.Any()) return;

        // Retrieving data we just saved
        var redHall = context.Halls.First(h => h.HallName == "Red Hall");
        var blueHall = context.Halls.First(h => h.HallName == "Blue Hall");

        var standardType = context.SeatTypes.First(st => st.TypeName == "Standard");
        var vipType = context.SeatTypes.First(st => st.TypeName == "VIP");

        var inception = context.Movies.First(m => m.Name == "Inception");
        var interstellar = context.Movies.First(m => m.Name == "Interstellar");
        var barbie = context.Movies.First(m => m.Name == "Barbie");

        var tomorrow = DateTime.UtcNow.Date.AddDays(1);

        // Creating Sessions
        var s1 = Session.Create(inception.Id, redHall.Id, tomorrow.AddHours(10), MovieFormat.TwoD); // Inception 10:00
        var s2 = Session.Create(interstellar.Id, blueHall.Id, tomorrow.AddHours(14), MovieFormat.TwoD); // Interstellar 14:00
        var s3 = Session.Create(barbie.Id, redHall.Id, tomorrow.AddHours(18), MovieFormat.TwoD); // Barbie 18:00

        context.Sessions.AddRange(s1, s2, s3);
        context.SaveChanges();

        // Creating Prices
        var prices = new List<TypePrice>
        {
            // Inception
            TypePrice.Create(s1.Id, standardType.Id, 150m),
            TypePrice.Create(s1.Id, vipType.Id, 250m),

            // Interstellar
            TypePrice.Create(s2.Id, standardType.Id, 160m),
            TypePrice.Create(s2.Id, vipType.Id, 280m),

            // Barbie
            TypePrice.Create(s3.Id, standardType.Id, 200m),
            TypePrice.Create(s3.Id, vipType.Id, 350m)
        };

        context.TypePrices.AddRange(prices);
        context.SaveChanges();
    }

    // ==========================================
    // PART 3: SECURITY (ROLES, PERMISSIONS, USERS)
    // ==========================================
    private static void SeedSecurity(CinemaDbContext context, IPasswordHasher passwordHasher)
    {
        if (context.Roles.Any()) return;

        // 1. Permissions
        var allPermissions = new List<Permission>
        {
            Permission.Create(PermissionCode.Create("movies.read")),
            Permission.Create(PermissionCode.Create("movies.manage")),

            Permission.Create(PermissionCode.Create("sessions.read")),
            Permission.Create(PermissionCode.Create("sessions.manage")),

            Permission.Create(PermissionCode.Create("halls.read")),
            Permission.Create(PermissionCode.Create("halls.manage")),

            Permission.Create(PermissionCode.Create("genres.read")),
            Permission.Create(PermissionCode.Create("genres.manage")),

            Permission.Create(PermissionCode.Create("tickets.create")),
            Permission.Create(PermissionCode.Create("tickets.read_all")),
            Permission.Create(PermissionCode.Create("tickets.manage")),

            Permission.Create(PermissionCode.Create("users.manage")),


        };

        if (!context.Permissions.Any())
        {
            context.Permissions.AddRange(allPermissions);
            context.SaveChanges();
        }

        // 2. Roles
        var adminRole = Role.Create("Admin");
        var userRole = Role.Create("User");

        // 3. Grant Permissions

        // ADMIN - Gets everything
        foreach (var p in allPermissions) adminRole.Grant(p);

        // USER - Read only
        foreach (var p in allPermissions)
        {
            if (p.Code.Value.Contains(".read")) userRole.Grant(p);
            if (p.Code.Value.Contains("tickets.create")) userRole.Grant(p);
        }

        context.Roles.AddRange(adminRole, userRole);
        context.SaveChanges();

        // 4. Create Default Users (Optional but helpful for testing)
        if (!context.Users.Any())
        {
            // Using a dummy hash for example purposes.

            // Admin User
            var adminUser = User.Create(
                "admin",
                passwordHasher.Hash("admin123"),
                "admin@absolutecinema.com"
            );
            adminUser.AddRole(adminRole);

            // Standard User
            var simpleUser = User.Create(
                "user",
                passwordHasher.Hash("user123"),
                "user@absolutecinema.com"
            );
            simpleUser.AddRole(userRole);

            context.Users.AddRange(adminUser, simpleUser);
            context.SaveChanges();
        }
    }

    // Helper method for Halls
    private static Hall CreateHallWithSeats(CinemaDbContext context, string name, int rows, int seatsPerRow, SeatType standard, SeatType vip)
    {
        var hall = Hall.Create(name, rows, seatsPerRow);
        context.Halls.Add(hall);

        var seats = new List<Seat>();
        for (short r = 1; r <= rows; r++)
        {
            for (short s = 1; s <= seatsPerRow; s++)
            {
                var type = (r > rows - 2) ? vip : standard;
                seats.Add(Seat.Create(hall.Id, r, s, type.Id));
            }
        }
        context.Seats.AddRange(seats);
        return hall;
    }
}