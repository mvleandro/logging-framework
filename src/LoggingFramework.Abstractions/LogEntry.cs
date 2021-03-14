using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace OpenJobs.LoggingFramework.Abstractions
{
    /// <summary>
    /// Represents a default log entry.
    /// </summary>
    public class LogEntry
    {
        #region Public properties

        /// <summary>
        /// The application's company name.
        /// </summary>
        public string ProductCompany { get; private set; } = _productCompany;
        private static string _productCompany;

        /// <summary>
        /// The application name.
        /// </summary>
        public string ProductName { get; private set; } = _productName;
        private static string _productName;

        /// <summary>
        /// The application version.
        /// </summary>
        public string ProductVersion { get; private set; } = _productVersion;
        private static string _productVersion;

        /// <summary>
        /// The LogEntry type.
        /// </summary>
        public string TypeName { get => this.GetType().Name; }

        /// <summary>
        /// The LogEntry level.
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        /// The LogEntry message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The request/execution TraceKey.
        /// </summary>
        public Guid? TraceKey { get; set; }

        /// <summary>
        /// The machine name.
        /// </summary>
        public string MachineName { get => _machineName; }
        private static string _machineName;

        /// <summary>
        /// The thread name.
        /// </summary>
        public string ThreadName { get => Thread.CurrentThread.Name; }

        /// <summary>
        /// The thread id.
        /// </summary>
        public int ThreadId { get => Thread.CurrentThread.ManagedThreadId; }

        /// <summary>
        /// The process name.
        /// </summary>
        public string ProcessName { get => _processName; }
        private static string _processName;

        /// <summary>
        /// The process path.
        /// </summary>
        public string ProcessPath { get => _processPath; }
        private static string _processPath;

        /// <summary>
        /// The process id.
        /// </summary>
        public int ProcessId { get => _processId; }
        private static int _processId;

        /// <summary>
        /// A list of tags.
        /// </summary>
        public HashSet<string> Tags { get; } = new HashSet<string>();

        /// <summary>
        /// A dictionary with additional informations.
        /// </summary>
        public Dictionary<string, object> AdditionalInformation { get; } = new Dictionary<string, object>();

        #endregion

        #region Public constructor

        /// <summary>
        /// The basic constructor.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="level">The log level.</param>
        /// <param name="tags">A list with tags to add.</param>
        public LogEntry(string message, LogLevel level = LogLevel.Information, params string[] tags)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            this.Message = message;
            this.Level = level;
            this.AddTags(tags);            

            if(
                string.IsNullOrWhiteSpace(ProductCompany) ||
                string.IsNullOrWhiteSpace(ProductName) ||
                string.IsNullOrWhiteSpace(ProductVersion)
              )
            {
                throw new ApplicationException($"You must provide '{nameof(ProductCompany)}', '{nameof(ProductName)}' and '{nameof(ProductVersion)}' information.");
            }
        }

        #endregion

        #region Static constructor

        /// <summary>
        /// The static constructor.
        /// </summary>
        static LogEntry()
        {
            _processId = Process.GetCurrentProcess().Id;
            _processName = Process.GetCurrentProcess().ProcessName.Split(Path.DirectorySeparatorChar).Last();
            _processPath = Process.GetCurrentProcess().MainModule.FileName;
            _machineName = Environment.MachineName;

            Assembly entryAssembly = Assembly.GetEntryAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(entryAssembly.Location);

            _productCompany = fileVersionInfo.CompanyName;
            _productName = fileVersionInfo.ProductName;
            _productVersion = fileVersionInfo.ProductVersion;          
        }

        #endregion

        #region Factory

        /// <summary>
        /// Creates a new LogEntry instance.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="tags">A list with tags.</param>
        /// <returns>A LogEntry instance.</returns>
        public static LogEntry New(string message, LogLevel level = LogLevel.Information, params string[] tags)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            return new LogEntry(message, level, tags);            
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Set the log message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The LogEntry instance.</returns>
        public LogEntry SetMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            this.Message = message;
            return this;
        }

        /// <summary>
        /// Set the log level.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <returns>The LogEntry instance.</returns>
        public LogEntry SetLevel(LogLevel logLevel)
        {
            this.Level = logLevel;
            return this;
        }

        /// <summary>
        /// Set the tracekey.
        /// </summary>
        /// <param name="traceKey">The traceKey.</param>
        /// <returns>The LogEntry instance.</returns>
        public LogEntry SetTraceKey(Guid traceKey)
        {
            this.TraceKey = traceKey;
            return this;
        }

        /// <summary>
        /// Add a new additional information.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">the value.</param>
        /// <returns>The LogEntry instance.</returns>
        public LogEntry AddAdditionalInformation(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }            

            this.AdditionalInformation.Add(key, value);
            return this;
        }

        /// <summary>
        /// Add tags to the LogEntry.
        /// </summary>
        /// <param name="tags">A list with tags to add.</param>
        /// <returns>The LogEntry instance.</returns>
        public LogEntry AddTags(params string[] tags)
        {
            if (tags?.Length > 0)
            {
                foreach (var tag in tags)
                {
                    this.Tags.Add(tag);
                }
            }
            
            return this;
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Overrides the assembly product information.
        /// </summary>
        /// <param name="productCompany">The product company.</param>
        /// <param name="productName">The product name.</param>
        /// <param name="productVersion">The product version.</param>
        public static void OverrideProductInformation(string productCompany, string productName, string productVersion)
        {
            _productCompany = productCompany;
            _productName = productName;
            _productVersion = productVersion;
        }

        #endregion
    }
}
