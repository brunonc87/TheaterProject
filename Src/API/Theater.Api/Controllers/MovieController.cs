using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Api.DTO.Movies;
using Theater.Domain.Movies;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Theater.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMoviesService _movieService;

        public MovieController(IMoviesService movieService)
        {
            _movieService = movieService;
        }

        // GET: api/<MovieController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<MovieModel> movieModels = new List<MovieModel>();
                IEnumerable<Movie> movies = _movieService.GetMovies();

                foreach (Movie movie in movies)
                {
                    MovieModel movieModel = new MovieModel();
                    movieModel.ConvertMovieToModel(movie);
                    movieModels.Add(movieModel);
                }

                return Ok(movieModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<MovieController>/tittle
        [HttpGet("{tittle}")]
        public IActionResult Get(string tittle)
        {
            try
            {
                MovieModel movieModel = new MovieModel();

                Movie movie = _movieService.GetByTittle(tittle);
                if (movie != null)
                    movieModel.ConvertMovieToModel(movie);

                return Ok(movieModel);
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
                _movieService.AddMovie(movieAddCommand.ConvertToMovie());
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
                _movieService.UpdateMovie(movieUpdateCommand.ConvertToMovie());
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
