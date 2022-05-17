using Movies.Data.Interfaces;
using Movies.Data.Models;

namespace Movies.Data.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly algebramssqlhost_moviesContext _context;
        public MovieRepository(algebramssqlhost_moviesContext context)
        {
            _context = context;
        }
        public IEnumerable<Movie> GetAll()
        {
            return _context.Movies.ToList();
        }

        public Movie GetMovieById(int id)
        {            
            return _context.Movies.FirstOrDefault(m => m.Id == id);
        }

        public Movie InsertMovie(Movie new_movie)   // bolje bi bilo da je umjesto public Movie --> public void, pa ne moramo vracati result.Entity
        {
            var result = _context.Movies.Add(new_movie);
            _context.SaveChanges();

            return result.Entity;
        }

        public Movie UpdateMovie(Movie update_movie)  // isto moze ici ovdje void
        {
            //var result = _context.Movies.Update(update_movie);
            var result = GetMovieById(update_movie.Id);

            result.Title = update_movie.Title;
            result.Genre = update_movie.Genre;
            result.ReleaseYear = update_movie.ReleaseYear;

            _context.SaveChanges();

            return result;
        }
        public Movie DeleteMovie(int id)
        {
            return null;
        }

        public IEnumerable<Movie> QueryStringFilter(string s, string orderby, int per_page)
        {
            return null;
        }
    }
}
