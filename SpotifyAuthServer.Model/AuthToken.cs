using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyAuthServer.Model
{
    public class AuthToken
    {
        public string AccessToken { get; set; } = "";

        public int ExpiresInSec { get; set; }

        public DateTime Acquired { get; set; }

    }
}
