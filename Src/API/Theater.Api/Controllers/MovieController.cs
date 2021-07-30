using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Theater.Application.Movies;
using Theater.Application.Movies.Commands;
using Theater.Application.Movies.Models;
using Theater.Domain.Movies;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Theater.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMoviesService _movieService;
        private readonly IMapper _mapper;

        public MovieController(IMoviesService movieService, IMapper mapper)
        {
            _movieService = movieService;
            _mapper = mapper;
        }

        // GET: api/<MovieController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                IEnumerable<Movie> movies = _movieService.GetMovies();

                return Ok(_mapper.Map<IEnumerable<MovieModel>>(movies));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<MovieController>/tittle
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                Movie movie = _movieService.GetById(id);

                return Ok(_mapper.Map<MovieModel>(movie));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<MovieController>
        [HttpPost]
        public IActionResult Add([FromBody] MovieAddCommand movieAddCommand)
        {
            try
            {
                ValidationResult result = movieAddCommand.Validate();
                if (!result.IsValid)
                    return BadRequest(result.Errors.First().ErrorMessage);

                _movieService.AddMovie(_mapper.Map<Movie>(movieAddCommand));

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<MovieController>/5
        [HttpPut]
        public IActionResult Update([FromBody] MovieUpdateCommand movieUpdateCommand)
        {
            try
            {
                ValidationResult result = movieUpdateCommand.Validate();
                if (!result.IsValid)
                    return BadRequest(result.Errors.First().ErrorMessage);

                _movieService.UpdateMovie(_mapper.Map<Movie>(movieUpdateCommand));

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<MovieController>/tittle
        [HttpDelete("{tittle}")]
        public IActionResult Delete(string tittle)
        {
            try
            {
                _movieService.DeleteMovie(tittle);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
