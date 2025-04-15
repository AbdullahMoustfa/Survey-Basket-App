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



	[HttpPost("")]
	public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
	{
		var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

		return authResult.IsSuccess
			   ? Ok(authResult.Value)
			   : authResult.ToProblem(StatusCodes.Status400BadRequest);
    }

	[HttpPost("refresh")]
	public async Task<IActionResult> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
	{
        var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return authResult.IsSuccess 
			? Ok(authResult.Value)
            : authResult.ToProblem(StatusCodes.Status400BadRequest);
    }

    [HttpPost("revoke-refresh-token")]
	public async Task<IActionResult> RevokeRefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
	{
		var result = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
       
		return result.IsSuccess 
			? Ok() 
			: result.ToProblem(StatusCodes.Status400BadRequest);

    }
}
