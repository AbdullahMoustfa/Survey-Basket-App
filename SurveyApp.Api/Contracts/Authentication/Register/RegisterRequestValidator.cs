using Microsoft.AspNetCore.Identity.Data;
using SurveyApp.Api.Contracts.Polls;
using SurveyApp.Api.Properties.Abstractions.Consts;

namespace SurveyApp.Api.Contracts.Authentication.Register

{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .Matches(RegexPattern.Password)
                .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase");

			RuleFor(x => x.FirstName)
				.NotEmpty()
				.Length(3, 100);

			RuleFor(x => x.LastName)
				.NotEmpty()
		        .Length(3, 100);
		}
    }
}
