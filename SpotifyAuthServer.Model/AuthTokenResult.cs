using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyAuthServer.Model
{
    public class AuthTokenResult
    {
        public string Error { get; set; }

        public AuthToken Token { get; set; }
    }
}
