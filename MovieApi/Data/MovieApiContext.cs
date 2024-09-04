using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieApi.Models.Entities;

namespace MovieApi.Data
{
    public class MovieApiContext : DbContext
    {
        public MovieApiContext (DbContextOptions<MovieApiContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movie => Set<Movie>();
        public DbSet<Genre> Genre => Set<Genre>();
        public DbSet<Actor> Actor => Set<Actor>();
        public DbSet<Director> Director => Set<Director>();
        public DbSet<ContactInformation> ContactInformation => Set<ContactInformation>();
    }
}
