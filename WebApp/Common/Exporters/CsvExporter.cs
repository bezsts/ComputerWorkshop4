using Microsoft.Extensions.Options;
using System.Text;
using WebApp.Common.Exporters.Contracts;
using WebApp.Dtos.Movie;
using WebApp.Options;

namespace WebApp.Common.Exporters
{
    public class CsvExporter : ICsvExporter
    {
        private readonly IOptionsSnapshot<CsvOptions> _csvOptions;
        private StringBuilder csvBuilder = new StringBuilder();

        public CsvExporter(IOptionsSnapshot<CsvOptions> csvOptions)
        {
            _csvOptions = csvOptions;
        }
        public string ExportMovies(List<MovieViewsOutputDto> movies)
        {

            var csvExportOptions = _csvOptions.Value;

            movies = movies.Take(csvExportOptions.MaxExportRecords).ToList();

            AppendHeaders();

            foreach (var movie in movies)
            {
                AppendRow(movie);
            }

            return csvBuilder.ToString();
        }

        private void AppendHeaders()
        {
            csvBuilder.AppendLine(String.Join(_csvOptions.Value.Delimiter, _csvOptions.Value.FieldsToExport));
        }
        private void AppendRow(MovieViewsOutputDto movie)
        {
            var values = _csvOptions.Value.FieldsToExport.Select(field =>
                GetPropertyValueAsString(field, movie)).ToList();

            csvBuilder.AppendLine(String.Join(_csvOptions.Value.Delimiter, values));
        }
        private string? GetPropertyValueAsString(string field, MovieViewsOutputDto movie)
        {
            var property = typeof(MovieViewsOutputDto).GetProperty(field);

            if (property?.Name == "ReleaseDate")
            {
                var releaseDateValue = property.GetValue(movie);
                return releaseDateValue != null
                    ? ((DateOnly)releaseDateValue).ToString(_csvOptions.Value.DateFormat)
                    : string.Empty;
            }
            return property != null
                ? property.GetValue(movie)?.ToString() : string.Empty;
        }
    }
}
