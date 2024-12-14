using Microsoft.Extensions.Options;
using SurveyApp.Api.Authentication;
using SurveyApp.Api.Contracts.Authentication.Login;
using SurveyApp.Api.Contracts.Authentication.RefreshToken;
using SurveyApp.Api.Contracts.Authentication.Register;

namespace SurveyApp.Api.Controllers;


[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
	private readonly IAuthService _authService;
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly JwtOptions _jwtOptions;

	public AuthController(IAuthService authService,
		UserManager<ApplicationUser> userManager)
	{
		_authService = authService;
		_userManager = userManager;
	}



	[HttpPost]
	public async Task<ActionResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
	{
		var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);


		return authResult is null? BadRequest("Invalid Email/Password!!!"):Ok(authResult);
	}

	[HttpPost("refresh")]
	public async Task<IActionResult> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
	{
		var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

		return authResult is null? BadRequest("Invalid Token!!"):Ok(authResult);
	}

	[HttpPost("revoke-refresh-token")]
	public async Task<IActionResult> RevokeRefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
	{
		var isRevoked = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

		return isRevoked ? Ok() : BadRequest("The operation could not be completed.");

	}
}
