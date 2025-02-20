namespace SurveyApp.Api.Entities
{

    [Owned]
    public class RefreshToken
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiryOn { get; set; }
        public DateTime CreateOn { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedOn { get; set; }

        public bool IsExpired => DateTime.UtcNow > ExpiryOn;
        public bool IsActive => RevokedOn is null && !IsExpired;
    }
}
