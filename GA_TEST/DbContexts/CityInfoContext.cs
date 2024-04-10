using System;
using GA_TEST.Entities;
using Microsoft.EntityFrameworkCore;

namespace GA_TEST.DbContexts
{
	public class CityInfoContext : DbContext
	{

        public CityInfoContext(DbContextOptions<CityInfoContext> options)
        : base(options)
        {
        }
        public DbSet<City> Cities { get; set; }
		public DbSet<PointOfInterest> PointOfInterest { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>()
                .HasData(
                new City("New York")
                {
                    Id = 1,
                    Description = "the big apple"
                },
                new City("Antwerp")
                {
                    Id = 2,
                    Description = "the one with the cathedral"
                },
                new City("Paris")
                {
                    Id = 3,
                    Description = "the one with big tower"
                });

            modelBuilder.Entity<PointOfInterest>()
                .HasData(
                new PointOfInterest("Central Park")
                {
                    Id = 1,
                    CityId = 1,
                    Description = "The most visited urban park in US"
                },
                new PointOfInterest("Empire State Building")
                {
                    Id = 2,
                    CityId = 1,
                    Description = "A 102-story skyscrapper located in Midtown Manhattan"
                },
                new PointOfInterest("Cathedral of our lady")
                {
                    Id = 3,
                    CityId = 2,
                    Description = "A Gothic style cathedral"
                },
                new PointOfInterest("Antwerp Central Station")
                {
                    Id = 4,
                    CityId = 2,
                    Description = "The finest example of railway atchitecture in Belgium"
                },
                new PointOfInterest("Eiffel Tower")
                {
                    Id = 5,
                    CityId = 3,
                    Description = "A wrought aron lattice tower on the Champ de Mars."
                },
                new PointOfInterest("The Louvre")
                {
                    Id = 6,
                    CityId = 3,
                    Description = "The world's largest museum."
                }
                );
            base.OnModelCreating(modelBuilder); 
        }

    }
}

