using ApiDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDomain.Repositories
{
    internal class MovieRepository : IMovieRepository
    {
        public Movie Create(Movie movie)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Movie? Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Movie> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Update(int id, Movie updatedMovie)
        {
            throw new NotImplementedException();
        }
    }
}
