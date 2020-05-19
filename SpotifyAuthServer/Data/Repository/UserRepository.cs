using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SpotifyAuthServer.Data.Context;
using SpotifyAuthServer.Data.Context.Model;
using SpotifyAuthServer.Data.Repository.Interfaces;

namespace SpotifyAuthServer.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext db;

        public DatabaseFacade Database => db.Database;

        public UserRepository(UserDbContext userDbContext)
        {
            db = userDbContext;
        }

        public async Task<bool> Register(User user)
        {
            await using (var tran = await db.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable))
            {
                if (db.Users.Any(u => u.Code.Equals(user.Code)))
                    return false;

                db.Users.Add(user);

                await db.SaveChangesAsync();
                await tran.CommitAsync();

                return true;
            }
        }

        public async Task<bool> Update(string code, string authToken, int authTokenExpiresIn, DateTime authTokenAcquired)
        {
            await using (var tran = await db.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead))
            {
                var user = Get(code);
                
                if(user is null)
                    return false;

                user.AccessToken = authToken;
                user.AccessTokenExpiresInSec = authTokenExpiresIn;
                user.AccessTokenAcquiredAt = authTokenAcquired;

                db.Users.Update(user);

                await db.SaveChangesAsync();
                await tran.CommitAsync();

                return true;
            }
        }

        public User Get(string code) =>
            db.Users.FirstOrDefault(u => u.Code.Equals(code));
    }
}
