using CoreLibrary.Logger.Exceptions;
using System;

namespace CoreLibrary.Logger
{
    /// <summary>
    /// Represents the ability to print messages to the console.
    /// </summary>
    public class LogToConsole : ILogger
    {
        /// <summary>
        /// Prints messages to the console.
        /// </summary>
        /// <param name="logLevel">Message output level. 
        /// Info - Gray,
        /// Warning - Yellow,
        /// Error - Red,
        /// Fatal - DarkRed,
        /// Successfully - Green
        /// </param>
        /// <param name="message">Message to display in console</param>
        public void Log(LogLevel logLevel, string message)
        {
            try
            {
                switch (logLevel)
                {
                    case LogLevel.Info:
                        ShowToConsole(ConsoleColor.Gray, message);
                        break;
                    case LogLevel.Warning:
                        ShowToConsole(ConsoleColor.Yellow, message);
                        break;
                    case LogLevel.Error:
                        ShowToConsole(ConsoleColor.Red, message);
                        break;
                    case LogLevel.Fatal:
                        ShowToConsole(ConsoleColor.DarkRed, message);
                        break;
                    case LogLevel.Successfully:
                        ShowToConsole(ConsoleColor.Green, message);
                        break;
                    default:
                        ShowToConsole(ConsoleColor.Gray, message);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new LoggerException(ex.Message);
            }
        }

        private void ShowToConsole(ConsoleColor color, string message)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
