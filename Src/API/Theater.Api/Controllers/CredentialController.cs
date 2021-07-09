using Microsoft.AspNetCore.Mvc;
using System;
using Theater.Api.DTO.Credentials;
using Theater.Domain.Credentials;

namespace Theater.Api.Controllers
{
    [Route("api/Authentication")]
    [ApiController]
    public class CredentialController : ControllerBase
    {
        private readonly ICredentialsService _credentialsService;

        public CredentialController(ICredentialsService credentialsService)
        {
            _credentialsService = credentialsService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] CredentialCommand credentialCommand)
        {
            try
            {
                bool result = _credentialsService.Authenticate(credentialCommand.Login, credentialCommand.Password);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
