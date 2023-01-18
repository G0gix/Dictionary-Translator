namespace Dictionary_Translator.Models
{
    internal class AppSettingsGoogleSheetModel
    {
        public string ApplicationName { get; set; }
        public string SheetID { get; set; }
        public string SheetName { get; set; }
        public string SecretsPath { get; set; }
        public SheetRange SheetRange { get; set; }
    }

    internal class SheetRange
    {
        public string ColumnStart { get; set; }
        public string ColumnEnd { get; set; }
    }
}
