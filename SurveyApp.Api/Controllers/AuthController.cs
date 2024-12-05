using Microsoft.Extensions.Options;
using SurveyApp.Api.Authentication;

namespace SurveyApp.Api.Controllers;


[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
	private readonly IAuthService _authService;
	private readonly JwtOptions _jwtOptions;

	public AuthController(IAuthService authService,
		IOptions<JwtOptions> jwtOptions)
	{
		_authService = authService;
		_jwtOptions = jwtOptions.Value;
	}


	[HttpPost]
	public async Task<IActionResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
	{
		var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

		if (authResult is null)
			return BadRequest("Invalid Email/Password!!!");

		return Ok(authResult);
	}

}
