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
        public DbSet<Movies.Models.Actor> Actor { get; set; }
        public DbSet<Movies.Models.MovieActor> MovieActor { get; set; }
        public DbSet<Movies.Models.Genre> Genre { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieActor>()
                        .HasKey(ma => new { ma.MovieID, ma.ActorID });

            modelBuilder.Entity<MovieActor>()
                        .HasOne(ma => ma.Movie)
                        .WithMany(b => b.MovieActors)
                        .HasForeignKey(ma => ma.MovieID);

            modelBuilder.Entity<MovieActor>()
                        .HasOne(ma => ma.Actor)
                        .WithMany(c => c.MovieActors)
                        .HasForeignKey(ma => ma.ActorID);
        }
    }
}