using System.Linq;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DataEncryption;
using Microsoft.EntityFrameworkCore.DataEncryption.Providers;
using Microsoft.Extensions.Configuration;
using SpotifyAuthServer.Data.Context.Model;

namespace SpotifyAuthServer.Data.Context
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        private readonly AesProvider provider;

        public UserDbContext(DbContextOptions options, IConfiguration configuration)
            : base(options)
        {
            // This shouldn't be necessary, but why not.
            var bytes = new Rfc2898DeriveBytes(
                configuration["ClientSecret"],
                ToByteArray(configuration["ClientId"]));

            // No two tokens will be the same, the IV isn't crucial here.
            provider = new AesProvider(bytes.GetBytes(32), bytes.GetBytes(16));

            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseEncryption(provider);

            modelBuilder.Entity<User>()
                .ToTable("users");
        }

        private byte[] ToByteArray(string s) =>
            s.Select(c => (byte) c).ToArray();
    }
}
