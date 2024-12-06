using Microsoft.Extensions.Options;
using WebApp.Dtos.Movie;
using WebApp.Options;

namespace WebApp.Common.Exporters.Contracts
{
    public interface ICsvExporter
    {
        string ExportMovies(List<MovieViewsOutputDto> movies);
    }
}
