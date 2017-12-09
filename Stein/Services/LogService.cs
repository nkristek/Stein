using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace nkristek.Stein.Services
{
    public static class LogService
    {
        private static string _LogFolderPath;
        /// <summary>
        /// Path to the folder in which the log files exists
        /// </summary>
        public static string LogFolderPath
        {
            get
            {
                return _LogFolderPath;
            }

            set
            {
                if (!Directory.Exists(value))
                    Directory.CreateDirectory(value);
                _LogFolderPath = value;
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
                    throw new Exception("LogFolderPath not set.");

                var dateTime = DateTime.Now;
                var fileName = String.Format("log-{0}-{1}-{2}.txt", dateTime.Year, dateTime.Month, dateTime.Day);
                return Path.Combine(LogFolderPath, fileName);
            }
        }
        
        private static StreamWriter _LogFile;
        /// <summary>
        /// StreamWriter to an open log file
        /// </summary>
        private static StreamWriter LogFile
        {
            get
            {
                if (_LogFile == null)
                    _LogFile = File.AppendText(LogFileFullName);
                return _LogFile;
            }

            set
            {
                _LogFile?.Close();
                _LogFile = value;
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
                LogFile.WriteLine(String.Concat(DateTime.Now.ToString(), ": INFO - ", message));
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
                await LogFile.WriteLineAsync(String.Concat(DateTime.Now.ToString(), ": INFO - ", message));
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
                LogFile.WriteLine(String.Concat(DateTime.Now.ToString(), ": WARNING - ", message));
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
                await LogFile.WriteLineAsync(String.Concat(DateTime.Now.ToString(), ": WARNING - ", message));
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
                var dateTime = DateTime.Now;
                var fileName = String.Format("error-{0}-{1}-{2}.txt", dateTime.Year, dateTime.Month, dateTime.Day);
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
                if (_ErrorLogFile == null)
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
                ErrorLogFile.WriteLine(String.Concat(DateTime.Now.ToString(), ": ERROR - ", message));
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
                await ErrorLogFile.WriteLineAsync(String.Concat(DateTime.Now.ToString(), ": ERROR - ", message));
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
        private static string BuildExceptionMessage(Exception exception)
        {
            var messageBuilder = new StringBuilder();

            messageBuilder.AppendLine(exception.Message);

            var innerException = exception.InnerException;
            while (innerException != null)
            {
                messageBuilder.AppendLine(innerException.Message);
                innerException = innerException.InnerException;
            }

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
