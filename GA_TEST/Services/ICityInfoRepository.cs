using System;
using GA_TEST.Entities;

namespace GA_TEST.Services
{
	public interface ICityInfoRepository
	{
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<City?> GetCityAsync(int cityId, bool includePointOfInterest);
        Task<bool> CityExistsAsync(int cityId);
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId);
        Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterest);
        Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);
        void DeletePointOfInterest(PointOfInterest pointOfInterest);
        Task<bool> CityNameMatchesCityId(string? cityName, int cityId);
        Task<bool> SaveChangesAsync();
    }
}

