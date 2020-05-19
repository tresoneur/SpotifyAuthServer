using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpotifyAuthServer.Business;
using SpotifyAuthServer.Controllers.Model;
using SpotifyAuthServer.Model;

namespace SpotifyAuthServer.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager users;

        public AuthController(UserManager userManager)
        {
            users = userManager;
        }

        // Won't use query parameters, web.config has a pretty sane (but relatively short) length limit.

        [HttpPost("register")]
        public async Task<ActionResult<AuthTokenResult>> Register([FromBody] AuthCodeAndCallbackUri codeAndCallbackUri)
        {
            return await Execute(() => users.Register(codeAndCallbackUri));
        }

        [HttpPost("token")] // May alter state on the server, doesn't satisfy the safety requirement of GET.
        public async Task<ActionResult<AuthTokenResult>> GetToken([FromBody] AuthCode code)
        {
            return await Execute(() => users.GetToken(code));
        }

        private async Task<ActionResult<AuthTokenResult>> Execute(Func<Task<AuthTokenResult>> action)
        {
            var result = await action();

            if (string.IsNullOrEmpty(result.Error))
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
