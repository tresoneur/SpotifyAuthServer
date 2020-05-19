using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SpotifyAuthServer.Services.Model
{
    /// <remarks>
    /// Based on SpotifyAPI.Web/Models/Token.cs
    /// </remarks>
    public class InitialResponse
    {
        public InitialResponse()
        {
            AcquiredAt = DateTime.Now;
        }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresInSec { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }

        public DateTime AcquiredAt { get; set; }
    }
}
