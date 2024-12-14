namespace SurveyApp.Api.Contracts.Authentication;

public record AuthResponse(
	string Id, 
	string Email,
	String FirstName,
	string LastName,
	string Token,
	int ExpiresIn,
	string RefreshToken,
	DateTime RefreshTokenExpiration
);
