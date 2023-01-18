using Dictionary_Translator.Models;
using Google.Apis.Sheets.v4;
using GoogleAPI_Library.Models;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Dictionary_Translator.Services.AppSettingsManager
{
    internal static class AppSettingsManager
    {
        private static IConfigurationRoot configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();

        internal static string GetValueFromSection(string sectionName)
        {
            return configuration[sectionName];
        }

        internal static (GoogleCredentialOptions_FilePath, GoogleSheetOptions) GetGoogleSheetsSetting(string rowId)
        {
            AppSettingsGoogleSheetModel model = new AppSettingsGoogleSheetModel();
            configuration.GetSection("GoogleSheetsOptions").Bind(model);

            GoogleCredentialOptions_FilePath credentialOptions = new GoogleCredentialOptions_FilePath()
            {
                ApplicationName = model.ApplicationName,
                ClientSecretPath = model.SecretsPath,
                Scopes = new string[] { SheetsService.Scope.Spreadsheets }
            };

            string sheetRange = $"{model.SheetName}!{model.SheetRange.ColumnStart ?? "A"}{rowId}:{model.SheetRange.ColumnEnd ?? "E"}";

            GoogleSheetOptions sheetOptions = new GoogleSheetOptions()
            {
                SheetId = model.SheetID,
                SheetRange = sheetRange
            };

            return (credentialOptions, sheetOptions);
        }
    }
}
