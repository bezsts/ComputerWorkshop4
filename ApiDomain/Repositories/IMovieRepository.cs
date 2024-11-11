using ApiDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDomain.Repositories
{
    public interface IMovieRepository
    {
        IEnumerable<Movie> GetAll();
        Movie? Get(int id);
        Movie Create(Movie movie);
        bool Update(int id, Movie updatedMovie);
        bool Delete(int id);
    }
}
