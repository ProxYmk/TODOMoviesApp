using System;
using Microsoft.EntityFrameworkCore;

namespace Movies.Models
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options)
            : base(options)
        {
        }

        public DbSet<Movies.Models.Movie> Movie { get; set; }
        public DbSet<Movies.Models.Genre> Genre { get; set; }
    }
}