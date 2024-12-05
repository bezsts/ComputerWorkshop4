namespace WebApp.Options
{
    //TODO: додати валідацію до конфігурації
    public class ExportMoviesOptions
    {
        public string FileName { get; set; } = string.Empty;
        public CsvOptions CsvOptions { get; set; } = new();
    }
}
