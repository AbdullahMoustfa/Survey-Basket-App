using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SurveyApp.Api.Authentication
{
	public class JwtOptions
	{
		public static string SectionName = "Jwt";

		[Required]
        public string Key { get; init; } = string.Empty;

		[Required]
		public string Issure { get; init; } = string.Empty;

		[Required]
		public string Audience { get; init; } = string.Empty;

		[Required]
		[Range(1, int.MaxValue)]
		public int ExpiryMinutes { get; init; }
	}
}
