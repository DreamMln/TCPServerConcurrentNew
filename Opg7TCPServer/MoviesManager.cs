using MovieLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTMovie.Managers
{
    public class MoviesManager
    {
        /// <summary>
        /// hvis vi ikke kan oprette den statiske liste
        /// så kan vi lave en constructor og initialisere listen
        /// i. Det kan være en løsning
        /// </summary>
        
        private static int _nextId = 1;
        private static List<Movie> _movies = new List<Movie>
        {
            new Movie { Id = _nextId++, LengthInMinutes = 120, CountryOfOrigin = "USA", Name = "Dune"},
            new Movie { Id = _nextId++, LengthInMinutes = 155, CountryOfOrigin = "California", Name = "LOTR"},
            new Movie { Id = _nextId++, LengthInMinutes = 133, CountryOfOrigin = "Denmark", Name = "Kære Børn"}
        };
        
        public List<Movie> GetAll()
        {
            List<Movie> result = new List<Movie>(_movies);
            return result;
        }

        //filter
        public List<Movie> GetFilter(string countryFilter)
        {
           return _movies.FindAll(x => x.CountryOfOrigin == countryFilter);
            
        }
    }
}
