using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SurveyApp.Api.Authentication;

public class JwtProvider : IJwtProvider
{
	private readonly JwtOptions _options;

	public JwtProvider(IOptions<JwtOptions> options)
    {
		_options = options.Value;
	}
    public (string token, int expireIn) GenerateToken(ApplicationUser user)
	{
	
		// 1) Add Claim which returned in Token
		Claim[] claims = [

			new Claim(JwtRegisteredClaimNames.Sub, user.Id),
			new Claim(JwtRegisteredClaimNames.Email, user.Email!),
			new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
			new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			];

		// 2) Generate Key which responsible for Encoding and Decoding
		var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

		var signingCredential = new SigningCredentials(symmetricSecurityKey,SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			issuer: _options.Issure,
			audience: _options.Audience,
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
			signingCredentials: signingCredential
			);

		// 3) Return Token
		return (token: new JwtSecurityTokenHandler().WriteToken(token), _options.ExpiryMinutes * 60);

	}

	// Method Which Implemented Refresh Token
	public string? ValidateToken(string token)
	{
		//1. Take an instance from class JwtSecurityHandlerToken()
		var tokenHandler = new JwtSecurityTokenHandler();

		//2. Key which used in Encoding to Make Decoding
		var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
		
		//3. Detected if Happened any problem while Encoding
		try
		{
			tokenHandler.ValidateToken(token, new TokenValidationParameters
			{
				IssuerSigningKey = symmetricSecurityKey,
				ValidateIssuerSigningKey = true,
				ValidateIssuer = false,
				ValidateAudience = false,
				ClockSkew = TimeSpan.Zero
			}, out SecurityToken validatedToken);

			var jwtToken = (JwtSecurityToken) validatedToken;

			return jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
		}

		catch
		{
			return null;	
		}


	}
}
