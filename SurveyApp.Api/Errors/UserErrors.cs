﻿namespace SurveyApp.Api.Errors
{
    public class UserErrors
    {
        public static readonly Error InvalidCredentials 
            = new("User.InvalidCredentials", "Invalid Email/Password");

        public static readonly Error InvalidJwtToken 
            = new("User.InvalidJwtToken", "Invalid Jwt Token");

        public static readonly Error InvalidRefreshToken
            = new("User.InvalidRefreshToken", "Invalid Refresh Token");
    }
}
