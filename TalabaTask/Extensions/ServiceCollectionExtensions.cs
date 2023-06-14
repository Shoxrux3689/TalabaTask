using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TalabaTask.Extensions;

public static class ServiceCollectionExtensions
{
	public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
		{
			var signingKey = Encoding.UTF32.GetBytes(configuration.GetSection("JwtOptions:SignIngKey").Value);

			options.TokenValidationParameters = new TokenValidationParameters()
			{
				ValidIssuer = configuration.GetSection("JwtOptions:ValidIssuer").Value,
				ValidAudience = configuration.GetSection("JwtOptions:ValidAudience").Value,
				ValidateIssuer = true,
				ValidateAudience = true,
				IssuerSigningKey = new SymmetricSecurityKey(signingKey),
				ValidateIssuerSigningKey = true,
				ValidateLifetime = true,
				ClockSkew = TimeSpan.Zero
			};
		});
	}
}
