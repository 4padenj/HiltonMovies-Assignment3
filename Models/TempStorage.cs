using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiltonMovies.Models
{
    public class TempStorage
    {
        private static List<Movie> movies = new List<Movie>();
        public static IEnumerable<Movie> Movies => movies;
        public static void AddMovie(Movie movie)
        {
            movies.Add(movie);
        }
                
    }
}
