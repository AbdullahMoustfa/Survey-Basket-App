namespace SurveyApp.Api.Contracts.Authentication.RefreshToken;
public record RefreshTokenRequest(
	string Token,
	string RefreshToken
);

