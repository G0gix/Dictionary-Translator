using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using GoogleAPI_Library.Exceptions;
using GoogleAPI_Library.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace GoogleAPI_Library
{
    public class GoogleSheetsManager
    {
        private SheetsService Service;
        
        public GoogleSheetsManager(GoogleCredentialOptions_FilePath options)
        {
            try
            {
                GoogleCredential credential;
                using (FileStream stream = new FileStream(options.ClientSecretPath, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream).CreateScoped(options.Scopes);
                }

                SetCredential(credential, options);
            }
            catch (Exception e)
            {
                throw new GoogleSheetsException($"Credential exception. It is not possible to set credentials. \n\n Message: {e.Message}");
            }
        }

        public GoogleSheetsManager(GoogleCredentialOptions_Stream options)
        {
            try
            {
                GoogleCredential credential = GoogleCredential.FromStream(options.ClientSecretStream);

                SetCredential(credential, options);
            }
            catch (Exception e)
            {
                throw new GoogleSheetsException($"Credential exception. It is not possible to set credentials. \n\n Message: {e.Message}");
            }
        }

        public async Task Write(GoogleSheetOptions writeOptions, IList<IList<object>> dataToInsert)
        {
            try
            {
                if (dataToInsert.Count == 0)
                {
                    throw new GoogleSheetsException("Error inserting data into Google Spreadsheet. Data is empty");
                }

                ValueRange valueRange = new ValueRange();
                valueRange.Values = dataToInsert;

                var appendRequest = Service.Spreadsheets.Values.Append(valueRange, writeOptions.SheetId, writeOptions.SheetRange);
                appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
                await appendRequest.ExecuteAsync();
                
            }
            catch (Exception ex)
            {
                throw new GoogleSheetsException(ex.Message);
            }
        }
    
        public async Task<IList<IList<Object>>> Read(GoogleSheetOptions readOptions)
        {
            try
            {
                var request = Service.Spreadsheets.Values.Get(readOptions.SheetId, readOptions.SheetRange);
                ValueRange response = await request.ExecuteAsync();

                IList<IList<Object>> values = response.Values;
                return values;
            }
            catch (Exception e)
            {
                throw new GoogleSheetsException($"Error reading data from google spreadsheet. \n\nMessage: {e.Message}");
            }
        }


        private void SetCredential(GoogleCredential credential, GoogleCredentialOptions options)
        {
            Service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = options.ApplicationName
            });
        }
    }
}
