using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TalabaTask.Entities;

namespace TalabaTask.Services;

public class JwtServiceStudent
{
	private readonly IConfiguration configuration;

	public JwtServiceStudent(IConfiguration _configuration)
	{
		configuration = _configuration;
	}
	public string GenerateToken(Student student)
	{
		var claims = new List<Claim>()
		{
			new Claim(ClaimTypes.NameIdentifier, student.Id.ToString()),
			new Claim(ClaimTypes.Name, student.FirstName),
		};

		var signingKey = Encoding.UTF32.GetBytes(configuration.GetSection("JwtOptions:SignIngKey").Value);
		var security = new JwtSecurityToken(
			issuer: configuration.GetSection("JwtOptions:ValidIssuer").Value,
			audience: configuration.GetSection("JwtOptions:ValidAudience").Value,
			claims: claims,
			expires: DateTime.Now.AddMinutes(Convert.ToInt64(configuration.GetSection("JwtOptions:ExpiresMinutes").Value)),
			signingCredentials: new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256)
			);

		var token = new JwtSecurityTokenHandler().WriteToken(security);

		return token;
	}
}
