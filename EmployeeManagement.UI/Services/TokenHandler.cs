using EmployeeManagement.UI.Models;
using Newtonsoft.Json;

namespace EmployeeManagement.UI.Services
{
    public static class TokenHandler
    {
        internal static async Task<string> GetOAuthToken(IConfiguration configuration, HttpClient httpClient)
        {
            OAuthModel auth = configuration.GetSection("OAuthConfig").Get<OAuthModel>();
            string accessToken = "";
            if (auth != null)
            {
                var form = new Dictionary<string, string>
                {
                {"grant_type",  auth.GrantType},
                {"client_id",  auth.ClientId},
                {"client_secret",  auth.ClientSecret},
                {"scope",  auth.Scope}
                };

                HttpResponseMessage tokenResponse = await httpClient.PostAsync(auth.TokenUrl, new FormUrlEncodedContent(form));
                var jsonString = await tokenResponse.Content.ReadAsStringAsync();
                TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(jsonString);
                accessToken = token?.access_token ?? "";
            }
            return accessToken;
        }
    }
}
