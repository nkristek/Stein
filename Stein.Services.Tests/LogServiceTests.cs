using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Stein.Services.Tests
{
    [TestClass]
    public class LogServiceTests
    {
        private string TestLogFolderPath
        {
            get
            {
                return null;
            }
        }

        private string TestLogMessage
        {
            get
            {
                return null;
            }
        }

        [TestMethod]
        public void TestLogFileLogging()
        {
            TestLogInfo();
            TestLogInfoAsync();

            TestLogWarning();
            TestLogWarningAsync();
        }

        private void TestLogInfo()
        {
            Assert.IsNotNull(TestLogFolderPath, "The folder path of the log folder is not set.");
            Assert.IsNotNull(TestLogMessage, "The log message for testing is not set.");

            LogService.LogFolderPath = TestLogFolderPath;
            Assert.IsFalse(File.Exists(LogService.LogFileFullName), "The log file already exists.");

            LogService.LogInfo(TestLogMessage);
            LogService.CloseLogFiles();

            Assert.IsTrue(File.Exists(LogService.LogFileFullName), "The log file doesn't exists.");
            using (var reader = new StreamReader(LogService.LogFileFullName))
                Assert.IsTrue(reader.ReadLine().Contains(TestLogMessage), "The log file doesn't contain the test message.");

            File.Delete(LogService.LogFileFullName);
        }

        private void TestLogInfoAsync()
        {
            Assert.IsNotNull(TestLogFolderPath, "The folder path of the log folder is not set.");
            Assert.IsNotNull(TestLogMessage, "The log message for testing is not set.");

            LogService.LogFolderPath = TestLogFolderPath;
            Assert.IsFalse(File.Exists(LogService.LogFileFullName), "The log file already exists.");

            LogService.LogInfoAsync(TestLogMessage).Wait();
            LogService.CloseLogFiles();

            Assert.IsTrue(File.Exists(LogService.LogFileFullName), "The log file doesn't exists.");
            using (var reader = new StreamReader(LogService.LogFileFullName))
                Assert.IsTrue(reader.ReadLine().Contains(TestLogMessage), "The log file doesn't contain the test message.");

            File.Delete(LogService.LogFileFullName);
        }

        private void TestLogWarning()
        {
            Assert.IsNotNull(TestLogFolderPath, "The folder path of the log folder is not set.");
            Assert.IsNotNull(TestLogMessage, "The log message for testing is not set.");

            LogService.LogFolderPath = TestLogFolderPath;
            Assert.IsFalse(File.Exists(LogService.LogFileFullName), "The log file already exists.");

            LogService.LogWarning(TestLogMessage);
            LogService.CloseLogFiles();

            Assert.IsTrue(File.Exists(LogService.LogFileFullName), "The log file doesn't exists.");
            using (var reader = new StreamReader(LogService.LogFileFullName))
                Assert.IsTrue(reader.ReadLine().Contains(TestLogMessage), "The log file doesn't contain the test message.");

            File.Delete(LogService.LogFileFullName);
        }

        private void TestLogWarningAsync()
        {
            Assert.IsNotNull(TestLogFolderPath, "The folder path of the log folder is not set.");
            Assert.IsNotNull(TestLogMessage, "The log message for testing is not set.");

            LogService.LogFolderPath = TestLogFolderPath;
            Assert.IsFalse(File.Exists(LogService.LogFileFullName), "The log file already exists.");

            LogService.LogWarningAsync(TestLogMessage).Wait();
            LogService.CloseLogFiles();

            Assert.IsTrue(File.Exists(LogService.LogFileFullName), "The log file doesn't exists.");
            using (var reader = new StreamReader(LogService.LogFileFullName))
                Assert.IsTrue(reader.ReadLine().Contains(TestLogMessage), "The log file doesn't contain the test message.");

            File.Delete(LogService.LogFileFullName);
        }

        [TestMethod]
        public void TestErrorLogFileLogging()
        {
            TestLogError();
            TestLogErrorAsync();
        }

        private void TestLogError()
        {
            Assert.IsNotNull(TestLogFolderPath, "The folder path of the log folder is not set.");
            Assert.IsNotNull(TestLogMessage, "The log message for testing is not set.");

            LogService.LogFolderPath = TestLogFolderPath;
            Assert.IsFalse(File.Exists(LogService.ErrorLogFileFullName), "The error log file already exists.");

            LogService.LogError(TestLogMessage);
            LogService.CloseLogFiles();

            Assert.IsTrue(File.Exists(LogService.ErrorLogFileFullName), "The error log file doesn't exists.");
            using (var reader = new StreamReader(LogService.ErrorLogFileFullName))
                Assert.IsTrue(reader.ReadLine().Contains(TestLogMessage), "The error log file doesn't contain the test message.");

            File.Delete(LogService.ErrorLogFileFullName);
        }

        private void TestLogErrorAsync()
        {
            Assert.IsNotNull(TestLogFolderPath, "The folder path of the log folder is not set.");
            Assert.IsNotNull(TestLogMessage, "The log message for testing is not set.");

            LogService.LogFolderPath = TestLogFolderPath;
            Assert.IsFalse(File.Exists(LogService.ErrorLogFileFullName), "The error log file already exists.");

            LogService.LogErrorAsync(TestLogMessage).Wait();
            LogService.CloseLogFiles();

            Assert.IsTrue(File.Exists(LogService.ErrorLogFileFullName), "The error log file doesn't exists.");
            using (var reader = new StreamReader(LogService.ErrorLogFileFullName))
                Assert.IsTrue(reader.ReadLine().Contains(TestLogMessage), "The error log file doesn't contain the test message.");

            File.Delete(LogService.ErrorLogFileFullName);
        }
    }
}
