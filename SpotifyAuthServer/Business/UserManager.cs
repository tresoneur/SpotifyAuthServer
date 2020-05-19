using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using SpotifyAuthServer.Business.Model;
using SpotifyAuthServer.Controllers.Model;
using SpotifyAuthServer.Data.Context.Model;
using SpotifyAuthServer.Data.Repository;
using SpotifyAuthServer.Data.Repository.Interfaces;
using SpotifyAuthServer.Model;
using SpotifyAuthServer.Services;

namespace SpotifyAuthServer.Business
{
    public class UserManager
    {
        private readonly IUserRepository db;
        private readonly SpotifyAuthService spotify;

        public UserManager(IUserRepository dbContext, SpotifyAuthService spotifyAuthServer)
        {
            db = dbContext;
            spotify = spotifyAuthServer;
        }

        public async Task<AuthTokenResult> Register(AuthCodeAndCallbackUri codeAndCallbackUri)
        {
            var response = await spotify.InitialRequest(codeAndCallbackUri);

            if (!string.IsNullOrEmpty(response.Error))
                return Error($"{response.Error}: {response.ErrorDescription}");

            var dbUser = new User
            {
                Code = codeAndCallbackUri.Code,
                AccessToken = response.AccessToken,
                AccessTokenExpiresInSec = response.ExpiresInSec,
                AccessTokenAcquiredAt = response.AcquiredAt,
                RefreshToken = response.RefreshToken
            };

            if (!await db.Register(dbUser))
                return Error("User most likely already registered.");

            return new AuthTokenResult
            {
                Token = new AuthToken
                {
                    AccessToken = response.AccessToken, 
                    Acquired = response.AcquiredAt.ToUniversalTime(),
                    ExpiresInSec = response.ExpiresInSec
                }
            };
        }

        public async Task<AuthTokenResult> GetToken(AuthCode code)
        {
            await using (var tran = await db.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead))
            {
                var dbUser = db.Get(code.Code);

                if (dbUser is null)
                    return Error("User not registered.");

                if (!dbUser.IsAlmostExpired())
                    return dbUser.AsAuthTokenResult();

                var response = await spotify.RefreshRequest(dbUser.RefreshToken);

                if (!string.IsNullOrEmpty(response.Error))
                    return Error($"{response.Error}: {response.ErrorDescription}");

                if (!await db.Update(code.Code, response.AccessToken, response.ExpiresInSec, response.AcquiredAt))
                    return Error("This should never happen. 2020 just keeps getting worse.");

                await tran.CommitAsync();

                return db.Get(code.Code).AsAuthTokenResult();
            }
        }

        private AuthTokenResult Error(string error) =>
            new AuthTokenResult { Error = error };
}
}
