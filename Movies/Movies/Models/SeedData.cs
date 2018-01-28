using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Movies.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MovieContext(
                serviceProvider.GetRequiredService<DbContextOptions<MovieContext>>()))
            {
                // Look for any movies.
                if (context.Genre.Any())
                {
                    return;   // DB has been seeded
                }

                context.Genre.AddRange(
                    new Genre { Name = "Comedy"},
                    new Genre { Name = "Romantic Comedy"},
                    new Genre { Name = "Western"}
                );
                context.SaveChanges();

                context.Movie.AddRange(
                    new Movie {Title = "When Harry Met Sally",ReleaseDate = DateTime.Parse("1989-1-11"),GenreID = 1,Rating = "R",Price = 7.99M},
                    new Movie {Title = "Ghostbusters ",ReleaseDate = DateTime.Parse("1984-3-13"),GenreID = 2,Rating = "P", Price = 8.99M},
                    new Movie {Title = "Ghostbusters 2",ReleaseDate = DateTime.Parse("1986-2-23"),GenreID = 1,Rating = "P",Price = 9.99M},
                    new Movie {Title = "Rio Bravo",ReleaseDate = DateTime.Parse("1959-4-15"),GenreID = 3,Rating = "R",Price = 3.99M}
                );
                context.SaveChanges();

                context.Actor.AddRange(
                    new Actor {Name = "Actor1", LastName = "LastName1"},
                    new Actor { Name = "Actor2", LastName = "LastName2" },
                    new Actor { Name = "Actor3", LastName = "LastName3" }
                );
                context.SaveChanges();

                context.MovieActor.AddRange(
                    new MovieActor { MovieID = 1, ActorID = 1},
                    new MovieActor { MovieID = 1, ActorID = 2 },
                    new MovieActor { MovieID = 1, ActorID = 3 },
                    new MovieActor { MovieID = 2, ActorID = 1 },
                    new MovieActor { MovieID = 2, ActorID = 2 }
                );
                context.SaveChanges();
            }
        }
    }
}
