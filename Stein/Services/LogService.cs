using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Stein.Services
{
    public static class LogService
    {
        private static string _LogFolderPath;
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
        
        public static void LogInfo(string message)
        {
            try
            {
                LogFile.WriteLine(String.Concat(DateTime.Now.ToString(), ": INFO - ", message));
                LogFile.Flush();
            }
            catch { }
        }

        public static async Task LogInfoAsync(string message)
        {
            try
            {
                await LogFile.WriteLineAsync(String.Concat(DateTime.Now.ToString(), ": INFO - ", message));
                await LogFile.FlushAsync();
            }
            catch { }
        }

        public static void LogInfo(Exception exception)
        {
            LogInfo(BuildExceptionMessage(exception));
        }

        public static async Task LogInfoAsync(Exception exception)
        {
            await LogInfoAsync(BuildExceptionMessage(exception));
        }

        public static void LogWarning(string message)
        {
            try
            { 
                LogFile.WriteLine(String.Concat(DateTime.Now.ToString(), ": WARNING - ", message));
                LogFile.Flush();
            }
            catch { }
        }

        public static async Task LogWarningAsync(string message)
        {
            try
            {
                await LogFile.WriteLineAsync(String.Concat(DateTime.Now.ToString(), ": WARNING - ", message));
                await LogFile.FlushAsync();
            }
            catch { }
        }

        public static void LogWarning(Exception exception)
        {
            LogWarning(BuildExceptionMessage(exception));
        }

        public static async Task LogWarningAsync(Exception exception)
        {
            await LogWarningAsync(BuildExceptionMessage(exception));
        }

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
        
        public static void LogError(string message)
        {
            try
            { 
                ErrorLogFile.WriteLine(String.Concat(DateTime.Now.ToString(), ": ERROR - ", message));
                ErrorLogFile.Flush();
            }
            catch { }
        }

        public static async Task LogErrorAsync(string message)
        {
            try
            {
                await ErrorLogFile.WriteLineAsync(String.Concat(DateTime.Now.ToString(), ": ERROR - ", message));
                await ErrorLogFile.FlushAsync();
            }
            catch { }
        }

        public static void LogError(Exception exception)
        {
            LogError(BuildExceptionMessage(exception));
        }

        public static async Task LogErrorAsync(Exception exception)
        {
            await LogErrorAsync(BuildExceptionMessage(exception));
        }

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

        public static void CloseLogFiles()
        {
            LogFile = null;
            ErrorLogFile = null;
        }
    }
}
