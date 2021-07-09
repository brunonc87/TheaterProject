using Microsoft.AspNetCore.Mvc;
using System;
using Theater.Api.DTO.Credentials;
using Theater.Domain.Credentials;

namespace Theater.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CredentialController : ControllerBase
    {
        private readonly ICredentialsService _credentialsService;

        public CredentialController(ICredentialsService credentialsService)
        {
            _credentialsService = credentialsService;
        }

        [HttpPost]
        [Route("Authenticate")]
        public IActionResult Post(CredentialCommand credentialCommand)
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
