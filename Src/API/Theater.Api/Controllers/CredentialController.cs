using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using Theater.Application.Credentials.Commands;

namespace Theater.Api.Controllers
{
    [Route("api/Authentication")]
    [ApiController]
    public class CredentialController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CredentialController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public IActionResult Post([FromBody] CredentialCommand credentialCommand)
        {
            try
            {
                return Ok(_mediator.Send(credentialCommand));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
