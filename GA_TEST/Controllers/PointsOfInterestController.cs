using AutoMapper;
using GA_TEST.Models;
using GA_TEST.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GA_TEST.Controllers
{
    [Route("api/cities/{cityId}/pointsofintrest")]
    //[Authorize(Policy = "MustBeFromAntwerp")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly ILocalMailService _localMailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, ILocalMailService localMailService, ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _localMailService = localMailService ?? throw new ArgumentNullException(nameof(localMailService));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
           
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointsOfInterestDto>>> GetPointsOfInterest(int cityId)
        {

            var cityName = User.Claims.FirstOrDefault(c => c.Type == "city")?.Value;
            if(!await _cityInfoRepository.CityNameMatchesCityId(cityName, cityId))
                {
                return Forbid();
            }

            if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"City with id {cityId} was not found");
                return NotFound();
            }
            var pointsOfInterestForCity = await _cityInfoRepository.GetPointsOfInterestForCityAsync(cityId);

            return Ok(_mapper.Map<IEnumerable<PointsOfInterestDto>>(pointsOfInterestForCity));
        }

        [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<IEnumerable<PointsOfInterestDto>>> GetPointOfInterest(int cityId, int pointofinterestid)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"City with id {cityId} was not found");
                return NotFound();
            }

            var pointOfInterest = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointofinterestid);

            if(pointOfInterest == null)
            {
                return NotFound();
            }
           
            return Ok(_mapper.Map<PointsOfInterestDto>(pointOfInterest));
        }

        [HttpPost]
        public async Task<ActionResult<PointsOfInterestDto>> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterest)
        {
            if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var finalPointOfIneterest = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);


            await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, finalPointOfIneterest);

            await _cityInfoRepository.SaveChangesAsync();

            var createdPointOfInterestReturn = _mapper.Map<Models.PointsOfInterestDto>(finalPointOfIneterest);
            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityId = cityId,
                    pointOfInterest = createdPointOfInterestReturn.Id
                }, createdPointOfInterestReturn);
        }

        [HttpPut("{pointofinterestid}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointofinterestid,
            PointOfInterestForUpdateDto pointOfInterest)
        {

            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointofinterestid);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(pointOfInterest, pointOfInterestEntity);

            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{pointofinterestid}")]
        public async Task <ActionResult> PartiallyUpdatePointOfInterest(int cityId, int pointofinterestid,
           JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {

            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointofinterestid);

            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);

            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{pointofinterestid}")]
        public async Task<ActionResult> DeletePoinOfInterest(int cityId, int pointofinterestid)
        {

            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointofinterestid);

            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);

            await _cityInfoRepository.SaveChangesAsync();

            _localMailService.Send("Point of interest deleted.", $"Point of int woth {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} was deleted.");
            return NoContent();
        }
    }
}

