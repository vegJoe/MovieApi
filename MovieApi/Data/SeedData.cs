using Bogus;
using MovieApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MovieApi.Data
{
    public static class SeedData
    {
        public static async Task SeedDataAsync(this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                var servicesProvider = scope.ServiceProvider;
                var db = servicesProvider.GetRequiredService<MovieApiContext>();

                await db.Database.MigrateAsync();

                if (await db.Movie.AnyAsync()) return; // If data exists, don't seed again

                try
                {
                    // Generate and add ContactInformation data first
                    var contactInformations = GenerateContactInformations(10);
                    await db.AddRangeAsync(contactInformations);
                    await db.SaveChangesAsync(); // Save to ensure IDs are generated

                    // Generate genres
                    var genres = GenerateGenres(5);
                    await db.AddRangeAsync(genres);
                    await db.SaveChangesAsync();

                    // Generate and add Directors after ContactInformation is saved
                    var directors = GenerateDirectors(5, contactInformations);
                    await db.AddRangeAsync(directors);
                    await db.SaveChangesAsync();

                    // Generate Actors
                    var actors = GenerateActors(10);
                    await db.AddRangeAsync(actors);
                    await db.SaveChangesAsync();

                    // Generate Movies after Directors are saved
                    var movies = GenerateMovies(10, directors);
                    await db.AddRangeAsync(movies);
                    await db.SaveChangesAsync();

                    // Assign actors to movies and genres to movies
                    AssignActorsToMovies(db, actors, movies);
                    AssignGenresToMovies(db, genres, movies);

                    // Save the changes
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message); // Log error for debugging
                    throw;
                }
            }
        }

        private static List<ContactInformation> GenerateContactInformations(int count)
        {
            var faker = new Faker<ContactInformation>()
                .RuleFor(ci => ci.Email, f => f.Internet.Email())
                .RuleFor(ci => ci.Phonenumber, f => f.Random.Int(1000000, 9999999));

            return faker.Generate(count);
        }

        private static List<Genre> GenerateGenres(int count)
        {
            var faker = new Faker<Genre>()
                .RuleFor(g => g.Name, f => f.Commerce.Categories(1).First());

            return faker.Generate(count);
        }

        private static List<Director> GenerateDirectors(int count, List<ContactInformation> contactInformations)
        {
            var contactInformationIds = contactInformations.Select(ci => ci.Id).ToList();

            var faker = new Faker<Director>()
                .RuleFor(d => d.Name, f => f.Name.FullName())
                .RuleFor(d => d.ContactInformationId, f => f.PickRandom(contactInformations).Id); // Pick valid IDs from saved ContactInformation

            return faker.Generate(count);
        }

        private static List<Actor> GenerateActors(int count)
        {
            var faker = new Faker<Actor>()
                .RuleFor(a => a.Name, f => f.Name.FullName())
                .RuleFor(a => a.DateOfBirth, f => f.Date.Past(90).Year); // Random DateOfBirth in past 30 years

            return faker.Generate(count);
        }

        private static List<Movie> GenerateMovies(int count, List<Director> directors)
        {
            var directorIds = directors.Select(d => d.Id).ToList();

            var faker = new Faker<Movie>()
                .RuleFor(m => m.Title, f => f.Lorem.Sentence(3))
                .RuleFor(m => m.Rating, f => f.Random.Int(1, 10))
                .RuleFor(m => m.ReleaseDate, f =>
                {
                    var date = f.Date.Past(100);
                    return date.Year * 10000 + date.Month * 100 + date.Day;
                })
                
                .RuleFor(m => m.Description, f => f.Lorem.Paragraph())
                .RuleFor(m => m.DirectorId, f => f.PickRandom(directors).Id); // Pick valid IDs from saved Directors

            return faker.Generate(count);
        }

        private static void AssignActorsToMovies(MovieApiContext db, List<Actor> actors, List<Movie> movies)
        {
            var faker = new Faker();
            foreach (var movie in movies)
            {
                // Randomly assign 1 to 3 actors to each movie
                var selectedActors = faker.PickRandom(actors, faker.Random.Int(1, 3)).ToList();
                movie.Actors = selectedActors;
            }

            db.Movie.UpdateRange(movies);
        }

        private static void AssignGenresToMovies(MovieApiContext db, List<Genre> genres, List<Movie> movies)
        {
            var faker = new Faker();
            foreach (var movie in movies)
            {
                // Randomly assign 1 to 2 genres to each movie
                var selectedGenres = faker.PickRandom(genres, faker.Random.Int(1, 2)).ToList();
                movie.Genres = selectedGenres;
            }

            db.Movie.UpdateRange(movies);
        }
    }
}