using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using Theater.Application.Credentials.Commands;
using Theater.Domain.Credentials;

namespace Theater.Api.Controllers
{
    [Route("api/Authentication")]
    [ApiController]
    public class CredentialController : ControllerBase
    {
        private readonly ICredentialsService _credentialsService;
        private readonly IMapper _mapper;

        public CredentialController(ICredentialsService credentialsService, IMapper mapper)
        {
            _credentialsService = credentialsService;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult Post([FromBody] CredentialCommand credentialCommand)
        {
            try
            {
                bool result = _credentialsService.Authenticate(_mapper.Map<Credential>(credentialCommand));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
