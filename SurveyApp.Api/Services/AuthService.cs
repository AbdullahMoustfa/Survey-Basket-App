using SurveyApp.Api.Authentication;
using System.Security.Cryptography;

namespace SurveyApp.Api.Services;

public class AuthService : IAuthService
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly IJwtProvider _jwtProvider;

	private readonly int _refreshTokenExpiryDays = 14;

	public AuthService(UserManager<ApplicationUser> userManager,
		IJwtProvider jwtProvider)
    {
		_userManager = userManager;
		_jwtProvider = jwtProvider;
	}
    public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
	{
		// check user
		var user = await _userManager.FindByEmailAsync(email);

		if (user is null)
			return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

		// check password
		var isValidated = await _userManager.CheckPasswordAsync(user, password);
		
		if( !isValidated )
			 return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials); 

		// generate jwt token
		var (token, expiresIn) = _jwtProvider.GenerateToken(user);

		// generate jwt refresh-token
		var refreshToken = GenerateRefreshToken();

		var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

		user.RefreshTokens.Add(new RefreshToken
		{
			Token = refreshToken,
            ExpiryOn = refreshTokenExpiration
		});

		await _userManager.UpdateAsync(user);

		var response = new AuthResponse(user.Id, user.Email!, user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenExpiration);

        return Result.Success(response);	
	}

	public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
	{
		var userId = _jwtProvider.ValidateToken(token);

		if( userId is null ) 
			return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

		var user = await _userManager.FindByIdAsync(userId);

		if( user is null )
            return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

        var userRefreshToken =  user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

		userRefreshToken.RevokedOn = DateTime.UtcNow;

		var (newToken, expiresIn) = _jwtProvider.GenerateToken(user);

		// generate jwt refresh-token
		var newRefreshToken = GenerateRefreshToken();

		var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

		user.RefreshTokens.Add(new RefreshToken
		{
			Token = newRefreshToken,
            ExpiryOn = refreshTokenExpiration
		});

		await _userManager.UpdateAsync(user);

        var response = new AuthResponse(user.Id, user.Email!, user.FirstName, user.LastName, newToken, expiresIn, newRefreshToken, refreshTokenExpiration);
		
			return Result.Success(response);	

    }

	public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
	{
		var userId = _jwtProvider?.ValidateToken(token);	

		if( userId is null )
			return Result.Failure(UserErrors.InvalidJwtToken);

		var user = await _userManager.FindByIdAsync(userId);	

		if( user is null )
            return Result.Failure(UserErrors.InvalidJwtToken);

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);


		if (userRefreshToken is null)
            return Result.Failure(UserErrors.InvalidRefreshToken);

        userRefreshToken.RevokedOn = DateTime.UtcNow;	

		await _userManager.UpdateAsync(user);	

		return Result.Success();	
			
	}

	private static string GenerateRefreshToken()
	{
		return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
	}

	
}
