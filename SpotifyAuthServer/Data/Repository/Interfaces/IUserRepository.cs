using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SpotifyAuthServer.Data.Context.Model;

namespace SpotifyAuthServer.Data.Repository.Interfaces
{
    public interface IUserRepository
    {
        public DatabaseFacade Database { get; }

        public Task<bool> Register(User user);

        public Task<bool> Update(string code, string authToken, int authTokenExpiresIn, DateTime authTokenAcquired);

        public User Get(string code);
    }
}
