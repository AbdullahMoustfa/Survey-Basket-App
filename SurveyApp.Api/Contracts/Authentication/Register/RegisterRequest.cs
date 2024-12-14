﻿namespace SurveyApp.Api.Contracts.Authentication.Register;

public record RegisterRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName
);
