using Movies.Data.Interfaces;
using Movies.Data.Models;

namespace Movies.Data.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        public IEnumerable<Movie> GetAll()
        {
            return null;
        }

        public Movie GetMovieById(int id)
        {
            return null;
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
