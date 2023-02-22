namespace EmployeeManagement.UI.Models
{
    public class OAuthModel
    {
        public string TokenUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
        public string GrantType { get; set; }
    }

    public class TokenResponse
    {
        public string access_token { get; set; }
        public string ext_expires_in { get; set; }
        public string expires_in { get; set; }
        public string token_type { get; set; }

    }
}
