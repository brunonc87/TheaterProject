using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
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
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] CredentialCommand credentialCommand)
        {
            try
            {
                return Ok(await _mediator.Send(credentialCommand));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
