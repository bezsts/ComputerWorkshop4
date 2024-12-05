namespace WebApp.Options
{
    public class CsvOptions
    {
        public char Delimiter { get; set; }
        public string DateFormat { get; set; } = string.Empty;
        public int MaxExportRecords { get; set; }
        public string[] FieldsToExport { get; set; } = [];
    }
}
