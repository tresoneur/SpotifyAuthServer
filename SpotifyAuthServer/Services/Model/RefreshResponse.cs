using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SpotifyAuthServer.Services.Model
{
    /// <remarks>
    /// Based on SpotifyAPI.Web/Models/Token.cs
    /// </remarks>
    public class RefreshResponse
    {
        public RefreshResponse()
        {
            AcquiredAt = DateTime.Now;
        }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresInSec { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }

        public DateTime AcquiredAt { get; set; }
    }
}
