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


        // GET: api/movies/2
        //[Route("~/api/[controller]/{id:int}")] // ne treba sve jer smo vec na pocetku stavili rutu
        [Route("{id:int}")]
        [HttpGet]  // moze se i ovdje dodati ("{id:int}")
        public ActionResult<IEnumerable<Movie>> GetMovie(int id)
        {

            try
            {
                var movie = _movieRepository.GetMovieById(id);

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


        // POST: api/movies
        [HttpPost]
        public ActionResult PostMovie(Movie new_movie)
        {
            // 1. odgovor -> Unos je uspio, HTTP 200
            // 2. odgovor -> validacija podatka nije dobra, HTTP 400
            // 3. odgovor -> greska na serveru, HTTP 500
            try
            {
                if (!ModelState.IsValid)
                {                    
                    return BadRequest(ModelState);
                }
                // Napomena: svojstvo primarnog kljuca nije auto increment!
                // DZ: programsko rjesenje za provjeru max vrijednosti svojstva id zapisa u tablici, te uvecanje za 1 tik prije kreiranja novog zapisa
                // RJESENJE je u MovieRepository klasi u metodi InsertMovie
                
                _movieRepository.InsertMovie(new_movie);

                return Ok("Zapis je kreiran!");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return Ok();

        }


        // PUT: api/movies/5
        [HttpPut("{id:int}")]
        public ActionResult PutMovie(int id, Movie update_movie)
        {
            // 1. Provjera Id-a (postoji li zapis u tablici)
            // 2. Validacija podataka
            // 3. Provjera ako su parametar id i update_movie.id razliciti
            // 4. U slucaju greske ili iznimke, vrati http 500

            try
            {
                if (id != update_movie.Id)
                {
                    return BadRequest("Parametri ID se ne poklapaju");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Podaci nisu validni!");
                }

                var find_movie = _movieRepository.GetMovieById(id);

                if (find_movie == null)
                {
                    return NotFound("Zapis nije pronađen!");
                }

                return Ok(_movieRepository.UpdateMovie(update_movie));


            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška kod ažuriranja podatka!");
            }


            return Ok();
        }

        // DZ!!! !!!
        // DELETE: api/movies/5
        [HttpDelete("{id:int}")]
        public ActionResult DeleteMovie(int id)
        {
            try
            {
                var movie = _movieRepository.GetMovieById(id);

                if (movie == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Rezultat nije pronađen!");
                }

                _movieRepository.DeleteMovie(id);
                return Ok("Zapisan je izbrisan!");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Nije moguće prikazati rezultate, dogodila se greška!");
            }
        }


    }
}
