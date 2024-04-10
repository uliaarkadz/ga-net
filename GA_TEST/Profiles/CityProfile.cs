using System;
using AutoMapper;

namespace GA_TEST.Profiles
{
	public class CityProfile : Profile
	{
		public CityProfile()
		{
			CreateMap<Entities.City, Models.CityWithoutPointsOfInterestDto>();
            CreateMap<Entities.City, Models.CityDto>();

        }
	}
}

