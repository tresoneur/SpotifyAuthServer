using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpotifyAuthServer.Data.Context.Model;
using SpotifyAuthServer.Model;

namespace SpotifyAuthServer.Business.Model
{
    public static class TokenExtensions
    {
        public static bool IsAlmostExpired(this User user)
        {
            const int paddingSec = 5 * 60;

            return user.AccessTokenAcquiredAt.AddSeconds(user.AccessTokenExpiresInSec - paddingSec).ToUniversalTime() <= DateTime.UtcNow;
        }

        public static AuthTokenResult AsAuthTokenResult(this User user)
        {
            return new AuthTokenResult
            {
                Token = new AuthToken
                {
                    AccessToken = user.AccessToken,
                    Acquired = user.AccessTokenAcquiredAt.ToUniversalTime(),
                    ExpiresInSec = user.AccessTokenExpiresInSec
                }
            };
        }
    }
}
