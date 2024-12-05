using SurveyApp.Api.Authentication;

namespace SurveyApp.Api.Services;

public class AuthService : IAuthService
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly IJwtProvider _jwtProvider;

	public AuthService(UserManager<ApplicationUser> userManager,
		IJwtProvider jwtProvider)

    {
		_userManager = userManager;
		_jwtProvider = jwtProvider;
	}
        public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
	{
		// check user
		var user = await _userManager.FindByEmailAsync(email);

		if(user is null)
			return null;

		// check password
		var isValidated = await _userManager.CheckPasswordAsync(user, password);
		
		if( !isValidated )
			return null;

		// generate jwt token

		var (token, expiresIn) = _jwtProvider.GenerateToken(user);
		return new AuthResponse(Guid.NewGuid().ToString(), "test@io.com", "Abdullah", "Mostafa",token, expiresIn);
	}
}
