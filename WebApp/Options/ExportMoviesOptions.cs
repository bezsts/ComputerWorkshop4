namespace WebApp.Options
{
    public class ExportMoviesOptions
    {
        public string FileName { get; set; } = string.Empty;
        public CsvOptions CsvOptions { get; set; } = new();
    }
}
