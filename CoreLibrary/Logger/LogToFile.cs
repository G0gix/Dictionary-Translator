using CoreLibrary.Logger.Exceptions;
using System;
using System.IO;

namespace CoreLibrary.Logger
{
    public class LogToFile : ILogger
    {
        public string LogFilePath { get; set; }

        public LogToFile(string logFilePath)
        {
            LogFilePath = logFilePath;
        }

        public async void Log(LogLevel logLevel, string message)
        {
            try
            {
                using (StreamWriter writeToLog = new StreamWriter(LogFilePath, true))
                {
                    string layout = $"{DateTime.Now}";

                    switch (logLevel)
                    {
                        case LogLevel.Info:
                            await writeToLog.WriteLineAsync($"{layout} | Info message: \t" + message);
                            break;
                        case LogLevel.Warning:
                            await writeToLog.WriteLineAsync($"{layout} | Warning message: \t" + message);
                            break;
                        case LogLevel.Error:
                            await writeToLog.WriteLineAsync($"{layout} | Error message: \t" + message);
                            break;
                        case LogLevel.Fatal:
                            await writeToLog.WriteLineAsync($"{layout} | Fatal message: \t" + message);
                            break;
                        case LogLevel.Successfully:
                            await writeToLog.WriteLineAsync($"{layout} | Successfully message: \t" + message);
                            break;
                        default:
                            await writeToLog.WriteLineAsync($"{layout} | Unknown message: \t" + message);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new LoggerException(ex.Message);
            }
        }
    }
}
