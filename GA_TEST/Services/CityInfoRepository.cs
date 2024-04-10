using System;
using Microsoft.EntityFrameworkCore;
using GA_TEST.DbContexts;
using GA_TEST.Entities;

namespace GA_TEST.Services
{
	public class CityInfoRepository : ICityInfoRepository
	{
        private readonly CityInfoContext _context;

		public CityInfoRepository(CityInfoContext context)
		{
            _context = context ?? throw new ArgumentNullException(nameof(context));
		}

        public async Task<bool> CityNameMatchesCityId(string? cityName, int cityId)
        {
            return await _context.Cities.AnyAsync(c => c.Id == cityId && c.Name == cityName);
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()

        {
            var cities = await _context.Cities.OrderBy(c => c.Name).ToListAsync();
            Console.WriteLine(cities);
            return  cities;
        }

        public async Task<City?> GetCityAsync(int cityId, bool includePointOfInterest)
        {
            if (includePointOfInterest)
            {
                return await _context.Cities.Include(c => c.PointsOfInterest)
                    .Where(c => c.Id == cityId).FirstOrDefaultAsync();
            }
            return await _context.Cities.Where(c => c.Id == cityId).FirstOrDefaultAsync();
        }

        public async Task<bool>CityExistsAsync(int cityId)
        {
            return await _context.Cities.AnyAsync(c => c.Id == cityId);
        }

        public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterest)
        {
            return await _context.PointOfInterest.Where(p => p.CityId == cityId && p.Id == pointOfInterest).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId)
        {
            return await _context.PointOfInterest.Where(p => p.CityId == cityId).ToListAsync();
        }

        public async Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest)
        {
            var city = await GetCityAsync(cityId, false);

            if (city != null)
            {
                city.PointsOfInterest.Add(pointOfInterest);
            }
        }

        public void DeletePointOfInterest(PointOfInterest pointOfInterest)
        {
            _context.Remove(pointOfInterest);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        
    }
}

