﻿using System;
using AutoMapper;
using GA_TEST.Models;
using GA_TEST.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GA_TEST.Controllers
{
	[ApiController]
	//[Authorize]
	[Route("api/cities")]
	public class CityController : ControllerBase //Base
	{
		private readonly ICityInfoRepository _cityInfoRepository;
		private readonly IMapper _mapper;

		public CityController(ICityInfoRepository cityInfoRepository, IMapper mapper)
		{
			_cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities()
		{
			var cityEntities = await _cityInfoRepository.GetCitiesAsync();

			return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
		{
			var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);
			if(city == null)
			{
				return NotFound();
			}

			if (includePointsOfInterest)
			{
				return Ok(_mapper.Map<CityDto>(city));
			}
			return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
		}
	}

}
