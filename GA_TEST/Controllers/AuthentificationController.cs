using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GA_TEST.Controllers


{

	[Route("(api/authentification")]
	[ApiController]
	public class AuthentificationController : ControllerBase
	{
        private readonly IConfiguration _configuration;
		public class AuthentificationRequestBody
		{
			public string? UserName { get; set; }

                public string? Password
            {
                get; set; }
            }

        private class CityInfoUser
        {
            public int UserId { get; set; }
            public string? UserName { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? City { get; set; }

            public CityInfoUser(int userId, string userName, string firstName, string lastName, string city)
            {
                UserId = userId;
                UserName = userName;
                FirstName = firstName;
                LastName = lastName;
                City = city;
            }

        }

        public AuthentificationController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost("authentificate")]
		public ActionResult<string> Authenticate(AuthentificationRequestBody authentificationRequestBody)
		{
			var user = ValidateUserCredentials(
				authentificationRequestBody.UserName,
				authentificationRequestBody.Password);
			if(user == null)
			{
				return Unauthorized();
            }

            var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(_configuration["Authentification:SecretForKey"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsForTokens = new List<Claim>();
            claimsForTokens.Add(new Claim("sub", user.UserId.ToString()));
            claimsForTokens.Add(new Claim("given_name", user.FirstName));
            claimsForTokens.Add(new Claim("family_name", user.LastName));
            claimsForTokens.Add(new Claim("city", user.City));
            var jwtSecuriotyToken = new JwtSecurityToken(
                _configuration["Authentification:Issuer"],
                _configuration["Authentification:Audience"],
                claimsForTokens,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials
                );
            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecuriotyToken);

            return Ok(tokenToReturn);
        }




        private CityInfoUser ValidateUserCredentials(string? userName, string? password)
        {
            
			return new CityInfoUser(
				1, userName ?? "", "Yuliya", "Buiko", "Plantation");
        }
    }
}

