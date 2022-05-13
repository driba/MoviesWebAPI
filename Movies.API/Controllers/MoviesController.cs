using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Data.Interfaces;
using Movies.Data.Models;

namespace Movies.API.Controllers
{
    // RUTA: localhost/
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        public readonly IMovieRepository _movieRepository;

        public MoviesController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        // GET: api/Movies
        [HttpGet]
        public ActionResult<IEnumerable<Movie>> GetMovies()
        {
            try
            {
                return Ok(_movieRepository.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            };
        }
    }
}
