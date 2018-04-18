using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Stein.Services
{
    public static class LogService
    {
        private static string _logFolderPath;
        /// <summary>
        /// Path to the folder in which the log files exists
        /// </summary>
        public static string LogFolderPath
        {
            get => _logFolderPath;

            set
            {
                if (!Directory.Exists(value))
                    Directory.CreateDirectory(value);
                _logFolderPath = value;
            }
        }

        /// <summary>
        /// Path to the main log file
        /// </summary>
        public static string LogFileFullName
        {
            get
            {
                if (String.IsNullOrEmpty(LogFolderPath))
                    return null;

                var dateTime = DateTime.Now;
                var fileName = $"log-{dateTime.Year}-{dateTime.Month}-{dateTime.Day}.txt";
                return Path.Combine(LogFolderPath, fileName);
            }
        }
        
        private static StreamWriter _logFile;
        /// <summary>
        /// StreamWriter to an open log file
        /// </summary>
        private static StreamWriter LogFile
        {
            get
            {
                if (_logFile == null && !String.IsNullOrEmpty(LogFileFullName))
                    _logFile = File.AppendText(LogFileFullName);
                return _logFile;
            }

            set
            {
                _logFile?.Close();
                _logFile = value;
            }
        }

        /// <summary>
        /// Write a message with an INFO prefix to the main log file
        /// </summary>
        /// <param name="message">The message which should get logged</param>
        public static void LogInfo(string message)
        {
            try
            {
                LogFile.WriteLine(String.Concat(DateTime.Now.ToString(CultureInfo.CurrentCulture), ": INFO - ", message));
                LogFile.Flush();
            }
            catch { }
        }

        /// <summary>
        /// Write a message with an INFO prefix to the main log file asynchronously
        /// </summary>
        /// <param name="message">The message which should get logged</param>
        /// <returns>Task which writes the message to the main log file</returns>
        public static async Task LogInfoAsync(string message)
        {
            try
            {
                await LogFile.WriteLineAsync(String.Concat(DateTime.Now.ToString(CultureInfo.CurrentCulture), ": INFO - ", message));
                await LogFile.FlushAsync();
            }
            catch { }
        }

        /// <summary>
        /// Write an exception with an INFO prefix to the main log file
        /// </summary>
        /// <param name="exception">The exception which should get logged</param>
        public static void LogInfo(Exception exception)
        {
            LogInfo(BuildExceptionMessage(exception));
        }

        /// <summary>
        /// Write an exception with an INFO prefix to the main log file asynchronously
        /// </summary>
        /// <param name="exception">The exception which should get logged</param>
        /// <returns>Task which writes the exception to the main log file</returns>
        public static async Task LogInfoAsync(Exception exception)
        {
            await LogInfoAsync(BuildExceptionMessage(exception));
        }

        /// <summary>
        /// Write a message with a WARNING prefix to the main log file
        /// </summary>
        /// <param name="message">The message which should get logged</param>
        public static void LogWarning(string message)
        {
            try
            { 
                LogFile.WriteLine(String.Concat(DateTime.Now.ToString(CultureInfo.CurrentCulture), ": WARNING - ", message));
                LogFile.Flush();
            }
            catch { }
        }

        /// <summary>
        /// Write a message with an WARNING prefix to the main log file asynchronously
        /// </summary>
        /// <param name="message">The message which should get logged</param>
        /// <returns>Task which writes the message to the main log file</returns>
        public static async Task LogWarningAsync(string message)
        {
            try
            {
                await LogFile.WriteLineAsync(String.Concat(DateTime.Now.ToString(CultureInfo.CurrentCulture), ": WARNING - ", message));
                await LogFile.FlushAsync();
            }
            catch { }
        }

        /// <summary>
        /// Write an exception with an WARNING prefix to the main log file
        /// </summary>
        /// <param name="exception">The exception which should get logged</param>
        public static void LogWarning(Exception exception)
        {
            LogWarning(BuildExceptionMessage(exception));
        }

        /// <summary>
        /// Write an exception with an WARNING prefix to the main log file asynchronously
        /// </summary>
        /// <param name="exception">The exception which should get logged</param>
        /// <returns>Task which writes the exception to the main log file</returns>
        public static async Task LogWarningAsync(Exception exception)
        {
            await LogWarningAsync(BuildExceptionMessage(exception));
        }

        /// <summary>
        /// Path to the error log file
        /// </summary>
        public static string ErrorLogFileFullName
        {
            get
            {
                if (String.IsNullOrEmpty(LogFolderPath))
                    return null;

                var dateTime = DateTime.Now;
                var fileName = $"error-{dateTime.Year}-{dateTime.Month}-{dateTime.Day}.txt";
                return Path.Combine(LogFolderPath, fileName);
            }
        }
        
        private static StreamWriter _ErrorLogFile;
        /// <summary>
        /// StreamWriter to an open error log file
        /// </summary>
        private static StreamWriter ErrorLogFile
        {
            get
            {
                if (_ErrorLogFile == null && !String.IsNullOrEmpty(ErrorLogFileFullName))
                    _ErrorLogFile = File.AppendText(ErrorLogFileFullName);
                return _ErrorLogFile;
            }

            set
            {
                _ErrorLogFile?.Close();
                _ErrorLogFile = value;
            }
        }

        /// <summary>
        /// Write a message to the error log file
        /// </summary>
        /// <param name="message">The message which should get logged</param>
        public static void LogError(string message)
        {
            try
            { 
                ErrorLogFile.WriteLine(String.Concat(DateTime.Now.ToString(CultureInfo.CurrentCulture), ": ERROR - ", message));
                ErrorLogFile.Flush();
            }
            catch { }
        }

        /// <summary>
        /// Write a message to the error log file asynchronously
        /// </summary>
        /// <param name="message">The message which should get logged</param>
        /// <returns>Task which writes the message to the error log file</returns>
        public static async Task LogErrorAsync(string message)
        {
            try
            {
                await ErrorLogFile.WriteLineAsync(String.Concat(DateTime.Now.ToString(CultureInfo.CurrentCulture), ": ERROR - ", message));
                await ErrorLogFile.FlushAsync();
            }
            catch { }
        }

        /// <summary>
        /// Write an exception to the error log file
        /// </summary>
        /// <param name="exception">The exception which should get logged</param>
        public static void LogError(Exception exception)
        {
            LogError(BuildExceptionMessage(exception));
        }

        /// <summary>
        /// Write an exception to the error log file asynchronously
        /// </summary>
        /// <param name="exception">The exception which should get logged</param>
        /// <returns>Task which writes the exception to the error log file</returns>
        public static async Task LogErrorAsync(Exception exception)
        {
            await LogErrorAsync(BuildExceptionMessage(exception));
        }

        /// <summary>
        /// Builds a message from the given exception and all InnerExceptions
        /// </summary>
        /// <param name="exception">The exception from which the message gets build</param>
        /// <returns>A message from the given exception and all InnerExceptions</returns>
        public static string BuildExceptionMessage(Exception exception)
        {
            var messageBuilder = new StringBuilder();

            messageBuilder.AppendLine(exception.Message);

            var innerException = exception.InnerException;
            while (innerException != null)
            {
                messageBuilder.AppendLine(innerException.Message);
                innerException = innerException.InnerException;
            }

            messageBuilder.AppendLine(exception.StackTrace);

            return messageBuilder.ToString();
        }

        /// <summary>
        /// Closes all open log files
        /// </summary>
        public static void CloseLogFiles()
        {
            LogFile = null;
            ErrorLogFile = null;
        }
    }
}
