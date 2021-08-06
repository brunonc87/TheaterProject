using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Application.Movies.Commands;
using Theater.Application.Movies.Models;
using Theater.Application.Movies.Querys;
using Theater.Domain.Movies;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Theater.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        IMediator _mediator;

        public MovieController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<MovieController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _mediator.Send(new AllMoviesQuery()));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<MovieController>/tittle
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Ok(await _mediator.Send(new MovieQuery(id)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<MovieController>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] MovieAddCommand movieAddCommand)
        {
            try
            {
                return Ok(await _mediator.Send(movieAddCommand));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<MovieController>/5
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MovieUpdateCommand movieUpdateCommand)
        {
            try
            {
                return Ok(await _mediator.Send(movieUpdateCommand));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<MovieController>/tittle
        [HttpDelete("{tittle}")]
        public async Task<IActionResult> Delete(string tittle)
        {
            try
            {
                return Ok(await _mediator.Send(new MovieDeleteCommand(tittle)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
