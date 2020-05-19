using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyAuthServer.Data.Context.Model
{
    public class User
    {
        /// Used as a session ID in practice. One is never submitted to the database without it first being successfully used to get an access and refresh token. As they are not reusable, it should be safe to store them unencrypted.
        [Key] 
        public string Code { get; set; }

        [Encrypted]
        public string AccessToken { get; set; }

        [Required]
        public int AccessTokenExpiresInSec { get; set; }

        [Required]
        public DateTime AccessTokenAcquiredAt { get; set; }

        [Required] [Encrypted]
        public string RefreshToken { get; set; }
    }
}
