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
            // Autoincrement za Id
            int new_id = 1;
            new_movie.Id = GetAll().Max(m => m.Id) + 1;
            
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
            var find_movie = GetMovieById(id);

            var result = _context.Movies.Remove(find_movie);
            _context.SaveChanges();

            return result.Entity;
        }

        public IEnumerable<Movie> QueryStringFilter(string s, string orderby, int per_page, int page)
        {
            // 1. dohvati filmove po parametru s 
            var results = _context.Movies.Where(m => m.Title.Contains(s));

            if (results.Any())
            {
                // 2. Poredaj rezultate po parametru "orderby"
                switch (orderby)
                {
                    case "desc":
                        results = results.OrderByDescending(m => m.Id);
                        break;
                    default:
                        results = results.OrderBy(m => m.Id);
                        break;
                }

                // 3. listaj rezultate po parametru per_page
                    // TODO: provjeriti gresku kod page = 0
                if (per_page != 0 && per_page > 0)
                {
                    results = results.Skip((page - 1) * per_page).Take(per_page);
                }

            }


            return results;
        }
    }
}
