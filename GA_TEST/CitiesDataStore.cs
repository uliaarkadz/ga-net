using System;
using GA_TEST.Models;

namespace GA_TEST
{
	public class CitiesDataStore
	{
		public List<CityDto> Cities { get; set; }
		//public static CitiesDataStore Current { get; } = new CitiesDataStore();

		public CitiesDataStore()
		{
			Cities = new List<CityDto>(){
				new CityDto()
				{
					Id = 1,
					Name = "New York City",
					Description = "the big apple",
					PointsOfInterests = new List<PointsOfInterestDto>()
					{
						new PointsOfInterestDto(){
						Id = 1,
						Name = "Central Park",
						Description = "The most visited urban park in US"},
                        new PointsOfInterestDto(){
                        Id = 2,
                        Name = "Empire State Building",
                        Description = "A 102-story skyscrapper located in Midtown Manhattan"}
                    }
				},
				new CityDto()
				{
					Id = 2,
					Name = "Antwerp",
					Description = "the one with the cathedral",
                    PointsOfInterests = new List<PointsOfInterestDto>()
                    {
                        new PointsOfInterestDto(){
                        Id = 3,
                        Name = "Cathedral of our lady",
                        Description = "A Gothic style cathedral"},
                        new PointsOfInterestDto(){
                        Id = 4,
                        Name = "Antwerp Central Station",
                        Description = "The finest example of railway atchitecture in Belgium"}
                    }
                },
				new CityDto()
				{
					Id = 3,
					Name = "Paris",
					Description = "the one with big tower",
                    PointsOfInterests = new List<PointsOfInterestDto>()
                    {
                        new PointsOfInterestDto(){
                        Id = 5,
                        Name = "Eiffel Tower",
                        Description = "A wrought aron lattice tower on the Champ de Mars."},
                        new PointsOfInterestDto(){
                        Id = 6,
                        Name = "The Louvre",
                        Description = "The world's largest museum."}
                    }
                }

			};
		}

    }
}

