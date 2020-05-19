using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyAuthServer.Controllers.Model
{
    public class AuthCodeAndCallbackUri
    {

        public string Code { get; set; }

        public string CallbackUri { get; set; }
    }
}
