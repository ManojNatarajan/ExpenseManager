using Newtonsoft.Json;

namespace ExpenseManagerRest.Models
{
    public class FacebookAppAccessToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }

    public class FacebookUserAccessTokenValidation
    {
        public FacebookAppInfo Data { get; set; }
    }

    public class FacebookAppInfo
    {
        [JsonProperty("app_id")]
        public string AppId { get; set; }
        public string Type { get; set; }
        public string Application { get; set; }
        [JsonProperty("data_access_expires_at")]
        public int DataAccessExpiresAt { get; set; }
        [JsonProperty("expires_at")]
        public int ExpiresAt { get; set; }
        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }
        [JsonProperty("issued_at")]
        public int IssuedAt { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

    }
}
