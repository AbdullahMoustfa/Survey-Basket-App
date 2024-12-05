namespace SurveyApp.Api.Authentication;

public interface IJwtProvider
{
	(string token, int expireIn) GenerateToken(ApplicationUser user);	
}
