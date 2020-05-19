using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SpotifyAuthServer.Controllers.Model;
using SpotifyAuthServer.Services.Model;

namespace SpotifyAuthServer.Services
{
    /// <remarks>
    /// Based on SpotifyAPI.Web.Auth/AuthorizationCodeAuth.cs
    /// </remarks>
    public class SpotifyAuthService
    {
        private readonly IConfiguration configuration;

        private const string ApiBase = "https://accounts.spotify.com/api/token";

        private string ClientAuth =>
            Convert.ToBase64String(Encoding.UTF8.GetBytes($"{configuration["ClientId"]}:{configuration["ClientSecret"]}"));

        public SpotifyAuthService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<InitialResponse> InitialRequest(AuthCodeAndCallbackUri codeAndCallbackUri)
        {
            return await Request<InitialResponse>(new Dictionary<string, string>
            {
                {"grant_type", "authorization_code"},
                {"code", codeAndCallbackUri.Code},
                {"redirect_uri", codeAndCallbackUri.CallbackUri}
            });
        }

        public async Task<RefreshResponse> RefreshRequest(string refreshToken)
        {
            return await Request<RefreshResponse>(new Dictionary<string, string>
            { 
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken }
            });
        }

        private async Task<TResponse> Request<TResponse>(Dictionary<string, string> args)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Basic {ClientAuth}");

            var response = await client.PostAsync(ApiBase, new FormUrlEncodedContent(args));
            var message = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TResponse>(message);
        }
    }
}
