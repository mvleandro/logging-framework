using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenJobs.LoggingFramework.Abstractions;
using System;

namespace OpenJobs.LoggingFramework.Abstractions.UnitTests
{
    [TestClass]
    public class LogEntryTest
    {
        [TestMethod]
        public void Factory_sucess()
        {
            LogEntry logEntry = LogEntry.New("Unit test log message.", LogLevel.Information, "abc");
            Console.WriteLine(logEntry.Message);

            Assert.AreEqual("Unit test log message.", logEntry.Message);
            Assert.AreEqual(LogLevel.Information, logEntry.Level);
            Assert.AreEqual("Microsoft Corporation", logEntry.ProductCompany);
            Assert.AreEqual("Microsoft.TestHost", logEntry.ProductName);
            Assert.IsNotNull(logEntry.MachineName);
            Assert.IsNotNull(logEntry.ProcessId);
            Assert.IsNotNull(logEntry.ProcessName);
            Assert.IsNotNull(logEntry.ProcessPath);
            Assert.IsNotNull(logEntry.ThreadId);
            Assert.AreEqual("LogEntry", logEntry.TypeName);
            Assert.IsTrue(logEntry.Tags.Contains("abc"));
        }

        [TestMethod]
        public void OverrideProductInformation_test()
        {
            LogEntry.OverrideProductInformation("Marcus", "LogFramework", "0.1.0");

            LogEntry logEntry = LogEntry.New("Unit test log message.");
            Console.WriteLine(logEntry.Message);

            Assert.AreEqual("Unit test log message.", logEntry.Message);
            Assert.AreEqual(LogLevel.Information, logEntry.Level);
            Assert.AreEqual("Marcus", logEntry.ProductCompany);
            Assert.AreEqual("LogFramework", logEntry.ProductName);
            Assert.AreEqual("0.1.0", logEntry.ProductVersion);
            Assert.IsNotNull(logEntry.MachineName);
            Assert.IsNotNull(logEntry.ProcessId);
            Assert.IsNotNull(logEntry.ProcessName);
            Assert.IsNotNull(logEntry.ProcessPath);
            Assert.IsNotNull(logEntry.ThreadId);
            Assert.AreEqual("LogEntry", logEntry.TypeName);
        }
    }
}
