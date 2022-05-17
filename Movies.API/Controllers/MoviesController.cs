using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Data.Interfaces;
using Movies.Data.Models;

namespace Movies.API.Controllers
{
    // RUTA: localhost/8000/api/Movies
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
        // GET: api/Movies/2
        [Route("~/api/[controller]/{id:int}")]
        [HttpGet]
        public ActionResult<IEnumerable<Movie>> GetMovieById(int id)
        {
            Movie? movie = _movieRepository.GetMovieById(id);
            try
            {
                if (movie == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Rezultat nije pronađen!");
                }
                return Ok(movie);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Nije moguće prikazati rezultate, dogodila se greška!");
            }
            
        }
    }
}
