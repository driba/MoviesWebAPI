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

        public Movie InsertMovie(Movie new_movie)
        {
            return null;
        }

        public Movie UpdateMovie(Movie update_movie)
        {
            return null;
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
